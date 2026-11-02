-- Uppg. 3, Skapa databasen
CREATE DATABASE FailSafeIT_Local
GO
-- Uppg. 4, Använd databasen FailSafeIT_Local
USE FailSafeIT_Local;
GO

-- Uppg. 5, Återanvänd FailSafeIT_Local databas 
USE FailSafeIT_Local;
GO
-- Skapa tabellen Users
CREATE TABLE dbo.Users (
    Id       INT           IDENTITY(1,1) NOT NULL,
    Username NVARCHAR(50)  NOT NULL,
    Role     NVARCHAR(20)  NULL,
    CONSTRAINT PK_Users PRIMARY KEY (Id)
);
-- Lägg till i efterhand 
ALTER TABLE dbo.Users
ADD CONSTRAINT CK_Users_UsernameTrimmed
CHECK (LEN(TRIM(Username)) > 0);

-- Uppg. 6, Använd FailSafeIT databasen
USE FailSafeIT_Local;
GO
-- Skapa Servers tabellen
CREATE TABLE dbo.Servers (
    Id          INT           IDENTITY(1,1) NOT NULL,
    ServerName  NVARCHAR(100) NOT NULL,
    IpAddress   VARCHAR(15)   NULL,
    Environment NVARCHAR(20)  NULL,
    CONSTRAINT PK_Servers PRIMARY KEY (Id)
);

-- Uppg. 7, Mata in två tekniker i Users
INSERT INTO dbo.Users (Username, Role)
VALUES
    ('RoRoDevOps', 'DevOps'),
    ('RoRoAdmin',    'Admin');

-- Uppg. 8, Mata in tre servrar i Servers
INSERT INTO dbo.Servers (ServerName, IpAddress, Environment)
VALUES
    ('srv-prod-01', '10.0.1.10', 'Prod'),
    ('dev-box-01',  '192.168.1.21', 'Dev'),
    ('dev-box-02',  '192.168.1.22', 'Dev');

-- Uppg. 9, Hämta alla rader och kolumner
SELECT *
FROM dbo.Servers;
SELECT *
FROM dbo.Users;

-- Uppg. 10, Begränsa kolumner och filtrera på Prod
SELECT ServerName, IpAddress
FROM dbo.Servers
WHERE Environment = 'Prod';

-- Uppg. 11, Uppdatera IP-adress för en specifik server
UPDATE dbo.Servers
SET    IpAddress = '10.0.1.99'
WHERE  Id = 1;

-- Uppg. 12, Radera en utvecklingsserver via dess Id
SELECT * FROM Servers 

DELETE FROM dbo.Servers
WHERE  Id = 3;

INSERT INTO dbo.Servers (ServerName, IpAddress, Environment)
VALUES
    ('dev-box-02',  '192.168.1.22', 'Dev');

-- Uppg. 13, Lägg till UNIQUE-constraint på Username && Lägg till Email
ALTER TABLE dbo.Users
    ADD CONSTRAINT UQ_Users_Username UNIQUE (Username);

ALTER TABLE dbo.Users
    ADD Email VARCHAR(255) NULL;

-- Uppg. 14, Lägg till DateCreated med standardvärde
ALTER TABLE dbo.Servers
    ADD DateCreated DATETIME2 NOT NULL
        CONSTRAINT DF_Servers_DateCreated DEFAULT SYSUTCDATETIME();

-- Uppg. 15, Radera tabellen och databasen
-- Växla till master först; man kan inte radera den databas man befinner sig i


USE master;
GO

DROP TABLE FailSafeIT_Local.dbo.Servers;
GO

DROP DATABASE FailSafeIT_Local;
GO

begin transaction -- Safeguard körning av SQL Querys.. Fungerar dock inte på DROP :)
ROLLBACK transaction --ROLLBACK ISTÄLLET FÖR COMMIT 