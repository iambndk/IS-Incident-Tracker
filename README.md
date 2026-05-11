# IS Incident Tracker

**Веб-приложение для учета и мониторинга инцидентов информационной безопасности**

Курсовой проект по дисциплине «Кроссплатформенная среда исполнения программного обеспечения»

## 📋 Содержание

- [О проекте](#о-проекте)
- [Технологии](#технологии)
- [Архитектура](#архитектура)
- [Установка и запуск](#установка-и-запуск)
- [Docker](#docker)
- [Структура проекта](#структура-проекта)
- [Git Workflow](#git-workflow)
- [База данных](#база-данных)
- [Авторы](#авторы)

## 📖 О проекте

IS Incident Tracker — это веб-приложение для регистрации, отслеживания и управления инцидентами информационной безопасности в организации.

**Основные возможности:**
- ✅ Создание и редактирование инцидентов
- ✅ Классификация по категориям и уровню критичности
- ✅ Назначение ответственных исполнителей
- ✅ Отслеживание статуса обработки (New, Assigned, InProgress, Resolved, Closed)
- ✅ Просмотр статистики и аналитики
- ✅ Полная аудитория действий

## 🛠 Технологии

### Backend
- **.NET 8.0** — кроссплатформенная среда исполнения
- **ASP.NET Core Blazor Server** — веб-фреймворк
- **Entity Framework Core 8.0** — ORM (Code First подход)
- **SQLite** — база данных
- **FluentValidation** — валидация данных

### Architecture
- **Repository Pattern** — абстракция доступа к данным
- **Dependency Injection** — внедрение зависимостей через IServiceCollection
- **Fluent API** — конфигурация сущностей и отношений (1:N)

### DevOps
- **Docker** — контейнеризация (Multi-stage build)
- **Docker Compose** — оркестрация контейнеров
- **Git** — контроль версий (ветки: main, dev, feature/*)

## 🏗 Архитектура
┌─────────────────┐
│ Blazor UI │
│ (Components) │
└────────────────┘
│
┌────────▼────────┐
│ Services │
│ (Business Logic)│
└────────────────┘
│
┌────────▼────────┐
│ Repositories │
│ (Data Access) │
└────────┬────────┘
│
┌────────▼────────┐
│ EF Core + SQL │
│ Lite DB │
└─────────────────┘

### Диаграмма сущностей

Category (1) ──────< Incident (N) >────── User (1)
│ │ │
│ │ │
└────────────────────┴────────────────────┘

**Основные сущности:**
- **Incident** — инцидент ИБ (Id, Title, Description, Severity, Status, ReportedDate)
- **Category** — категория инцидента (Id, Name, Description, Code)
- **User** — пользователь системы (Id, Username, FullName, Email, Role)

**Отношения:**
- Incident → Category (N:1) — многие инциденты к одной категории
- Incident → User (ReportedBy) (N:1) — многие инциденты к одному автору
- Incident → User (AssignedTo) (N:1) — многие инциденты к одному ответственному

## 🚀 Установка и запуск

### Предварительные требования

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) или [VS Code](https://code.visualstudio.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (опционально)
- [Git](https://git-scm.com/)

### Локальный запуск

```powershell
# 1. Клонирование репозитория
git clone https://github.com/iambndk/IS-Incident-Tracker.git
cd IS-Incident-Tracker/IS-Incident-Tracker

# 2. Восстановление зависимостей
dotnet restore

# 3. Применение миграций БД
dotnet ef database update -p ISIncidentTracker.Web

# 4. Запуск приложения
dotnet run --project ISIncidentTracker.Web