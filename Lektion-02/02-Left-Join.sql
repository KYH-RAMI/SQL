/*
Lektion 2 – Uppgift 2: LEFT JOIN
Skriv en LEFT JOIN som listar samtliga användare i databasen,
oavsett om de hanterar en server eller inte. Sortera resultatet så att användare utan
servrar hamnar överst.
*/

USE FailSafeIT_Local;
GO

SELECT u.Username, u.Email, s.ServerName
FROM dbo.Users AS u
LEFT JOIN dbo.Servers AS s
    ON s.ManagedByUserId = u.Id

ORDER BY s.ManagedByUserId ASC,
         u.Username ASC,
         s.ServerName ASC;