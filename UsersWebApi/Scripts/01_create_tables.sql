-- Создание таблицы справочника статусов
CREATE TABLE IF NOT EXISTS "UserStatuses"
(
    "Id"   INT PRIMARY KEY,
    "Name" VARCHAR(50) NOT NULL
);

-- Наполнение справочника статусов
INSERT INTO "UserStatuses" ("Id", "Name")
VALUES
    (1, 'Active'),
    (2, 'Inactive'),
    (3, 'Blocked')
ON CONFLICT ("Id") DO NOTHING;


-- Создание таблицы пользователей
CREATE TABLE IF NOT EXISTS "Users"
(
    "Id"          UUID PRIMARY KEY,
    "FirstName"   VARCHAR(100) NOT NULL,
    "LastName"    VARCHAR(100) NOT NULL,
    "Email"       VARCHAR(255) NOT NULL,
    "BirthDate"   DATE NOT NULL,
    "CreatedAt"   TIMESTAMPTZ NOT NULL,
    "UpdatedAt"   TIMESTAMPTZ NOT NULL,
    "AstraSource" VARCHAR(100) NOT NULL,
    "StatusId"    INT NOT NULL,

    FOREIGN KEY ("StatusId")
        REFERENCES "UserStatuses" ("Id")
);

-- Email должен быть уникальным без учета регистра.
CREATE UNIQUE INDEX IF NOT EXISTS "UX_Users_Email_Lower"
    ON "Users" (LOWER("Email"));
