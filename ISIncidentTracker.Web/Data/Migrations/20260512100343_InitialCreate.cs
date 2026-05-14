using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ISIncidentTracker.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true, defaultValue: "Viewer"),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    ReportedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OccurredDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Severity = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 2),
                    Status = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReportedById = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignedToId = table.Column<int>(type: "INTEGER", nullable: true),
                    ResolvedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Resolution = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    AdditionalData = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incidents_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidents_Users_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Incidents_Users_ReportedById",
                        column: x => x.ReportedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncidentTags",
                columns: table => new
                {
                    IncidentId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentTags", x => new { x.IncidentId, x.TagId });
                    table.ForeignKey(
                        name: "FK_IncidentTags_Incidents",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncidentTags_Tags",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "PHISHING", "Фишинговые атаки и поддельные письма", "Фишинг" },
                    { 2, "DATA_LEAK", "Компрометация или утечка конфиденциальных данных", "Утечка данных" },
                    { 3, "DDOS", "Распределённая атака типа 'отказ в обслуживании'", "DDoS-атака" },
                    { 4, "MALWARE", "Вирусы, трояны, шпионское ПО, ботнеты", "Вредоносное ПО" },
                    { 5, "RANSOMWARE", "Шифровальщики и программы-вымогатели", "Ransomware" },
                    { 6, "UNAUTH_ACCESS", "Попытка или факт несанкционированного входа в систему", "Несанкционированный доступ" },
                    { 7, "DEVICE_LOSS", "Утеря или кража корпоративного устройства", "Потеря устройства" },
                    { 8, "POLICY_VIOLATION", "Нарушение внутренних правил ИБ", "Нарушение политик безопасности" },
                    { 9, "SQLI", "Атаки внедрением вредоносного SQL-кода", "SQL Injection" },
                    { 10, "XSS", "Межсайтовый скриптинг и инъекции кода", "XSS атака" },
                    { 11, "SOCIAL_ENG", "Манипулирование персоналом для получения доступа", "Социальная инженерия" },
                    { 12, "OTHER", "Инциденты, не подпадающие под другие категории", "Другое" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Требует немедленного внимания", "Срочно" },
                    { 2, "Затрагивает клиентские данные", "Клиент" },
                    { 3, "Внутренний инцидент", "Внутренний" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "LastLoginAt", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 12, 10, 3, 42, 697, DateTimeKind.Utc).AddTicks(4939), "admin@is-tracker.local", "Администратор Системы", true, null, "Admin", "admin" },
                    { 2, new DateTime(2026, 5, 12, 10, 3, 42, 697, DateTimeKind.Utc).AddTicks(4941), "analyst@is-tracker.local", "Аналитик ИБ", true, null, "Analyst", "analyst" },
                    { 3, new DateTime(2026, 5, 12, 10, 3, 42, 697, DateTimeKind.Utc).AddTicks(4942), "viewer@is-tracker.local", "Наблюдатель", true, null, "Viewer", "viewer" },
                    { 4, new DateTime(2026, 5, 12, 10, 3, 42, 697, DateTimeKind.Utc).AddTicks(4943), "soc@is-tracker.local", "Оператор SOC", true, null, "Analyst", "soc_operator" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Code",
                table: "Categories",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_AssignedToId",
                table: "Incidents",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_CategoryId",
                table: "Incidents",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_ReportedById",
                table: "Incidents",
                column: "ReportedById");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_ReportedDate",
                table: "Incidents",
                column: "ReportedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_Severity",
                table: "Incidents",
                column: "Severity");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_Status",
                table: "Incidents",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_Title",
                table: "Incidents",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentTags_IncidentId",
                table: "IncidentTags",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentTags_TagId",
                table: "IncidentTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncidentTags");

            migrationBuilder.DropTable(
                name: "Incidents");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
