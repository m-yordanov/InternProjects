using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternProjects.Data;
using InternProjects.Models.ViewModels;

namespace InternProjects.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        public async Task<IActionResult> Tasks()
        {
            var rows = await BuildTaskRows();
            return View(rows);
        }

        public async Task<IActionResult> Hours(DateTime? from, DateTime? to)
        {
            var rows = await BuildHoursRows(from, to);
            ViewBag.From = from;
            ViewBag.To = to;
            return View(rows);
        }

        public async Task<IActionResult> Summary()
        {
            var rows = await BuildSummaryRows();
            return View(rows);
        }

        public async Task<IActionResult> TasksCsv()
        {
            var rows = await BuildTaskRows();
            var sb = new StringBuilder();
            sb.AppendLine("Задача;Категория;Статус;Избрана от;Часове;Предадена;Приета");
            foreach (var r in rows)
            {
                sb.AppendLine(string.Join(";",
                    Csv(r.Title), Csv(r.Category), Csv(r.Status), Csv(r.ChosenBy),
                    r.AssignedHours,
                    r.SubmitDate?.ToString("dd.MM.yyyy") ?? "",
                    r.AcceptDate?.ToString("dd.MM.yyyy") ?? ""));
            }
            return CsvFile(sb, "spravka-zadachi.csv");
        }

        public async Task<IActionResult> HoursCsv(DateTime? from, DateTime? to)
        {
            var rows = await BuildHoursRows(from, to);
            var sb = new StringBuilder();
            sb.AppendLine("Стажант;Дата;Източник;Описание;Часове;Добавено от");
            foreach (var r in rows)
            {
                sb.AppendLine(string.Join(";",
                    Csv(r.InternName), r.Date.ToString("dd.MM.yyyy"), Csv(r.Source),
                    Csv(r.Description), r.Hours, Csv(r.AddedBy)));
            }
            return CsvFile(sb, "spravka-chasove.csv");
        }

        public async Task<IActionResult> SummaryCsv()
        {
            var rows = await BuildSummaryRows();
            var sb = new StringBuilder();
            sb.AppendLine("Стажант;Изискуеми часове;Отчетени;Оставащи;Процент");
            foreach (var r in rows)
            {
                sb.AppendLine(string.Join(";",
                    Csv(r.InternName), r.TotalHours, r.ReportedHours, r.RemainingHours, r.Percent + "%"));
            }
            return CsvFile(sb, "obsht-otchet.csv");
        }

        private async Task<List<TaskReportRow>> BuildTaskRows()
        {
            var assignments = await _context.TaskAssignments
                .Include(a => a.Task).ThenInclude(t => t.Category)
                .Include(a => a.Intern).ThenInclude(i => i.User)
                .Select(a => new TaskReportRow
                {
                    Title = a.Task!.Title,
                    Category = a.Task.Category!.Name,
                    Status = a.Status,
                    ChosenBy = a.Intern!.User!.FirstName + " " + a.Intern.User.LastName,
                    AssignedHours = a.Task.AssignedHours,
                    SubmitDate = a.SubmitDate,
                    AcceptDate = a.Status == "Приета" ? a.SubmitDate : null
                })
                .ToListAsync();

            var unchosen = await _context.TaskItems
                .Include(t => t.Category)
                .Where(t => !_context.TaskAssignments.Any(a => a.TaskId == t.Id))
                .Select(t => new TaskReportRow
                {
                    Title = t.Title,
                    Category = t.Category!.Name,
                    Status = t.Status,
                    ChosenBy = "-",
                    AssignedHours = t.AssignedHours
                })
                .ToListAsync();

            return assignments.Concat(unchosen)
                .OrderBy(r => r.Status).ThenBy(r => r.Title)
                .ToList();
        }

        private async Task<List<HoursReportRow>> BuildHoursRows(DateTime? from, DateTime? to)
        {
            var query = _context.TimeLogs
                .Include(t => t.Intern).ThenInclude(i => i.User)
                .Include(t => t.CreatedBy)
                .AsQueryable();

            if (from != null) query = query.Where(t => t.Date >= from.Value);
            if (to != null) query = query.Where(t => t.Date <= to.Value);

            return await query
                .OrderByDescending(t => t.Date)
                .Select(t => new HoursReportRow
                {
                    InternName = t.Intern!.User!.FirstName + " " + t.Intern.User.LastName,
                    Date = t.Date,
                    Source = t.SourceType == "task" ? "Задача" : "Ръчно",
                    Description = t.Description ?? "",
                    Hours = t.Hours,
                    AddedBy = t.CreatedBy != null
                        ? t.CreatedBy.FirstName + " " + t.CreatedBy.LastName : "-"
                })
                .ToListAsync();
        }

        private async Task<List<SummaryReportRow>> BuildSummaryRows()
        {
            return await _context.Interns
                .Include(i => i.User)
                .OrderBy(i => i.User!.FirstName)
                .Select(i => new SummaryReportRow
                {
                    InternId = i.Id,
                    InternName = i.User!.FirstName + " " + i.User.LastName,
                    TotalHours = i.TotalHours,
                    ReportedHours = i.ReportedHours,
                    RemainingHours = i.RemainingHours
                })
                .ToListAsync();
        }

        private static string Csv(string? value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return value.Contains(';') || value.Contains('"') || value.Contains('\n')
                ? "\"" + value.Replace("\"", "\"\"") + "\""
                : value;
        }

        private FileContentResult CsvFile(StringBuilder sb, string fileName)
        {
            var bytes = Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(sb.ToString()))
                .ToArray();
            return File(bytes, "text/csv", fileName);
        }
    }
}