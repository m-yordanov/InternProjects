using InternProjects.Data;
using InternProjects.Models;


namespace InternProjects.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Users.Any()) return;

            var admin = new User
            {
                FirstName = "Мартин",
                LastName = "Администраторов",
                Email = "admin@aksbg.com",
                PhoneNumber = "123456789",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin",
                Status = "Активен",
                CreationDate = DateTime.Now
            };

            var mentor = new User
            {
                FirstName = "Велислав",
                LastName = "Менторов",
                Email = "mentor@aksbg.com",
                PhoneNumber = "123456789",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("mentor123"),
                Role = "Admin",
                Status = "Активен",
                CreationDate = DateTime.Now
            };

            var internUser = new User
            {
                FirstName = "Иван",
                LastName = "Иванов",
                Email = "ivan@aksbg.com",
                PhoneNumber = "123456789",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("intern123"),
                Role = "Intern",
                Status = "Активен",
                CreationDate = DateTime.Now
            };

            context.Users.AddRange(admin, mentor, internUser);
            context.SaveChanges();

            var intern = new Intern
            {
                UserId = internUser.Id,
                University = "ТУ Варна",
                Specialty = "Софтуерни и интернет технологии",
                StartDate = new DateTime(2026, 7, 1),
                EndDate = new DateTime(2026, 9, 30),
                TotalHours = 240,
                TaskHours = 0,
                AddedHours = 3,
                ReportedHours = 3,
                RemainingHours = 237,
                MentorId = mentor.Id,
                Notes = "Тестов стажант"
            };
            context.Interns.Add(intern);
            context.SaveChanges();

            // ===== Категории (т. 11 от документа — седемте основни) =====
            var categories = new List<Category>
            {
                new() { Name = "Маркетинг", Description = "Дигитален маркетинг, социални мрежи, SEO, съдържание" },
                new() { Name = "Уеб разработка", Description = "HTML/CSS, JavaScript, frontend, backend, бази данни" },
                new() { Name = "Хардуер", Description = "Компоненти, диагностика, ремонт, профилактика" },
                new() { Name = "Софтуер", Description = "Инсталация, ОС, драйвери, софтуерни проблеми" },
                new() { Name = "Иновации", Description = "Нови технологии, автоматизация, AI инструменти" },
                new() { Name = "Мениджмънт", Description = "Организация, планиране, координация, документация"},
                new() { Name = "Киберсигурност", Description = "Информационна сигурност, защита на данни, уязвимости"}
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            var cyber = categories.First(c => c.Name == "Киберсигурност");
            var hardware = categories.First(c => c.Name == "Хардуер");
            var webdev = categories.First(c => c.Name == "Уеб разработка");
            var marketing = categories.First(c => c.Name == "Маркетинг");

            var tasks = new List<TaskItem>
            {
                new()
                {
                    Title = "Как да разпознаем фишинг имейл",
                    CategoryId = cyber.Id,
                    ShortDescription = "Статия за разпознаване на фишинг имейли",
                    LongDescription = "Минимум 600 думи; поне 5 признака за фишинг; практически съвети; реални примери; ясен и разбираем език.",
                    SubmissionFormat = "Текст или Google Docs линк",
                    AssignedHours = 2,
                    Difficulty = "Средна",
                    Priority = "Нормален",
                    Status = "Свободна",
                    SuitableFor = "Киберсигурност, Маркетинг",
                    IsTeamTask = false,
                    MaxInterns = 1,
                    CreatorId = admin.Id,
                    Deadline = DateTime.Now.AddDays(14),
                    CreationDate = DateTime.Now
                },
                new()
                {
                    Title = "Почистване на лаптоп",
                    CategoryId = hardware.Id,
                    ShortDescription = "Профилактика и почистване на лаптоп под наблюдение",
                    LongDescription = "Безопасно разглобяване; почистване; тест след сглобяване; отбелязване на проблеми. Работа под наблюдение.",
                    SubmissionFormat = "Снимка преди; снимка след; кратко описание; чеклист",
                    AssignedHours = 3,
                    Difficulty = "Лесна",
                    Priority = "Нормален",
                    Status = "Свободна",
                    SuitableFor = "Хардуер",
                    IsTeamTask = false,
                    MaxInterns = 1,
                    CreatorId = admin.Id,
                    Deadline = DateTime.Now.AddDays(7),
                    CreationDate = DateTime.Now
                },
                new()
                {
                    Title = "Уеб страница FAQ",
                    CategoryId = webdev.Id,
                    ShortDescription = "FAQ страница с HTML и CSS",
                    LongDescription = "HTML; CSS; responsive дизайн; минимум 10 въпроса и отговора; чист код.",
                    SubmissionFormat = "GitHub линк; screenshot; кратко описание",
                    AssignedHours = 8,
                    Difficulty = "Средна",
                    Priority = "Нормален",
                    Status = "Свободна",
                    SuitableFor = "Уеб разработка",
                    IsTeamTask = false,
                    MaxInterns = 1,
                    CreatorId = admin.Id,
                    Deadline = DateTime.Now.AddDays(21),
                    CreationDate = DateTime.Now
                },
                new()
                {
                    Title = "AI агент за социални мрежи",
                    CategoryId = marketing.Id,
                    ShortDescription = "AI агент за генериране на съдържание",
                    LongDescription = "Ясно описание на логиката; поне 20 примерни идеи; категории съдържание; инструкция за използване.",
                    SubmissionFormat = "Документация; prompt-и; примерни резултати; линк към проекта",
                    AssignedHours = 16,
                    Difficulty = "Трудна",
                    Priority = "Висок",
                    Status = "Свободна",
                    SuitableFor = "Маркетинг, Иновации",
                    IsTeamTask = false,
                    MaxInterns = 1,
                    CreatorId = admin.Id,
                    Deadline = DateTime.Now.AddDays(30),
                    CreationDate = DateTime.Now
                },
                new()
                {
                    Title = "Анализ на конкуренти",
                    CategoryId = marketing.Id,
                    ShortDescription = "Сравнителен анализ на 5 конкурента",
                    LongDescription = "Минимум 5 конкурента; услуги; силни страни; слаби страни; предложения за АКС.",
                    SubmissionFormat = "Таблица; кратък анализ; заключение",
                    AssignedHours = 6,
                    Difficulty = "Средна",
                    Priority = "Нормален",
                    Status = "Свободна",
                    SuitableFor = "Маркетинг",
                    IsTeamTask = false,
                    MaxInterns = 1,
                    CreatorId = admin.Id,
                    Deadline = DateTime.Now.AddDays(14),
                    CreationDate = DateTime.Now
                },
                new()
                {
                    Title = "Вътрешна документация на сервизния процес",
                    CategoryId = categories.First(c => c.Name == "Мениджмънт").Id,
                    ShortDescription = "Документиране на процеса от приемане до връщане на устройство",
                    SubmissionFormat = "DOCX",
                    AssignedHours = 4,
                    Difficulty = "Средна",
                    Priority = "Нисък",
                    Status = "Чернова",
                    IsTeamTask = false,
                    MaxInterns = 1,
                    CreatorId = admin.Id,
                    CreationDate = DateTime.Now
                }
            };
            context.TaskItems.AddRange(tasks);
            context.SaveChanges();

            var manualLog = new TimeLog
            {
                SourceType = "manual",
                InternId = intern.Id,
                AssignmentId = null,
                Hours = 3,
                Date = DateTime.Now.AddDays(-2),
                Description = "Практическо участие при диагностика и профилактика на лаптопи.",
                StatusApproval = "Одобрен",
                CreatedById = mentor.Id,
                ApprovedById = mentor.Id,
                CreationDate = DateTime.Now
            };
            context.TimeLogs.Add(manualLog);
            context.SaveChanges();
        }
    }
}