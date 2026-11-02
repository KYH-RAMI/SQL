/*
Lektion 2 – Uppgift 7: NONCLUSTERED INDEX
Skapa ett NONCLUSTERED INDEX på tabellen Servers för
kolumnen ManagedByUserId. Skriv en kort kommentar om varför detta är viktigt vid
Foreign Key-valideringar.
*/

USE FailSafeIT_Local;
GO

CREATE NONCLUSTERED INDEX IX_Servers_ManagedByUserId
ON dbo.Servers (ManagedByUserId);

/*
En FOREIGN KEY skapar inte automatiskt ett index.

Indexet gör det lättare att hitta servrar som tillhör en viss
användare, till exempel vid JOIN eller när en användare tas bort.

Med få servrar märks knappt någon skillnad, men med många
servrar kan indexet göra sökningarna snabbare.
*/