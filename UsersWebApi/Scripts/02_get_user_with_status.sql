-- SQL-запрос с JOIN для получения пользователя с его статусом
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
INNER JOIN "UserStatuses" AS s ON u."StatusId" = s."Id";
