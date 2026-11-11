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

SELECT * FROM AuditLogs_Heap
WHERE LogMessage = 'DevSecOps Audit event log #45236 - Firewall traffic authorized.';

-- SQL Server visar att den har gjort en table scan och letat igenom alla 50 000 rader.

/*
Uppgift 7 
Blixtsnabb sökning (Index Seek) – Kör en sökfråga mot dbo.AuditLogs_Clustered 
där du filtrerar på ett specifikt ID (WHERE Id = 1). Verifiera i planen att operationen har 
skiftat till en Clustered Index Seek. 
*/

-- UPPGIFT 7: Blixtsnabb sökning (Index Seek)

SELECT * FROM AuditLogs_Clustered
WHERE Id = 45236;

/*
Uppgift 8 
Den dolda kostnaden (Key Lookup) – Kör en sökfråga där du filtrerar på CreatedAt 
men väljer (SELECT) kolumnen LogMessage. Identifiera varningssymbolen Key Lookup i 
exekveringsplanen.
*/

-- UPPGIFT 8: Den dolda kostnaden (Key Lookup)

SELECT LogMessage
FROM dbo.AuditLogs_Clustered
WHERE CreatedAt = '2026-01-01T12:00:00';

/*
Uppgift 9 
Den perfekta skölden (Covering Index) – Skriv om indexet från Uppgift 3 och använd 
nyckelordet INCLUDE för att lägga till kolumnen LogMessage i lövnoderna. Kör om frågan 
från Uppgift 8 och verifiera att din Key Lookup är helt borta.
*/

-- UPPGIFT 9: Den perfekta skölden (Covering Index)
-- Bygg om indexet med INCLUDE för att lägga till texten som payload i lövnoderna

CREATE NONCLUSTERED INDEX IX_AuditLogs_CreatedAt
ON dbo.AuditLogs_Clustered (CreatedAt)
INCLUDE (LogMessage)
WITH (DROP_EXISTING = ON);
GO


/*
Uppgift 10 
SARGable kodtvätt – Du har fått en långsam fråga: SELECT * FROM dbo.Incidents 
WHERE YEAR(ReportedAt) = 2026;. Skriv om denna fråga så att den blir SARGable och 
kan utnyttja index på kolumnen ReportedAt.
*/

-- UPPGIFT 10: SARGable kodtvätt
-- Dålig query: 
SELECT * FROM AuditLogs_Clustered
WHERE YEAR(CreatedAt) = 2026;

-- Bra Query (SARGabble)
SELECT * FROM AuditLogs_Clustered
WHERE CreatedAt >= '2026-01-01 00:00:00' 
AND CreatedAt < '2027-01-01 00:00:00'; 
-- ?!

/*
Uppgift 11 
Indexera länkade nycklar (FK Optimering) – Skriv det index som alltid bör skapas på 
en Foreign Key-kolumn för att skydda raderings- och valideringsprestanda, applicerat 
på kolumnen TargetServerId i tabellen dbo.Incidents. 
*/

-- UPPGIFT 11: Indexera länkade nycklar (FK Optimering)
-- FOREIGN KEYS BLIR INTE INDEXERADE AUTOMATISKT. DET MÅSTE VI GÖRA FÖR HAND!
CREATE TABLE dbo.Incidents (
	Id INT NOT NULL PRIMARY KEY,
	TargetServerId INT NOT NULL,
	ReportedAt DATETIME2 NOT NULL,
	Description NVARCHAR(500) NOT NULL,
	FOREIGN KEY (TargetServerId) REFERENCES dbo.Servers(Id)
);

-- Testdata: Incidents
INSERT INTO dbo.Incidents (Id, TargetServerId, ReportedAt, Description)
VALUES
(1, 1, '2026-01-15 08:30:00', 'Failed login attempt'),
(2, 2, '2026-02-10 14:20:00', 'Firewall blocked suspicious traffic'),
(3, 3, '2026-03-05 09:15:00', 'Unauthorized access attempt'),
(4, 4, '2026-04-12 16:45:00', 'Security patch installed'),
(5, 1, '2026-06-20 11:00:00', 'Database connection timeout'),
(6, 2, '2026-09-08 13:30:00', 'Malware scan completed'),
(7, 3, '2025-11-15 10:00:00', 'Old security incident'),
(8, 4, '2027-01-10 12:00:00', 'Scheduled security audit');
GO
CREATE NONCLUSTERED INDEX IX_Incidents_TargetServerId
ON dbo.Incidents (TargetServerId);
GO

/*
Uppgift 12 
Contradiction Detection i praktiken – Lägg till en CHECK-constraint på tabellen 
dbo.Servers som begränsar IsActive till antingen 0 eller 1. Skriv sedan en fråga 
WHERE IsActive = 5 och verifiera i exekveringsplanen att SQL Server inte ens rör 
disken.
*/

-- UPPGIFT 12: Contradiction Detection i praktiken
CREATE TABLE dbo.ServerPolicyMaps (
	Id INT NOT NULL PRIMARY KEY,
	ServerId INT NOT NULL,
	PolicyId INT NOT NULL,
	IsActive BIT NOT NULL
);
-- Testdata: ServerPolicyMaps
INSERT INTO dbo.ServerPolicyMaps (Id, ServerId, PolicyId, IsActive)
VALUES
(1, 1, 101, 1),
(2, 1, 102, 1),
(3, 2, 101, 1),
(4, 2, 103, 0),
(5, 3, 102, 1),
(6, 3, 104, 0),
(7, 4, 101, 1),
(8, 4, 105, 1);
GO
ALTER TABLE dbo.ServerPolicyMaps
ADD CONSTRAINT CK_ServerPolicyMaps_IsActive CHECK (IsActive IN (0, 1));

SELECT * FROM dbo.ServerPolicyMaps
WHERE IsActive = 5;


/*
Uppgift 13 
Implicit konverteringsjakt – Om kolumnen MacAddress i en tabell är definierad som 
VARCHAR(17), visa med ett T-SQL-exempel hur en inmatad unicode-sträng 
(NVARCHAR, t.ex. N'00-14-22-01-23-45') tvingar fram en implicit konvertering och 
förstör indexprestanda.
*/

-- UPPGIFT 13: Implicit konverteringsjakt
CREATE TABLE dbo.NetworkDevices (
	Id INT NOT NULL PRIMARY KEY,
	MacAddress VARCHAR(17) NOT NULL
);

CREATE NONCLUSTERED INDEX IX_NetworkDevices_MacAddress
ON dbo.NetworkDevices(MacAddress);

INSERT INTO dbo.NetworkDevices (Id, MacAddress)
VALUES
(1, '00-14-22-01-23-45'),
(2, '00-14-22-01-23-46'),
(3, '00-14-22-01-23-47');

SELECT *
FROM dbo.NetworkDevices
WHERE MacAddress = '00-14-22-01-23-45';

-- MED UNICODE -PREFIX
SELECT *
FROM dbo.NetworkDevices
WHERE MacAddress = N'00-14-22-01-23-45';

/*
Uppgift 14 
Smutsiga läsningar (NOLOCK) – Skriv en sökfråga mot dbo.Servers som använder 
isoleringsnivån READ UNCOMMITTED (eller NOLOCK-hinten). Ange i en kommentar när detta 
är acceptabelt och när det är direkt livsfarligt ur ett finansiellt perspektiv.
*/

-- UPPGIFT 14: Smutsiga läsningar (NOLOCK)

SELECT * FROM dbo.Servers WITH (NOLOCK)

-- Acceptabelt: Vid enklare felsökning eller analys där
-- Tillfälligt felaktiga eller inkonsekventa resultat kan tolereras.

-- Livsfarligt: När man hanterar finansiella transaktioner, bankkonton eller annan känslig information där det är avgörande att alltid ha korrekt och uppdaterad data.


/*
Uppgift 15 
Minsta behörighet vid prestandafelsökning – Skapa en databasanvändare 
DB_Profiler utan login. Tilldela denna användare enbart de systemrättigheter som 
krävs för att kunna köra SHOWPLAN och läsa exekveringsplaner i din databas, helt enligt 
principen om Least Privilege.
*/

-- UPPGIFT 15: Minsta behörighet vid prestandafelsökning

CREATE USER DB_Profiler WITHOUT LOGIN;

-- Ge endast behörighet att visa exekveringsplaner
GRANT SHOWPLAN TO DB_Profiler;

