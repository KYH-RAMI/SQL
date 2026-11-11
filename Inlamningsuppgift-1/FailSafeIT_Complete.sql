-- =========================================================================
-- FAILSAFE IT – INLÄMNINGSUPPGIFT 1
-- Självständigt och upprepningsbart SQL-skript
-- OBS: Databasen raderas och återskapas vid varje körning.
-- =========================================================================

USE master;
GO

IF DB_ID('FailSafeIT_Local') IS NOT NULL
BEGIN
    ALTER DATABASE FailSafeIT_Local
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

    DROP DATABASE FailSafeIT_Local;
END;
GO

CREATE DATABASE FailSafeIT_Local;
GO

USE FailSafeIT_Local;
GO

-- =========================================================================
-- DEL 1: Skapa tabeller
-- =========================================================================

-- 1. Customers
CREATE TABLE dbo.Customers
(
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    OrganizationNumber VARCHAR(10) NOT NULL UNIQUE,
    CompanyName NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Phone VARCHAR(20) NOT NULL
);
GO

-- 2. Consultants
CREATE TABLE dbo.Consultants
(
    ConsultantID INT IDENTITY(1,1) PRIMARY KEY,
    PersonalNumber VARCHAR(12) NOT NULL UNIQUE,
    ConsultantName NVARCHAR(100) NOT NULL,
    HourlyPrice DECIMAL(10, 2) NOT NULL CHECK (HourlyPrice >= 0),
    Specialty NVARCHAR(100) NOT NULL
);
GO

-- 3. Assignments
CREATE TABLE dbo.Assignments
(
    AssignmentID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL REFERENCES dbo.Customers(CustomerID),
    ConsultantID INT NOT NULL REFERENCES dbo.Consultants(ConsultantID),
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    RoleDescription NVARCHAR(150) NOT NULL,
    CONSTRAINT CK_Assignments_Dates CHECK (EndDate >= StartDate)
);
GO

-- DEL 2: Lägg in testdata

-- 1. Customers
INSERT INTO dbo.Customers (OrganizationNumber, CompanyName, Email, Phone)
VALUES
    ('5591234567', 'NordTech AB', 'info@nordtech.ex', '081234567'),
    ('5592345678', 'SecureCloud AB', 'kontakt@securecloud.ex', '082345678'),
    ('5591555555', 'Rami Solutions AB', 'info@ramisolutions.ex', '083456789');
GO

-- 2. Consultants
INSERT INTO dbo.Consultants (PersonalNumber, ConsultantName, HourlyPrice, Specialty)
VALUES
    ('198901010001', 'Rami Issa', 1450.00, 'DevSecOps'),
    ('197001010002', 'Tim Corey', 2250.00, 'DevOps'),
    ('198502020003', 'Clay Dummy', 1250.00, 'Database Security');
GO

-- 3. Assignments
INSERT INTO dbo.Assignments (CustomerID, ConsultantID, StartDate, EndDate, RoleDescription)
VALUES
    (1, 1, '2026-11-01', '2027-03-31', 'DevSecOps Implementation'),
    (2, 2, '2026-10-15', '2027-01-15', 'Security Monitoring'),
    (3, 3, '2026-09-01', '2026-12-31', 'Database Security Audit'),
    (1, 3, '2026-11-10', '2027-02-28', 'SQL Injection Prevention'),
    (2, 1, '2026-08-01', '2026-10-31', 'Infrastructure Security Review');
GO

-- DEL 3: Frågor A–E

-- FRÅGA A: Visa konsulter med timpris över 1 200, dyrast först
SELECT ConsultantName, HourlyPrice
FROM dbo.Consultants
WHERE HourlyPrice > 1200
ORDER BY HourlyPrice DESC;
GO

-- FRÅGA B: Visa företag, konsult och uppdragsdatum
SELECT c.CompanyName, cu.ConsultantName, a.StartDate, a.EndDate
FROM dbo.Assignments AS a
INNER JOIN dbo.Customers AS c
    ON a.CustomerID = c.CustomerID
INNER JOIN dbo.Consultants AS cu
    ON a.ConsultantID = cu.ConsultantID;
GO

-- FRÅGA C: Räkna aktiva uppdrag per kund
SELECT c.CompanyName, COUNT(*) AS AntalUppdrag
FROM dbo.Assignments AS a
INNER JOIN dbo.Customers AS c
    ON a.CustomerID = c.CustomerID
WHERE a.StartDate <= CAST(GETDATE() AS DATE)
  AND a.EndDate >= CAST(GETDATE() AS DATE)
GROUP BY c.CustomerID, c.CompanyName;
GO

-- FRÅGA D: Uppdatera timpriset för konsult 2
-- UPDATE är kommenterad för att undvika oavsiktlig ändring av testdata.
/*
UPDATE dbo.Consultants
SET HourlyPrice = 2500
WHERE ConsultantID = 2;
*/

SELECT ConsultantName, HourlyPrice
FROM dbo.Consultants
WHERE ConsultantID = 2;
GO

-- FRÅGA E: Kontrollera kunder som saknar aktiva uppdrag
SELECT *
FROM dbo.Customers AS c
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.Assignments AS a
    WHERE a.CustomerID = c.CustomerID
      AND a.StartDate <= CAST(GETDATE() AS DATE)
      AND a.EndDate >= CAST(GETDATE() AS DATE)
);
GO

-- DELETE är kommenterad för att undvika oavsiktlig radering.
-- OBS: FK FOREIGN KEY hindrar även radering om kunden har avslutade uppdrag.
/*
DELETE c
FROM dbo.Customers AS c
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.Assignments AS a
    WHERE a.CustomerID = c.CustomerID
      AND a.StartDate <= CAST(GETDATE() AS DATE)
      AND a.EndDate >= CAST(GETDATE() AS DATE)
);
*/
GO
