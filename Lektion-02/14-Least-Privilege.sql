/*
Lektion 2 – Uppgift 14: Least Privilege

Skapar en databasanvändare och begränsar
vad den får göra i Users och Servers.

FailsafeApp ska inte kunna läsa, lägga till,
ändra eller radera data direkt i tabellerna.

OBS: SQL-inloggningen FailsafeApp måste redan finnas.
Lösenord sparas inte i detta skript.
*/

USE FailSafeIT_Local;
GO

-- Skapa databasanvändaren om den inte redan finns
IF NOT EXISTS (
    SELECT 1
    FROM sys.database_principals
    WHERE name = N'FailsafeApp'
)
BEGIN
    CREATE USER FailsafeApp FOR LOGIN FailsafeApp;
END;
GO

-- Begränsa åtkomst till Users
DENY SELECT ON OBJECT::dbo.Users TO FailsafeApp;
DENY INSERT ON OBJECT::dbo.Users TO FailsafeApp;
DENY UPDATE ON OBJECT::dbo.Users TO FailsafeApp;
DENY DELETE ON OBJECT::dbo.Users TO FailsafeApp;

-- Begränsa åtkomst till Servers
DENY SELECT ON OBJECT::dbo.Servers TO FailsafeApp;
DENY INSERT ON OBJECT::dbo.Servers TO FailsafeApp;
DENY UPDATE ON OBJECT::dbo.Servers TO FailsafeApp;
DENY DELETE ON OBJECT::dbo.Servers TO FailsafeApp;

-- Kontrollera användarens behörigheter
SELECT
    OBJECT_NAME(major_id) AS TableName,
    permission_name,
    state_desc
FROM sys.database_permissions
WHERE grantee_principal_id = USER_ID('FailsafeApp')
  AND major_id IN (
      OBJECT_ID('dbo.Users'),
      OBJECT_ID('dbo.Servers')
  )
ORDER BY TableName, permission_name;