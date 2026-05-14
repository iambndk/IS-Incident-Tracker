using Microsoft.EntityFrameworkCore;
using ISIncidentTracker.Web.Models;
using ISIncidentTracker.Web.Models.Enums;

namespace ISIncidentTracker.Web.Data;

/// <summary>
/// Контекст базы данных для системы учета инцидентов информационной безопасности.
/// Использует SQLite и подход Code First.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AppDbContext"/>.
    /// </summary>
    /// <param name="options">Опции конфигурации контекста.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Набор сущностей инцидентов.
    /// </summary>
    public DbSet<Incident> Incidents { get; set; } = null!;

    /// <summary>
    /// Набор сущностей категорий.
    /// </summary>
    public DbSet<Category> Categories { get; set; } = null!;

    /// <summary>
    /// Набор сущностей пользователей.
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Набор сущностей тегов.
    /// </summary>
    public DbSet<Tag> Tags { get; set; } = null!;

    /// <summary>
    /// Набор сущностей связи инцидентов и тегов (Many-to-Many).
    /// </summary>
    public DbSet<IncidentTag> IncidentTags { get; set; } = null!;

    /// <summary>
    /// Конфигурация моделей, отношений и начальных данных (Seed Data) с использованием Fluent API.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ==========================================
        // 1. Конфигурация Category
        // ==========================================
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => e.Code).IsUnique();
        });

        // ==========================================
        // 2. Конфигурация User
        // ==========================================
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(50).HasDefaultValue("Viewer");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email);
        });

        // ==========================================
        // 3. Конфигурация Tag
        // ==========================================
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // ==========================================
        // 4. Конфигурация Incident (Основная сущность)
        // ==========================================
        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Severity).HasDefaultValue(IncidentSeverity.Medium);
            entity.Property(e => e.Status).HasDefaultValue(IncidentStatus.New);
            entity.Property(e => e.Resolution).HasMaxLength(2000);
            entity.Property(e => e.AdditionalData).HasMaxLength(4000);

            // Индексы для оптимизации поиска
            entity.HasIndex(e => e.Title);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Severity);
            entity.HasIndex(e => e.ReportedDate);
            entity.HasIndex(e => e.CategoryId);
            entity.HasIndex(e => e.AssignedToId);

            // 4.1. Отношение Incident -> Category (N:1)
            entity.HasOne(e => e.Category)
                .WithMany(c => c!.Incidents)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Incidents_Categories_CategoryId");

            // 4.2. Отношение Incident -> ReportedBy User (N:1)
            entity.HasOne(e => e.ReportedBy)
                .WithMany(u => u!.ReportedIncidents)
                .HasForeignKey(e => e.ReportedById)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Incidents_Users_ReportedById");

            // 4.3. Отношение Incident -> AssignedTo User (N:1)
            entity.HasOne(e => e.AssignedTo)
                .WithMany(u => u!.AssignedIncidents)
                .HasForeignKey(e => e.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Incidents_Users_AssignedToId");
        });

        // ==========================================
        // 5. Конфигурация IncidentTag (Many-to-Many)
        // ==========================================
        modelBuilder.Entity<IncidentTag>(entity =>
        {
            // Составной первичный ключ (рекомендуемый подход для Many-to-Many)
            entity.HasKey(e => new { e.IncidentId, e.TagId });

            entity.HasOne(e => e.Incident)
                .WithMany(i => i!.IncidentTags)
                .HasForeignKey(e => e.IncidentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_IncidentTags_Incidents");

            entity.HasOne(e => e.Tag)
                .WithMany(t => t!.IncidentTags)
                .HasForeignKey(e => e.TagId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_IncidentTags_Tags");

            // Индексы для производительности
            entity.HasIndex(e => e.IncidentId);
            entity.HasIndex(e => e.TagId);
        });

        // ==========================================
        // 6. Начальные данные (Seed Data)
        // ==========================================
        SeedData(modelBuilder);
    }

    /// <summary>
    /// Заполнение БД начальными данными при создании.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели.</param>
    private static void SeedData(ModelBuilder modelBuilder)
    {
        // === Категории инцидентов (12 обязательных типов) ===
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Фишинг", Code = "PHISHING", Description = "Фишинговые атаки и поддельные письма" },
            new Category { Id = 2, Name = "Утечка данных", Code = "DATA_LEAK", Description = "Компрометация или утечка конфиденциальных данных" },
            new Category { Id = 3, Name = "DDoS-атака", Code = "DDOS", Description = "Распределённая атака типа 'отказ в обслуживании'" },
            new Category { Id = 4, Name = "Вредоносное ПО", Code = "MALWARE", Description = "Вирусы, трояны, шпионское ПО, ботнеты" },
            new Category { Id = 5, Name = "Ransomware", Code = "RANSOMWARE", Description = "Шифровальщики и программы-вымогатели" },
            new Category { Id = 6, Name = "Несанкционированный доступ", Code = "UNAUTH_ACCESS", Description = "Попытка или факт несанкционированного входа в систему" },
            new Category { Id = 7, Name = "Потеря устройства", Code = "DEVICE_LOSS", Description = "Утеря или кража корпоративного устройства" },
            new Category { Id = 8, Name = "Нарушение политик безопасности", Code = "POLICY_VIOLATION", Description = "Нарушение внутренних правил ИБ" },
            new Category { Id = 9, Name = "SQL Injection", Code = "SQLI", Description = "Атаки внедрением вредоносного SQL-кода" },
            new Category { Id = 10, Name = "XSS атака", Code = "XSS", Description = "Межсайтовый скриптинг и инъекции кода" },
            new Category { Id = 11, Name = "Социальная инженерия", Code = "SOCIAL_ENG", Description = "Манипулирование персоналом для получения доступа" },
            new Category { Id = 12, Name = "Другое", Code = "OTHER", Description = "Инциденты, не подпадающие под другие категории" }
        );

        // === Пользователи по умолчанию ===
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                FullName = "Администратор Системы",
                Email = "admin@is-tracker.local",
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Id = 2,
                Username = "analyst",
                FullName = "Аналитик ИБ",
                Email = "analyst@is-tracker.local",
                Role = "Analyst",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Id = 3,
                Username = "viewer",
                FullName = "Наблюдатель",
                Email = "viewer@is-tracker.local",
                Role = "Viewer",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Id = 4,
                Username = "soc_operator",
                FullName = "Оператор SOC",
                Email = "soc@is-tracker.local",
                Role = "Analyst",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        );

        // === Теги по умолчанию ===
        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = "Срочно", Description = "Требует немедленного внимания" },
            new Tag { Id = 2, Name = "Клиент", Description = "Затрагивает клиентские данные" },
            new Tag { Id = 3, Name = "Внутренний", Description = "Внутренний инцидент" }
        );
    }
}
