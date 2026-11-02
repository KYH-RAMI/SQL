/*
Lektion 2 – Uppgift 6: INNER JOIN FILTER
Skriv en query som via en INNER JOIN hämtar användarnamn och
servernamn, men filtrera resultatet så att du endast visar servrar som tillhör miljöerna
'Production' eller 'Staging'.
*/

USE FailSafeIT_Local;
GO

SELECT u.Username, s.ServerName
FROM Users AS u
INNER JOIN Servers AS s
    ON s.ManagedByUserId = u.Id
WHERE s.Environment IN ('Production', 'Staging');