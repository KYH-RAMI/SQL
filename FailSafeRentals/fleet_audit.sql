IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261115112447_InitialCreate'
)
BEGIN
    CREATE TABLE [Customers] (
        [Id] int NOT NULL IDENTITY,
        [FullName] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [DriverLicenseNumber] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261115112447_InitialCreate'
)
BEGIN
    CREATE TABLE [ExtraFeatures] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(50) NOT NULL,
        [DailyPrice] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_ExtraFeatures] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261115112447_InitialCreate'
)
BEGIN
    CREATE TABLE [Vehicles] (
        [Id] int NOT NULL IDENTITY,
        [RegistrationNumber] nvarchar(10) NOT NULL,
        [Brand] nvarchar(50) NOT NULL,
        [Model] nvarchar(50) NOT NULL,
        [DailyRate] decimal(10,2) NOT NULL,
        [IsAvailable] bit NOT NULL,
        CONSTRAINT [PK_Vehicles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261115112447_InitialCreate'
)
BEGIN
    CREATE TABLE [ExtraFeatureVehicle] (
        [ExtraFeaturesId] int NOT NULL,
        [VehiclesId] int NOT NULL,
        CONSTRAINT [PK_ExtraFeatureVehicle] PRIMARY KEY ([ExtraFeaturesId], [VehiclesId]),
        CONSTRAINT [FK_ExtraFeatureVehicle_ExtraFeatures_ExtraFeaturesId] FOREIGN KEY ([ExtraFeaturesId]) REFERENCES [ExtraFeatures] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ExtraFeatureVehicle_Vehicles_VehiclesId] FOREIGN KEY ([VehiclesId]) REFERENCES [Vehicles] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261115112447_InitialCreate'
)
BEGIN
    CREATE TABLE [Rentals] (
        [Id] int NOT NULL IDENTITY,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NULL,
        [TotalCost] decimal(18,2) NOT NULL,
        [CustomerId] int NOT NULL,
        [VehicleId] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        CONSTRAINT [PK_Rentals] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Rentals_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Rentals_Vehicles_VehicleId] FOREIGN KEY ([VehicleId]) REFERENCES [Vehicles] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261115112447_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ExtraFeatureVehicle_VehiclesId] ON [ExtraFeatureVehicle] ([VehiclesId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261115112447_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Rentals_CustomerId] ON [Rentals] ([CustomerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261115112447_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Rentals_VehicleId] ON [Rentals] ([VehicleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261115112447_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Vehicles_RegistrationNumber] ON [Vehicles] ([RegistrationNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261115112447_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261115112447_InitialCreate', N'11.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116091054_AddedFleetManagement'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME(OBJECT_NAME([c].[default_object_id]))
    FROM [sys].[columns] [c]
    WHERE [c].[object_id] = OBJECT_ID(N'[Customers]') AND [c].[name] = N'FullName';
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Customers] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Customers] ALTER COLUMN [FullName] nvarchar(100) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116091054_AddedFleetManagement'
)
BEGIN
    DECLARE @var1 nvarchar(max);
    SELECT @var1 = QUOTENAME(OBJECT_NAME([c].[default_object_id]))
    FROM [sys].[columns] [c]
    WHERE [c].[object_id] = OBJECT_ID(N'[Customers]') AND [c].[name] = N'Email';
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Customers] DROP CONSTRAINT ' + @var1 + ';');
    ALTER TABLE [Customers] ALTER COLUMN [Email] nvarchar(200) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116091054_AddedFleetManagement'
)
BEGIN
    DECLARE @var2 nvarchar(max);
    SELECT @var2 = QUOTENAME(OBJECT_NAME([c].[default_object_id]))
    FROM [sys].[columns] [c]
    WHERE [c].[object_id] = OBJECT_ID(N'[Customers]') AND [c].[name] = N'DriverLicenseNumber';
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Customers] DROP CONSTRAINT ' + @var2 + ';');
    ALTER TABLE [Customers] ALTER COLUMN [DriverLicenseNumber] nvarchar(50) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116091054_AddedFleetManagement'
)
BEGIN
    CREATE TABLE [RentalStations] (
        [Id] int NOT NULL IDENTITY,
        [StationCode] nvarchar(10) NOT NULL,
        [City] nvarchar(50) NOT NULL,
        [StreetAddress] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_RentalStations] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116091054_AddedFleetManagement'
)
BEGIN
    CREATE TABLE [FleetVehicles] (
        [Id] int NOT NULL IDENTITY,
        [RegistrationNumber] nvarchar(10) NOT NULL,
        [MakeAndModel] nvarchar(100) NOT NULL,
        [MileageKm] int NOT NULL,
        [NeedsService] bit NOT NULL,
        [RentalStationId] int NOT NULL,
        CONSTRAINT [PK_FleetVehicles] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_FleetVehicles_RentalStations_RentalStationId] FOREIGN KEY ([RentalStationId]) REFERENCES [RentalStations] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116091054_AddedFleetManagement'
)
BEGIN
    CREATE TABLE [DamageReports] (
        [Id] int NOT NULL IDENTITY,
        [Description] nvarchar(1000) NOT NULL,
        [Severity] nvarchar(20) NOT NULL,
        [EstimatedCost] decimal(18,2) NOT NULL,
        [ReportedAt] datetime2 NOT NULL,
        [RepairedAt] datetime2 NULL,
        [FleetVehicleId] int NOT NULL,
        CONSTRAINT [PK_DamageReports] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DamageReports_FleetVehicles_FleetVehicleId] FOREIGN KEY ([FleetVehicleId]) REFERENCES [FleetVehicles] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116091054_AddedFleetManagement'
)
BEGIN
    CREATE INDEX [IX_DamageReports_FleetVehicleId] ON [DamageReports] ([FleetVehicleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116091054_AddedFleetManagement'
)
BEGIN
    CREATE INDEX [IX_FleetVehicles_RentalStationId] ON [FleetVehicles] ([RentalStationId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116091054_AddedFleetManagement'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261116091054_AddedFleetManagement', N'11.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116094431_AddFleetIndexesAndPrecision'
)
BEGIN
    DECLARE @var3 nvarchar(max);
    SELECT @var3 = QUOTENAME(OBJECT_NAME([c].[default_object_id]))
    FROM [sys].[columns] [c]
    WHERE [c].[object_id] = OBJECT_ID(N'[RentalStations]') AND [c].[name] = N'City';
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [RentalStations] DROP CONSTRAINT ' + @var3 + ';');
    ALTER TABLE [RentalStations] ALTER COLUMN [City] nvarchar(80) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116094431_AddFleetIndexesAndPrecision'
)
BEGIN
    DECLARE @var4 nvarchar(max);
    SELECT @var4 = QUOTENAME(OBJECT_NAME([c].[default_object_id]))
    FROM [sys].[columns] [c]
    WHERE [c].[object_id] = OBJECT_ID(N'[DamageReports]') AND [c].[name] = N'EstimatedCost';
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [DamageReports] DROP CONSTRAINT ' + @var4 + ';');
    ALTER TABLE [DamageReports] ALTER COLUMN [EstimatedCost] decimal(10,2) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116094431_AddFleetIndexesAndPrecision'
)
BEGIN
    CREATE UNIQUE INDEX [IX_RentalStations_StationCode] ON [RentalStations] ([StationCode]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116094431_AddFleetIndexesAndPrecision'
)
BEGIN
    CREATE UNIQUE INDEX [IX_FleetVehicles_RegistrationNumber] ON [FleetVehicles] ([RegistrationNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116094431_AddFleetIndexesAndPrecision'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261116094431_AddFleetIndexesAndPrecision', N'11.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116094948_AddDamageAuditShadowProperty'
)
BEGIN
    ALTER TABLE [DamageReports] ADD [AuditLoggedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE());
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116094948_AddDamageAuditShadowProperty'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261116094948_AddDamageAuditShadowProperty', N'11.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116101316_RestrictDeleteOnFleet'
)
BEGIN
    ALTER TABLE [DamageReports] DROP CONSTRAINT [FK_DamageReports_FleetVehicles_FleetVehicleId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116101316_RestrictDeleteOnFleet'
)
BEGIN
    ALTER TABLE [FleetVehicles] DROP CONSTRAINT [FK_FleetVehicles_RentalStations_RentalStationId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116101316_RestrictDeleteOnFleet'
)
BEGIN
    ALTER TABLE [FleetVehicles] ADD [DamageReportId] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116101316_RestrictDeleteOnFleet'
)
BEGIN
    ALTER TABLE [DamageReports] ADD CONSTRAINT [FK_DamageReports_FleetVehicles_FleetVehicleId] FOREIGN KEY ([FleetVehicleId]) REFERENCES [FleetVehicles] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116101316_RestrictDeleteOnFleet'
)
BEGIN
    ALTER TABLE [FleetVehicles] ADD CONSTRAINT [FK_FleetVehicles_RentalStations_RentalStationId] FOREIGN KEY ([RentalStationId]) REFERENCES [RentalStations] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116101316_RestrictDeleteOnFleet'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261116101316_RestrictDeleteOnFleet', N'11.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116103846_AddMaintenanceTagsManyToMany'
)
BEGIN
    CREATE TABLE [MaintenanceTags] (
        [Id] int NOT NULL IDENTITY,
        [TagName] nvarchar(40) NOT NULL,
        CONSTRAINT [PK_MaintenanceTags] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116103846_AddMaintenanceTagsManyToMany'
)
BEGIN
    CREATE TABLE [FleetVehicleMaintenanceTag] (
        [MaintenanceTagsId] int NOT NULL,
        [VehiclesId] int NOT NULL,
        CONSTRAINT [PK_FleetVehicleMaintenanceTag] PRIMARY KEY ([MaintenanceTagsId], [VehiclesId]),
        CONSTRAINT [FK_FleetVehicleMaintenanceTag_FleetVehicles_VehiclesId] FOREIGN KEY ([VehiclesId]) REFERENCES [FleetVehicles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_FleetVehicleMaintenanceTag_MaintenanceTags_MaintenanceTagsId] FOREIGN KEY ([MaintenanceTagsId]) REFERENCES [MaintenanceTags] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116103846_AddMaintenanceTagsManyToMany'
)
BEGIN
    CREATE INDEX [IX_FleetVehicleMaintenanceTag_VehiclesId] ON [FleetVehicleMaintenanceTag] ([VehiclesId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116103846_AddMaintenanceTagsManyToMany'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261116103846_AddMaintenanceTagsManyToMany', N'11.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116105139_SeedRentalStations'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'City', N'StationCode', N'StreetAddress') AND [object_id] = OBJECT_ID(N'[RentalStations]'))
        SET IDENTITY_INSERT [RentalStations] ON;
    EXEC(N'INSERT INTO [RentalStations] ([Id], [City], [StationCode], [StreetAddress])
    VALUES (1, N''Göteborg'', N''GOT-01'', N''Landvetter flygplats 1''),
    (2, N''Stockholm'', N''STO-01'', N''Arlanda Terminal 5''),
    (3, N''Malmö'', N''MMX-01'', N''Sturupsvägen 3'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'City', N'StationCode', N'StreetAddress') AND [object_id] = OBJECT_ID(N'[RentalStations]'))
        SET IDENTITY_INSERT [RentalStations] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116105139_SeedRentalStations'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261116105139_SeedRentalStations', N'11.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116112836_SplitMakeAndModel'
)
BEGIN
    ALTER TABLE [FleetVehicles] ADD [Make] nvarchar(40) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116112836_SplitMakeAndModel'
)
BEGIN
    ALTER TABLE [FleetVehicles] ADD [Model] nvarchar(40) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116112836_SplitMakeAndModel'
)
BEGIN
    UPDATE FleetVehicles SET Make = MakeAndModel;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116112836_SplitMakeAndModel'
)
BEGIN
    DECLARE @var5 nvarchar(max);
    SELECT @var5 = QUOTENAME(OBJECT_NAME([c].[default_object_id]))
    FROM [sys].[columns] [c]
    WHERE [c].[object_id] = OBJECT_ID(N'[FleetVehicles]') AND [c].[name] = N'MakeAndModel';
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [FleetVehicles] DROP CONSTRAINT ' + @var5 + ';');
    ALTER TABLE [FleetVehicles] DROP COLUMN [MakeAndModel];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261116112836_SplitMakeAndModel'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261116112836_SplitMakeAndModel', N'11.0.0');
END;

COMMIT;
GO
