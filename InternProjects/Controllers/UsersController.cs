using InternProjects.Data;
using InternProjects.Models;
using InternProjects.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InternProjects.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .OrderBy(u => u.Role).ThenBy(u => u.FirstName)
                .Select(u => new UserListItemViewModel
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName,
                    Email = u.Email,
                    Role = u.Role,
                    Status = u.Status,
                    LastLogin = u.LastLogin,
                    InternId = _context.Interns
                        .Where(i => i.UserId == u.Id)
                        .Select(i => (int?)i.Id)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return View(users);
        }

        [HttpGet]
        public IActionResult Create() => View(new UserCreateViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            bool emailTaken = await _context.Users.AnyAsync(u => u.Email == model.Email);
            if (emailTaken)
                ModelState.AddModelError(nameof(model.Email), "Потребител с този имейл вече съществува.");

            if (model.Role != "Admin" && model.Role != "Intern")
                ModelState.AddModelError(nameof(model.Role), "Невалидна роля.");

            if (model.Role == "Intern" && model.EndDate != null && model.EndDate < model.StartDate)
                ModelState.AddModelError(nameof(model.EndDate), "Краят не може да е преди началото.");

            if (!ModelState.IsValid) return View(model);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber ?? "",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Role = model.Role,
                    Status = "Активен",
                    CreationDate = DateTime.Now
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                if (model.Role == "Intern")
                {
                    _context.Interns.Add(new Intern
                    {
                        UserId = user.Id,
                        University = model.University,
                        Specialty = model.Specialty,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        TotalHours = model.TotalHours,
                        TaskHours = 0,
                        AddedHours = 0,
                        ReportedHours = 0,
                        RemainingHours = model.TotalHours,
                        Notes = model.Notes
                    });
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "Грешка при създаването. Нищо не е записано - опитай пак.");
                return View(model);
            }

            TempData["Success"] = $"{model.FirstName} {model.LastName} ({model.Role}) е създаден.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var vm = new UserEditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            var user = await _context.Users.FindAsync(model.Id);
            if (user == null) return NotFound();

            bool emailTaken = await _context.Users
                .AnyAsync(u => u.Email == model.Email && u.Id != model.Id);
            if (emailTaken)
                ModelState.AddModelError(nameof(model.Email), "Имейлът е зает от друг потребител.");

            if (!ModelState.IsValid) return View(model);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber ?? "";
            user.UpdateDate = DateTime.Now;

            if (!string.IsNullOrEmpty(model.NewPassword))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

            await _context.SaveChangesAsync();

            TempData["Success"] = $"{user.FirstName} {user.LastName} е обновен"
                + (!string.IsNullOrEmpty(model.NewPassword) ? " (с нова парола)." : ".");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (id == currentUserId)
            {
                TempData["Error"] = "Не можеш да деактивираш собствения си акаунт.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Status = user.Status == "Активен" ? "Неактивен" : "Активен";
            user.UpdateDate = DateTime.Now;
            await _context.SaveChangesAsync();

            TempData["Success"] = user.Status == "Неактивен"
                ? $"{user.FirstName} {user.LastName} е деактивиран — не може да влиза в системата."
                : $"{user.FirstName} {user.LastName} е активиран.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (id == currentUserId)
            {
                TempData["Error"] = "Не можете да изтриете собствения си акаунт.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                TempData["Error"] = "Потребителят не беше намерен.";
                return RedirectToAction(nameof(Index));
            }

            var intern = await _context.Interns
                .FirstOrDefaultAsync(i => i.UserId == id);

            if (intern != null)
            {
                var assignments = await _context.TaskAssignments
                    .Include(a => a.Task)
                    .Where(a => a.InternId == intern.Id)
                    .ToListAsync();

                foreach (var assignment in assignments)
                {
                    if (assignment.Task != null)
                    {
                        assignment.Task.Status = "Свободна";
                    }

                    _context.TaskAssignments.Remove(assignment);
                }

                _context.Interns.Remove(intern);
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Потребителят беше изтрит успешно.";
            return RedirectToAction(nameof(Index));
        }
    }
}