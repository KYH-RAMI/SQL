-- ==========================================
-- LEKTION 4 - ÖVNINGSUPPGIFTER
-- ==========================================

USE master;

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'FailSafe_Lektion4')
BEGIN
ALTER DATABASE FailSafe_Lektion4 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE FailSafe_Lektion4;
END;
GO

CREATE DATABASE FailSafe_Lektion4;
GO

USE FailSafe_Lektion4;
GO

/*
Skapa en Hög (Heap) – Skriv ett DDL-skript som skapar en tabell dbo.AuditLogs_Heap 
helt utan primärnyckel eller index. Lägg till kolumnerna Id INT, LogMessage 
NVARCHAR(500) och CreatedAt DATETIME. 
*/

-- UPPGIFT 1: Skapa en hög (Heap)
CREATE TABLE dbo.AuditLogs_Heap (
	Id INT NOT NULL,
	LogMessage NVARCHAR(500) NOT NULL,
	CreatedAt DATETIME2 NOT NULL
);
GO

/*
Uppgift 2 
Det automatiska indexet (Clustered) – Skriv ett skript som skapar tabellen 
dbo.AuditLogs_Clustered med exakt samma kolumner som i Uppgift 1, men lägg till 
en PRIMARY KEY CLUSTERED på kolumnen Id.
*/

-- UPPGIFT 2: Skapa en tabell med ett klusterat index
CREATE TABLE dbo.AuditLogs_Clustered (
	Id INT NOT NULL,
	LogMessage NVARCHAR(500) NOT NULL,
	CreatedAt DATETIME2 NOT NULL,
	CONSTRAINT PK_AuditLogs_Clustered PRIMARY KEY CLUSTERED (Id)
);
GO

-- POPULERA 50 000 RADER I TESTDATA
-- Sätt tidmätning och räknare i minnet
SET NOCOUNT ON;
DECLARE @COUNTER INT = 1;
DECLARE @RANDOMMESSAGE NVARCHAR(500);
DECLARE @RANDOMDATE DATETIME2;

--STARTA LOOPEN FÖR 50 000 EXEKVERINGAR
WHILE @COUNTER <= 50000 
BEGIN
	-- Generera slumpmässiga värden
	SET @RANDOMMESSAGE = N'DevSecOps Audit event log # ' 
		+ CAST(@COUNTER AS NVARCHAR(10))
		+ N' - Firewall traffic authorized.';
	SET @RANDOMDATE = DATEADD(SECOND, @COUNTER, '2026-01-01 00:00:00');

	-- Infoga in i Heap-tabellen
	INSERT INTO dbo.AuditLogs_Heap(Id, LogMessage, CreatedAt)
	VALUES (@COUNTER, @RANDOMMESSAGE, @RANDOMDATE);
	-- Infoga in i Clustered-tabellen
	INSERT INTO dbo.AuditLogs_Clustered(Id, LogMessage, CreatedAt)
	VALUES (@COUNTER, @RANDOMMESSAGE, @RANDOMDATE);

	-- Öka räknaren
	SET @COUNTER = @COUNTER + 1;
END;
GO

-- Återställ systeminställningen räknaren
SET NOCOUNT OFF;
PRINT '50 000 rader har skapats i tabellerna dbo.AuditLogs_Heap och dbo.AuditLogs_Clustered.';

SELECT 'AuditLogs_Heap' AS TableName, COUNT(*) AS TotalRows_Heap FROM dbo.AuditLogs_Heap
UNION ALL
SELECT 'AuditLogs_Clustered', COUNT(*) FROM dbo.AuditLogs_Clustered;

/*

Uppgift 3 
Det kirurgiska snittet (Non-Clustered) – Skapa ett vanligt, icke-klustrat index med 
namnet IX_AuditLogs_CreatedAt på kolumnen CreatedAt i tabellen 
dbo.AuditLogs_Clustered.
*/

-- UPPGIFT 3: Skapa ett icke-klustrat index
CREATE NONCLUSTERED INDEX IX_AuditLogs_CreatedAt ON dbo.AuditLogs_Clustered (CreatedAt);
GO

/*

Uppgift 4 
Verifiera indexstrukturen – Skriv en systemfråga mot sys.indexes som visar namn 
och type_desc för alla index som existerar på tabellen dbo.AuditLogs_Clustered. 
*/

-- UPPGIFT 4: Verifiera indexstrukturen
SELECT TOP 10 * FROM sys.indexes;

SELECT name, type_desc, IS_UNIQUE 
FROM sys.indexes 
WHERE object_id = OBJECT_ID('dbo.AuditLogs_Clustered');

/*
Uppgift 5 
Säkerställa unika värden – Skapa en UNIQUE constraint på kolumnen ServerName i 
tabellen dbo.Servers. Förklara i en kommentar vilket index SQL Server skapar 
automatiskt under motorhuven för att lösa detta.
*/
CREATE TABLE dbo.Servers (
	Id INT NOT NULL PRIMARY KEY,
	ServerName NVARCHAR(255) NOT NULL
);

INSERT INTO dbo.Servers (Id, ServerName) VALUES (1, 'DEV-DB-01');
INSERT INTO dbo.Servers (Id, ServerName) VALUES (2, 'PROD-WEB-01');
INSERT INTO dbo.Servers (Id, ServerName) VALUES (3, 'STAGE-APP-01');
INSERT INTO dbo.Servers (Id, ServerName) VALUES (4, 'STAGE-APP-02');

-- Skapa unique index 
ALTER TABLE dbo.Servers 
ADD CONSTRAINT UQ_Servers_ServerName UNIQUE (ServerName);

-- Ger oss ett (UNIQUE  non-clustered index) på kolumnen ServerName
-- för att säkerställa unika värden.

/*
Uppgift 6 
Detektera en Table Scan – Slå på "Actual Execution Plan" (Ctrl + M). Skriv och kör en 
sökfråga som letar efter ett specifikt textmeddelan
*/

-- UPPGIFT 6: Detektera en Table Scan



