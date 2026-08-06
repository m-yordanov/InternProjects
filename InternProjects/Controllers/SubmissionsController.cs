using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternProjects.Data;
using InternProjects.Models;
using InternProjects.Models.ViewModels;

namespace InternProjects.Controllers
{
    [Authorize(Roles = "Intern")]
    public class SubmissionsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        private static readonly string[] AllowedExtensions =
            { ".pdf", ".docx", ".png", ".jpg", ".jpeg", ".zip" };
        private const long MaxFileSize = 20 * 1024 * 1024;

        public SubmissionsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        private async Task<TaskAssignment?> GetOwnAssignment(int assignmentId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var intern = await _context.Interns.FirstOrDefaultAsync(i => i.UserId == userId);
            if (intern == null) return null;

            return await _context.TaskAssignments
                .Include(a => a.Task)
                .FirstOrDefaultAsync(a => a.Id == assignmentId && a.InternId == intern.Id);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int assignmentId)
        {
            var assignment = await GetOwnAssignment(assignmentId);
            if (assignment == null) return NotFound(); 

            if (assignment.Status != "В процес" && assignment.Status != "Върната за корекция")
            {
                TempData["Error"] = "Тази задача не е в състояние за предаване.";
                return RedirectToAction("Intern", "Dashboard");
            }

            var vm = new SubmissionCreateViewModel
            {
                AssignmentId = assignment.Id,
                TaskTitle = assignment.Task?.Title ?? "",
                SubmissionFormat = assignment.Task?.SubmissionFormat
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubmissionCreateViewModel model)
        {
            var assignment = await GetOwnAssignment(model.AssignmentId);
            if (assignment == null) return NotFound();

            if (assignment.Status != "В процес" && assignment.Status != "Върната за корекция")
            {
                TempData["Error"] = "Тази задача не е в състояние за предаване.";
                return RedirectToAction("Intern", "Dashboard");
            }

            bool hasContent = !string.IsNullOrWhiteSpace(model.SubmittedText)
                || !string.IsNullOrWhiteSpace(model.SubmittedLink)
                || (model.File != null && model.File.Length > 0)
                || (model.Photos != null && model.Photos.Any(p => p.Length > 0));

            if (!hasContent)
            {
                ModelState.AddModelError("", "Предай поне едно: текст, линк, файл или снимка.");
            }

            string? filePath = null;
            string? photoPaths = null;

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.File != null && model.File.Length > 0)
                        filePath = await SaveFile(model.File);

                    if (model.Photos != null)
                    {
                        var saved = new List<string>();
                        foreach (var photo in model.Photos.Where(p => p.Length > 0))
                        {
                            var ext = Path.GetExtension(photo.FileName).ToLowerInvariant();
                            if (ext != ".png" && ext != ".jpg" && ext != ".jpeg")
                                throw new InvalidOperationException($"„{photo.FileName}\" не е позволен формат за снимка.");
                            saved.Add(await SaveFile(photo));
                        }
                        photoPaths = saved.Any() ? string.Join(";", saved) : null;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            if (!ModelState.IsValid)
            {
                model.TaskTitle = assignment.Task?.Title ?? "";
                model.SubmissionFormat = assignment.Task?.SubmissionFormat;
                return View(model);
            }

            int version = await _context.Submissions
                .CountAsync(s => s.AssignmentId == assignment.Id) + 1;

            var submission = new Submission
            {
                AssignmentId = assignment.Id,
                SubmittedText = model.SubmittedText,
                SubmittedLink = model.SubmittedLink,
                SubmittedFile = filePath,
                SubmittedPhotos = photoPaths,
                SubmittedNotes = model.SubmittedNotes,
                SubmitDate = DateTime.Now,
                Version = version,
                StatusSubmission = "Чака проверка"
            };

            assignment.Status = "Предадена за проверка";
            assignment.SubmitDate = DateTime.Now;
            if (assignment.Task != null)
                assignment.Task.Status = "Предадена за проверка";

            _context.Submissions.Add(submission);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Задачата е предадена. Ще получиш обратна връзка след проверка.";
            return RedirectToAction("Intern", "Dashboard");
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                throw new InvalidOperationException(
                    $"Файлът „{file.FileName}\" не е позволен тип. Разрешени: PDF, DOCX, PNG, JPG, ZIP.");

            if (file.Length > MaxFileSize)
                throw new InvalidOperationException($"Файлът „{file.FileName}\" е над 20 MB.");

            var uploadsDir = FilesController.UploadsRoot(_env);
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploadsDir, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }
    }
}