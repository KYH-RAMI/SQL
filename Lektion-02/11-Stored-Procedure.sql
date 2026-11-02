/*
Lektion 2 – Uppgift 11: Stored Procedure
Skapa en Stored Procedure i SQL Server som heter
sp_RegisterServer. Den ska ta emot parametrar för servernamn, IP-adress, miljö och
hanterande användar-ID, och utföra en INSERT INTO.
Registrerar en ny server med hjälp av parametrar.
*/

USE FailSafeIT_Local;
GO

CREATE OR ALTER PROCEDURE dbo.usp_RegisterServer 
@ServerName NVARCHAR(100), @IpAddress VARCHAR(15),
@Environment NVARCHAR(50), @ManagedByUserId INT

AS

BEGIN

    SET NOCOUNT ON;

    INSERT INTO dbo.Servers
    (ServerName, IpAddress, Environment, ManagedByUserId)
    VALUES
    (@ServerName, @IpAddress, @Environment, @ManagedByUserId);
    
END;
GO
EXEC dbo.usp_RegisterServer
    @ServerName = N'BackupServer',
    @IpAddress = '10.0.0.50',
    @Environment = N'Production',
    @ManagedByUserId = 2;
    
    SELECT
    s.ServerName,
    s.Environment,
    s.IsActive,
    u.Username
    
    FROM dbo.Servers AS s
    INNER JOIN dbo.Users AS u
    ON s.ManagedByUserId = u.Id
    WHERE s.ServerName = N'BackupServer';