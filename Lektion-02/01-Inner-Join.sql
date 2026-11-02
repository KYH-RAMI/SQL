/* 
Lektion 2 – Uppgift 1: INNER JOIN
Hämta alla aktiva servrar tillsammans med
användarnamn och e-postadress för ansvarig administratör.
*/

USE FailSafeIT_Local;
GO

SELECT u.Username, u.Email, s.ServerName
FROM dbo.Users AS u
INNER JOIN dbo.Servers AS s
ON s.ManagedByUserId = u.Id
WHERE s.IsActive = 1;
