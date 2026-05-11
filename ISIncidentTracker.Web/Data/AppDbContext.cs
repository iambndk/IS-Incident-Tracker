using Microsoft.EntityFrameworkCore;
using ISIncidentTracker.Web.Models;
using ISIncidentTracker.Web.Models.Enums;

namespace ISIncidentTracker.Web.Data;

/// <summary>
/// Контекст базы данных для системы учета инцидентов ИБ.
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
    /// Конфигурация моделей и отношений с использованием Fluent API.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Конфигурация Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // 2. Конфигурация User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(50).HasDefaultValue("Viewer");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasIndex(e => e.Username).IsUnique();
        });

        // 3. Конфигурация Incident (сложная часть: отношения)
        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Severity).HasDefaultValue(IncidentSeverity.Medium);
            entity.Property(e => e.Status).HasDefaultValue(IncidentStatus.New);
            entity.Property(e => e.Resolution).HasMaxLength(2000);

            // Индексы для ускорения поиска
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Severity);
            entity.HasIndex(e => e.ReportedDate);

            // 3.1 Отношение Incident → Category (N:1)
            // Один инцидент имеет одну категорию, одна категория может иметь много инцидентов
            entity.HasOne(e => e.Category)
                .WithMany(c => c!.Incidents)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict) // Нельзя удалить категорию, если есть инциденты
                .HasConstraintName("FK_Incidents_Categories");

            // 3.2 Отношение Incident → ReportedBy User (N:1)
            // Один инцидент создан одним пользователем, пользователь создал много инцидентов
            entity.HasOne(e => e.ReportedBy)
                .WithMany(u => u!.ReportedIncidents)
                .HasForeignKey(e => e.ReportedById)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Incidents_Users_ReportedBy");

            // 3.3 Отношение Incident → AssignedTo User (N:1)
            // Один инцидент может быть назначен одному пользователю (или никому)
            entity.HasOne(e => e.AssignedTo)
                .WithMany(u => u!.AssignedIncidents)
                .HasForeignKey(e => e.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull) // Если удалить пользователя, инцидент останется без ответственного
                .HasConstraintName("FK_Incidents_Users_AssignedTo");
        });

        // 4. Начальные данные (Seed Data)
        SeedData(modelBuilder);
    }

    /// <summary>
    /// Заполнение начальными данными при создании БД.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели.</param>
    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Категории инцидентов
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Утечка данных", Code = "DATA_LEAK", Description = "Несанкционированная передача конфиденциальной информации" },
            new Category { Id = 2, Name = "DDoS-атака", Code = "DDOS", Description = "Атака типа 'отказ в обслуживании'" },
            new Category { Id = 3, Name = "Вредоносное ПО", Code = "MALWARE", Description = "Обнаружение вирусов, троянов, шпионского ПО" },
            new Category { Id = 4, Name = "Несанкционированный доступ", Code = "UNAUTH_ACCESS", Description = "Попытка или факт несанкционированного входа" },
            new Category { Id = 5, Name = "Фишинг", Code = "PHISHING", Description = "Попытка получения учетных данных через социальную инженерию" }
        );

        // Тестовые пользователи
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
            }
        );
    }
}
