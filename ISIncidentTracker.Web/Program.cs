using FluentValidation;
using ISIncidentTracker.Web.Components;
using ISIncidentTracker.Web.Data;
using ISIncidentTracker.Web.Data.Repositories;
using ISIncidentTracker.Web.Data.Repositories.Interfaces;
using ISIncidentTracker.Web.Models;
using ISIncidentTracker.Web.Services;
using ISIncidentTracker.Web.Services.Interfaces;
using ISIncidentTracker.Web.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Регистрация DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Регистрация репозиториев
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();

// 3. Регистрация сервисов
builder.Services.AddScoped<IIncidentService, IncidentService>();

// 4. Регистрация FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<IncidentValidator>();

// 5. Добавление поддержки Razor компонентов
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// 6. Настройка конвейера HTTP-запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 7. Автоматическое применение миграций при запуске (для удобства в курсовом проекте)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
            Console.WriteLine("✅ Миграции применены автоматически.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Ошибка при применении миграций: {ex.Message}");
    }
}

app.Run();
