# ============================================================
# ЭТАП 1: Build - компиляция проекта
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj файл и восстанавливаем зависимости
COPY ["ISIncidentTracker.Web/ISIncidentTracker.Web.csproj", "ISIncidentTracker.Web/"]
RUN dotnet restore "ISIncidentTracker.Web/ISIncidentTracker.Web.csproj"

# Копируем весь проект и публикуем
COPY . .
WORKDIR "/src/ISIncidentTracker.Web"
RUN dotnet publish "ISIncidentTracker.Web.csproj" -c Release -o /app/publish

# ============================================================
# ЭТАП 2: Runtime - запуск приложения
# ============================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Создаем пользователя для безопасности (не root)
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Копируем опубликованное приложение
COPY --from=build /app/publish .

# Создаем папку для БД SQLite
RUN mkdir -p /app/data && chown -R appuser:appuser /app/data

# Переключаемся на не-root пользователя
USER appuser

# Открываем порт
EXPOSE 8080
EXPOSE 8081

# Переменные окружения
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Запуск приложения
ENTRYPOINT ["dotnet", "ISIncidentTracker.Web.dll"]