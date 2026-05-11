CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "Categories" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Categories" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Description" TEXT NULL,
    "Code" TEXT NULL
);

CREATE TABLE "Users" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY AUTOINCREMENT,
    "Username" TEXT NOT NULL,
    "FullName" TEXT NULL,
    "Email" TEXT NULL,
    "Role" TEXT NULL DEFAULT 'Viewer',
    "CreatedAt" TEXT NOT NULL,
    "LastLoginAt" TEXT NULL,
    "IsActive" INTEGER NOT NULL DEFAULT 1
);

CREATE TABLE "Incidents" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Incidents" PRIMARY KEY AUTOINCREMENT,
    "Title" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "ReportedDate" TEXT NOT NULL,
    "OccurredDate" TEXT NULL,
    "Severity" INTEGER NOT NULL DEFAULT 2,
    "Status" INTEGER NOT NULL DEFAULT 1,
    "CategoryId" INTEGER NOT NULL,
    "ReportedById" INTEGER NOT NULL,
    "AssignedToId" INTEGER NULL,
    "ResolvedDate" TEXT NULL,
    "Resolution" TEXT NULL,
    "AdditionalData" TEXT NULL,
    "UpdatedAt" TEXT NULL,
    CONSTRAINT "FK_Incidents_Categories" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Incidents_Users_AssignedTo" FOREIGN KEY ("AssignedToId") REFERENCES "Users" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_Incidents_Users_ReportedBy" FOREIGN KEY ("ReportedById") REFERENCES "Users" ("Id") ON DELETE RESTRICT
);

INSERT INTO "Categories" ("Id", "Code", "Description", "Name")
VALUES (1, 'DATA_LEAK', 'Несанкционированная передача конфиденциальной информации', 'Утечка данных');
SELECT changes();

INSERT INTO "Categories" ("Id", "Code", "Description", "Name")
VALUES (2, 'DDOS', 'Атака типа ''отказ в обслуживании''', 'DDoS-атака');
SELECT changes();

INSERT INTO "Categories" ("Id", "Code", "Description", "Name")
VALUES (3, 'MALWARE', 'Обнаружение вирусов, троянов, шпионского ПО', 'Вредоносное ПО');
SELECT changes();

INSERT INTO "Categories" ("Id", "Code", "Description", "Name")
VALUES (4, 'UNAUTH_ACCESS', 'Попытка или факт несанкционированного входа', 'Несанкционированный доступ');
SELECT changes();

INSERT INTO "Categories" ("Id", "Code", "Description", "Name")
VALUES (5, 'PHISHING', 'Попытка получения учетных данных через социальную инженерию', 'Фишинг');
SELECT changes();


INSERT INTO "Users" ("Id", "CreatedAt", "Email", "FullName", "IsActive", "LastLoginAt", "Role", "Username")
VALUES (1, '2026-05-11 21:53:43.7959544', 'admin@is-tracker.local', 'Администратор Системы', 1, NULL, 'Admin', 'admin');
SELECT changes();

INSERT INTO "Users" ("Id", "CreatedAt", "Email", "FullName", "IsActive", "LastLoginAt", "Role", "Username")
VALUES (2, '2026-05-11 21:53:43.7959546', 'analyst@is-tracker.local', 'Аналитик ИБ', 1, NULL, 'Analyst', 'analyst');
SELECT changes();

INSERT INTO "Users" ("Id", "CreatedAt", "Email", "FullName", "IsActive", "LastLoginAt", "Role", "Username")
VALUES (3, '2026-05-11 21:53:43.7959548', 'viewer@is-tracker.local', 'Наблюдатель', 1, NULL, 'Viewer', 'viewer');
SELECT changes();


CREATE UNIQUE INDEX "IX_Categories_Name" ON "Categories" ("Name");

CREATE INDEX "IX_Incidents_AssignedToId" ON "Incidents" ("AssignedToId");

CREATE INDEX "IX_Incidents_CategoryId" ON "Incidents" ("CategoryId");

CREATE INDEX "IX_Incidents_ReportedById" ON "Incidents" ("ReportedById");

CREATE INDEX "IX_Incidents_ReportedDate" ON "Incidents" ("ReportedDate");

CREATE INDEX "IX_Incidents_Severity" ON "Incidents" ("Severity");

CREATE INDEX "IX_Incidents_Status" ON "Incidents" ("Status");

CREATE UNIQUE INDEX "IX_Users_Username" ON "Users" ("Username");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260511215344_InitialCreate', '8.0.0');

COMMIT;

