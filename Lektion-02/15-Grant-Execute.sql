/*
Lektion 2 – Uppgift 15: GRANT EXECUTE

FailsafeApp får köra proceduren som registrerar servrar,
men får inte läsa eller ändra tabellerna direkt.
*/

USE FailSafeIT_Local;
GO

-- Ge FailsafeApp rätt att köra proceduren
GRANT EXECUTE ON OBJECT::dbo.usp_RegisterServer
TO FailsafeApp;
GO

-- Kontrollera att behörigheten finns
SELECT
    permission_name,
    state_desc
FROM sys.database_permissions
WHERE grantee_principal_id = USER_ID(N'FailsafeApp')
  AND major_id = OBJECT_ID(N'dbo.usp_RegisterServer');
GO