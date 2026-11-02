/*
Lektion 2 – Uppgift 4: FULL OUTER JOIN
Skriv en FULL OUTER JOIN som kombinerar Users och
Servers. Visa Username och ServerName för att kartlägga alla befintliga och saknade
relationer i infrastrukturen.
*/

USE FailSafeIT_Local;
GO

SELECT u.Username, s.ServerName
FROM dbo.Users AS u
FULL OUTER JOIN dbo.Servers AS s
    ON s.ManagedByUserId = u.Id;
        