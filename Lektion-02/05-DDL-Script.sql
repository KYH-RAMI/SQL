/*
Lektion 2 – Uppgift 5: FOREIGN KEY
Ändra relationen mellan Servers och Users
så att ON DELETE SET NULL används.
*/

USE FailSafeIT_Local;
GO

-- Ta bort den befintliga främmande nyckeln
ALTER TABLE dbo.Servers
DROP CONSTRAINT FK_Servers_Users;

-- Skapa främmande nyckeln igen med en ny borttagningsregel
ALTER TABLE dbo.Servers
ADD CONSTRAINT FK_Servers_Users
    FOREIGN KEY (ManagedByUserId)
    REFERENCES dbo.Users (Id)
    ON DELETE SET NULL;
