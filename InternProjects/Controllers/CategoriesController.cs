using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternProjects.Data;
using InternProjects.Models;
using InternProjects.Models.ViewModels;

namespace InternProjects.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new CategoryListItemViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    TaskCount = _context.TaskItems.Count(t => t.CategoryId == c.Id)
                })
                .ToListAsync();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create() => View(new Category());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                ModelState.AddModelError(nameof(model.Name), "Името е задължително.");

            bool exists = await _context.Categories.AnyAsync(c => c.Name == model.Name);
            if (exists)
                ModelState.AddModelError(nameof(model.Name), "Категория с това име вече съществува.");

            if (!ModelState.IsValid) return View(model);

            _context.Categories.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Категорията „{model.Name}\" е създадена.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category model)
        {
            var category = await _context.Categories.FindAsync(model.Id);
            if (category == null) return NotFound();

            if (string.IsNullOrWhiteSpace(model.Name))
                ModelState.AddModelError(nameof(model.Name), "Името е задължително.");

            bool duplicate = await _context.Categories
                .AnyAsync(c => c.Name == model.Name && c.Id != model.Id);
            if (duplicate)
                ModelState.AddModelError(nameof(model.Name), "Категория с това име вече съществува.");

            if (!ModelState.IsValid) return View(model);

            category.Name = model.Name;
            category.Description = model.Description;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Категорията „{category.Name}\" е обновена.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                TempData["Error"] = "Категорията не е намерена.";
                return RedirectToAction(nameof(Index));
            }

            bool hasTasks = await _context.TaskItems
                .AnyAsync(t => t.CategoryId == id);

            if (hasTasks)
            {
                ViewData["Error"] =
                    $"Категорията „{category.Name}\" не може да бъде изтрита, защото има свързани задачи.";

                return View("Edit", category);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            TempData["Success"] =
                $"Категорията „{category.Name}\" е изтрита.";

            return RedirectToAction(nameof(Index));
        }
    }
}