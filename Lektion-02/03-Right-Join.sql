/*
Lektion 2 – Uppgift 3: RIGHT JOIN
Skriv en RIGHT JOIN som identifierar alla servrar som för
närvarande saknar en tilldelad administratör (ManagedByUserId IS NULL).
*/

USE FailSafeIT_Local;
GO

SELECT u.Username, u.Email, s.ServerName
FROM dbo.Users AS u
RIGHT JOIN dbo.Servers AS s
    ON s.ManagedByUserId = u.Id
WHERE s.ManagedByUserId IS NULL
ORDER BY s.ServerName ASC;
        