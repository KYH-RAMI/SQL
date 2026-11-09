-- ==========================================
-- LEKTION 3 - ÖVNINGSUPPGIFTER
-- ==========================================

USE FailSafeIT_Local;
GO

-- UPPGIFT 1: Skapa tabellen Incidents (1:M)
-- Severity => "Low", "Medium", "High", "Critical"

CREATE TABLE 
	dbo.Incidents (
		Id INT IDENTITY(1,1) PRIMARY KEY,
		Severity NVARCHAR(10) NOT NULL,
		Description NVARCHAR(500) NOT NULL,
		TargetServerId INT NOT NULL REFERENCES dbo.Servers(Id)
		);
GO
-- UPPGIFT 2: Registrera tre incidenter (DML)

INSERT INTO dbo.Incidents 
			(Severity, Description, TargetServerId) 
		VALUES
			('Critical', 'DB DOWN', 1),
			('High', 'DDOS ATTACK', 1),
			('Low', 'SERVER RESTART', 1);
GO

-- UPPGIFT 3: INNER JOIN

SELECT s.ServerName, s.IpAddress, i.Severity, i.Description
FROM dbo.Servers AS s
INNER JOIN dbo.Incidents AS i
ON s.Id = i.TargetServerId;

GO

-- UPPGIFT 4: Säkerhetsprofiler (M:M)

CREATE TABLE 
	dbo.SecurityProfiles (
		Id INT IDENTITY(1,1) PRIMARY KEY,
		ProfileName NVARCHAR(30) NOT NULL,
		EncryptionStandard VARCHAR(30) NOT NULL
	);
GO

-- UPPGIFT 5: Kopplingstabellens grund (Composite PK)

CREATE TABLE 
	dbo.ServerSecurityMapping (
		ServerId INT NOT NULL REFERENCES dbo.Servers(Id),
		ProfileId INT NOT NULL REFERENCES dbo.SecurityProfiles(Id)

		PRIMARY KEY (ServerId, ProfileId)
		);
GO

-- UPPGIFT 6: Anomalijägaren 

/*

Detta är en uppdateringsanomali eftersom teknikerns telefonnummer lagras på flera serverrader. 
Om numret ändras måste samtliga rader uppdateras, 
vilket riskerar att skapa inkonsekvent data. 
Genom att lagra teknikeruppgifter i en separat tabell och använda en främmande nyckel 
behöver telefonnumret bara ändras på ett ställe.

*/

-- UPPGIFT 7: (UNF till 1NF)

CREATE TABLE 
		dbo.ServerIpAddresses 
			(Id INT IDENTITY(1,1) PRIMARY KEY,
			ServerId INT NOT NULL REFERENCES dbo.Servers(Id),
			IpAddress VARCHAR(45) NOT NULL); 

GO

INSERT INTO dbo.ServerIpAddresses
			(ServerId, IpAddress) 
		VALUES
			(1, '10.0.0.15'),
			(1, '192.168.1.10');
GO

-- UPPGIFT 8: Från 2NF till 3NF

-- STEG 1 
CREATE TABLE 
		dbo.DataCenters
		(Id INT IDENTITY(1,1) PRIMARY KEY,
		Name NVARCHAR(100) NOT NULL);

GO
-- STEG 2
	ALTER TABLE 
		dbo.Servers 
		Add DataCenterId INT NULL REFERENCES dbo.DataCenters(Id);

	GO

-- UPPGIFT 11: (CHECK Constraint)

ALTER TABLE 
		dbo.Incidents 
		ADD CONSTRAINT CHK_Incidents_Severity
		CHECK (Severity IN ('Low', 'Medium', 'High', 'Critical'));
GO

-- UPPGIFT 12: CHECK Constraint på Servers

ALTER TABLE 
		dbo.Servers
		ADD CONSTRAINT CHK_Servers_Environment
		CHECK (Environment IN ('Production', 'Staging', 'Development'));
GO

-- UPPGIFT 13: Foreign Key med ON DELETE SET NULL
-- FK_Servers_Users finns redan från tidigare lektion.
-- ManagedByUserId tillåter NULL.
-- Relationens DELETE_ACTION är SET NULL.
-- Därför behöver ingen ny Foreign Key skapas.


-- UPPGIFT 14: Databasbehörigheter (GRANT SELECT)

USE FailSafeIT_Local;
GO

CREATE USER Auditor_Role WITHOUT LOGIN;
GRANT SELECT ON dbo.Servers TO Auditor_Role;
GRANT SELECT ON dbo.Incidents TO Auditor_Role;

-- UPPGIFT 15: Skapa en 1:1-relation

CREATE TABLE dbo.ServerSecretKeys (
    ServerId INT PRIMARY KEY REFERENCES dbo.Servers(Id),
	EncryptedKey NVARCHAR(255) NOT NULL UNIQUE
);