using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using InternProjects.Data;

namespace InternProjects.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FilesController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public static string UploadsRoot(IWebHostEnvironment env) =>
            Path.Combine(env.ContentRootPath, "uploads");

        [HttpGet]
        public async Task<IActionResult> Download(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return NotFound();

            var fileName = Path.GetFileName(name.Replace('\\', '/'));
            if (string.IsNullOrWhiteSpace(fileName)
                || fileName.Contains("..")
                || fileName.Contains('/')
                || fileName.Contains('\\'))
                return NotFound();

            var submission = await _context.Submissions
                .Include(s => s.Assignment)
                .FirstOrDefaultAsync(s =>
                    (s.SubmittedFile != null && s.SubmittedFile.Contains(fileName))
                    || (s.SubmittedPhotos != null && s.SubmittedPhotos.Contains(fileName)));

            if (submission == null) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var intern = await _context.Interns.FirstOrDefaultAsync(i => i.UserId == userId);

                if (intern == null || submission.Assignment?.InternId != intern.Id)
                    return Forbid();
            }

            var candidates = new List<string> { Path.Combine(UploadsRoot(_env), fileName) };
            if (!string.IsNullOrEmpty(_env.WebRootPath))
                candidates.Add(Path.Combine(_env.WebRootPath, "uploads", fileName));

            var fullPath = candidates.FirstOrDefault(System.IO.File.Exists);
            if (fullPath == null)
                return NotFound($"Файлът „{fileName}\" не е намерен на диска.");

            if (!new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var contentType))
                contentType = "application/octet-stream";

            return PhysicalFile(fullPath, contentType);
        }
    }
}
