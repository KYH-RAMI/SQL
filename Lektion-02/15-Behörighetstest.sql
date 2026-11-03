/*
Säkerhetstest för uppgift 15.

Testar att FailsafeApp inte kan läsa Servers direkt
men kan registrera en server via Stored Procedure.

OBS: Procedurtestet lägger till en ny server vid varje körning.
*/

/*
Lektion 2 – Uppgift 15: Behörighetstest

Testar att FailsafeApp inte får läsa Servers direkt,
men kan registrera en server via en Stored Procedure.

OBS! Test 2 registrerar en ny server.
Kör inte testet flera gånger i onödan.
*/

USE FailSafeIT_Local;
GO


-- ==========================================
-- TEST 1: Kontrollera att SELECT blockeras
-- ==========================================

DECLARE @SelectError INT = NULL;

EXECUTE AS USER = 'FailsafeApp';

BEGIN TRY

    SELECT * FROM dbo.Servers;

END TRY
BEGIN CATCH

    SET @SelectError = ERROR_NUMBER();
    PRINT ERROR_MESSAGE();

END CATCH;

-- Återgå till vanliga användaren
REVERT;

IF @SelectError = 229
    PRINT 'OK: SELECT blockerades som förväntat!';
ELSE
    PRINT 'FEL: SELECT-testet gav inte förväntat resultat!';

GO


-- ==========================================
-- TEST 2: Registrera server via proceduren
-- ==========================================

BEGIN TRY

    -- Byt till FailsafeApp
    EXECUTE AS USER = 'FailsafeApp';

    -- Anropa proceduren
    EXEC dbo.usp_RegisterServer
        @ServerName = N'LeastPrivilege-Test-15',
        @IpAddress = '192.168.1.150',
        @Environment = N'Development',
        @ManagedByUserId = 1;

    PRINT 'OK: Proceduren kördes utan fel!';

    -- Återgå till vanliga användaren
    REVERT;

END TRY
BEGIN CATCH

    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();

    IF USER_NAME() = N'FailsafeApp'
        REVERT;

    PRINT 'FEL: Proceduranropet misslyckades!';
    PRINT @ErrorMessage;

END CATCH;
GO


-- ==========================================
-- TEST 3: Kontrollera att servern finns
-- ==========================================

SELECT
    s.ServerName,
    s.IpAddress,
    s.Environment,
    s.IsActive,
    u.Username
FROM dbo.Servers AS s
INNER JOIN dbo.Users AS u
    ON s.ManagedByUserId = u.Id
WHERE s.ServerName = N'LeastPrivilege-Test-15';

GO