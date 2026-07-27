using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using InternProjects.Data;
using InternProjects.Models.ViewModels;

namespace InternProjects.Controllers
{
    [Authorize]
    public class InternProfileController : Controller
    {
        private readonly AppDbContext _context;

        public InternProfileController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            return await BuildProfile(id);
        }

        [Authorize(Roles = "Intern")]
        public async Task<IActionResult> My()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var intern = await _context.Interns
                .FirstOrDefaultAsync(i => i.UserId == userId);

            if (intern == null) return NotFound();
            return await BuildProfile(intern.Id, viewName: "Details");
        }

        private async Task<IActionResult> BuildProfile(int internId, string? viewName = null)
        {
            var intern = await _context.Interns
                .Include(i => i.User)
                .Include(i => i.Mentor)
                .FirstOrDefaultAsync(i => i.Id == internId);

            if (intern == null) return NotFound();

            var assignments = await _context.TaskAssignments
                .Include(a => a.Task).ThenInclude(t => t.Category)
                .Where(a => a.InternId == internId)
                .OrderByDescending(a => a.StartDate)
                .ToListAsync();

            var timeLogs = await _context.TimeLogs
                .Include(t => t.CreatedBy)
                .Where(t => t.InternId == internId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            var vm = new InternProfileViewModel
            {
                Intern = intern,
                Assignments = assignments,
                TimeLogs = timeLogs,
                AcceptedCount = assignments.Count(a => a.Status == "Приета"),
                InProgressCount = assignments.Count(a => a.Status == "В процес"
                    || a.Status == "Предадена за проверка"),
                ReturnedCount = assignments.Count(a => a.Status == "Върната за корекция")
            };

            return View(viewName ?? "Details", vm);
        }
    }
}