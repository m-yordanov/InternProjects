using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using InternProjects.Data;
using InternProjects.Models;
using InternProjects.Models.ViewModels;

namespace InternProjects.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManualHoursController : Controller
    {
        private readonly AppDbContext _context;

        public ManualHoursController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? internId)
        {
            var vm = new ManualHoursViewModel { InternId = internId };
            await FillInterns(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ManualHoursViewModel model)
        {
            if (model.Date > DateTime.Today)
                ModelState.AddModelError(nameof(model.Date), "Датата не може да е в бъдещето.");

            var intern = await _context.Interns
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.Id == model.InternId);

            if (intern == null)
                ModelState.AddModelError(nameof(model.InternId), "Невалиден стажант.");

            if (!ModelState.IsValid)
            {
                await FillInterns(model);
                return View(model);
            }

            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var hours = model.Hours!.Value;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.TimeLogs.Add(new TimeLog
                {
                    SourceType = "manual",
                    InternId = intern!.Id,
                    AssignmentId = null,
                    Hours = hours,
                    Date = model.Date,
                    Description = string.IsNullOrWhiteSpace(model.Note)
                        ? model.Description
                        : $"{model.Description} (Бележка: {model.Note})",
                    StatusApproval = "Одобрен",
                    CreatedById = adminId,
                    ApprovedById = adminId,
                    CreationDate = DateTime.Now
                });

                intern.AddedHours += hours;
                intern.ReportedHours = intern.TaskHours + intern.AddedHours;
                intern.RemainingHours = Math.Max(0, intern.TotalHours - intern.ReportedHours);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                TempData["Error"] = "Грешка при записа. Нищо не е добавено - опитай пак.";
                await FillInterns(model);
                return View(model);
            }

            TempData["Success"] =
                $"+{hours} ч. добавени на {intern.User?.FirstName} {intern.User?.LastName}.";

            return RedirectToAction("Details", "InternProfile", new { id = intern.Id });
        }

        private async Task FillInterns(ManualHoursViewModel vm)
        {
            vm.Interns = await _context.Interns
                .Include(i => i.User)
                .Where(i => i.User != null && i.User.Status == "Активен")
                .OrderBy(i => i.User!.FirstName)
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.User!.FirstName + " " + i.User.LastName
                })
                .ToListAsync();
        }
    }
}