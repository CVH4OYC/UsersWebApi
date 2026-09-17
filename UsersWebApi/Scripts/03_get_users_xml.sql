-- SQL-запрос, который возвращает список пользователей с их статусами в формате XML

-- Вариант 1: Для Microsoft SQL Server (как требуется в ТЗ: "используйте FOR XML")
SELECT 
    u."Id", 
    u."FirstName", 
    u."LastName", 
    u."Email", 
    u."BirthDate", 
    u."CreatedAt", 
    u."UpdatedAt", 
    u."AstraSource", 
    u."StatusId",
    s."Name" AS "StatusName"
FROM "Users" AS u
INNER JOIN "UserStatuses" AS s ON u."StatusId" = s."Id"
FOR XML PATH('User'), ROOT('Users');


-- Вариант 2: Для PostgreSQL (так как основная БД в ТЗ PostgreSQL, FOR XML в ней нет)

SELECT query_to_xml('
    SELECT 
        u."Id", 
        u."FirstName", 
        u."LastName", 
        u."Email", 
        u."BirthDate", 
        u."CreatedAt", 
        u."UpdatedAt", 
        u."AstraSource", 
        u."StatusId",
        s."Name" AS "StatusName"
    FROM "Users" AS u
    INNER JOIN "UserStatuses" AS s ON u."StatusId" = s."Id"', 
true, true, '');

