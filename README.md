# User Web API

REST API для управления справочником пользователей на базе ASP.NET Core 8, Dapper и PostgreSQL.

## Зависимости
- .NET 8.0 SDK (или новее)
- PostgreSQL

## Структура проекта
- `UserWebApi/` — основной проект ASP.NET Core Web API
- `UsersWebApi.Tests/` — проект с xUnit тестами сервисного и контроллерного слоёв
- `UserWebApi/Scripts/` — SQL-скрипты для работы с БД

## Развёртывание базы данных
Проект не выполняет миграции автоматически (используется Dapper). Для инициализации БД выполните скрипт из папки `Scripts`:
1. Убедитесь, что у вас есть база данных PostgreSQL. Задайте строку подключения через User Secrets или переменную окружения.
2. Выполните скрипт `UserWebApi/Scripts/01_create_tables.sql` на вашей БД.

Скрипт можно повторно выполнять: он не дублирует статусы и уникальный индекс email.

В папке `Scripts` также находятся примеры SQL-запросов:
- `02_get_user_with_status.sql` — получение пользователя с джойном справочника.
- `03_get_users_xml.sql` — примеры выборки в XML формате (для MSSQL и PostgreSQL).

## Запуск приложения
```bash
dotnet restore UsersWebApi.sln
dotnet build UsersWebApi.sln
dotnet run --project UsersWebApi/UserWebApi.csproj
```
После запуска перейдите на Swagger UI:

- HTTPS: `https://localhost:7274/swagger`
- HTTP: `http://localhost:5209/swagger`

При запуске через профиль IIS Express используются порты `http://localhost:4670` и `https://localhost:44357`.

## Запуск тестов
```bash
dotnet test UsersWebApi.sln
```
