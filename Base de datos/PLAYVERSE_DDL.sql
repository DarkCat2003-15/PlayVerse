-- CREAR BASE DE DATOS
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'PLAYVERSE')
    CREATE DATABASE PLAYVERSE;
GO

USE PLAYVERSE
GO


-- LIMPIAR TABLAS EN ORDEN DE DEPENDENCIAS PARA RECREAR LA BD
DROP TABLE IF EXISTS dbo.UserRoles;
DROP TABLE IF EXISTS dbo.RolePermissions;
DROP TABLE IF EXISTS dbo.UserAchievements;
DROP TABLE IF EXISTS dbo.ReviewAnswers;
DROP TABLE IF EXISTS dbo.Reviews;
DROP TABLE IF EXISTS dbo.Friends;
DROP TABLE IF EXISTS dbo.Sessions;
DROP TABLE IF EXISTS dbo.Wishlist;
DROP TABLE IF EXISTS dbo.Library;
DROP TABLE IF EXISTS dbo.Offers;
DROP TABLE IF EXISTS dbo.DLCs;
DROP TABLE IF EXISTS dbo.Game_Genre;
DROP TABLE IF EXISTS dbo.Achievements;
DROP TABLE IF EXISTS dbo.Games;
DROP TABLE IF EXISTS dbo.Genre;
DROP TABLE IF EXISTS dbo.Publishers;
DROP TABLE IF EXISTS dbo.Developer;
DROP TABLE IF EXISTS dbo.EmailTemplates;
DROP TABLE IF EXISTS dbo.Permissions;
DROP TABLE IF EXISTS dbo.Roles;
DROP TABLE IF EXISTS dbo.Users;
DROP TABLE IF EXISTS dbo.Status;
GO

-- TABLA STATUS

CREATE TABLE Status(
    StatusID INT NOT NULL PRIMARY KEY, 
    Code NVARCHAR(20) NOT NULL,
    ShowName NVARCHAR(20) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

INSERT INTO Status (StatusID, Code, ShowName)
VALUES 
(1, 'ACTIVE', 'Activo'),
(0, 'INACTIVE', 'Desactivo');

--TABLA Usuario
CREATE TABLE Users (
    UserID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) UNIQUE,
    Password NVARCHAR(255),
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    DeletedAt DATETIME2,
    StatusID INT,
    FOREIGN KEY (StatusID) REFERENCES Status(StatusID)
);

-- TABLA AMIGOS
CREATE TABLE Friends (
    UserID UNIQUEIDENTIFIER,
    FriendID UNIQUEIDENTIFIER,
    AddedAt DATETIME2 DEFAULT SYSUTCDATETIME(),

    PRIMARY KEY (UserID, FriendID),

    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (FriendID) REFERENCES Users(UserID)
);

--TABLA DESARROLLADOR
CREATE TABLE Developer (
    DeveloperID UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    DeveloperName NVARCHAR(150),
    Email NVARCHAR(150),
	Password NVARCHAR(255),
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    DeletedAt DATETIME2
);
-- TABLA EDITOR
CREATE TABLE Publishers (
    PublisherID UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    PublisherName NVARCHAR(150),
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    DeletedAt DATETIME2
);

--TABLA JUEGOS
CREATE TABLE Games (
    GameID UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    Title NVARCHAR(200),
    Description NVARCHAR(1000),
    ImageUrl NVARCHAR(600),
    ReleaseDate DATE,
    Price DECIMAL(10,2),

    DeveloperID UNIQUEIDENTIFIER,
    PublisherID UNIQUEIDENTIFIER,

    DeletedAt DATETIME2,

    FOREIGN KEY (DeveloperID) REFERENCES Developer(DeveloperID),
    FOREIGN KEY (PublisherID) REFERENCES Publishers(PublisherID)
);

-- TABLA LOGROS:
CREATE TABLE Achievements (
    AchievementID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(200),
    Description NVARCHAR(500),
    GameID UNIQUEIDENTIFIER,

    FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

-- TABLA LOGROS DE USUARIO:
CREATE TABLE UserAchievements (
    UserAchievementID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserID UNIQUEIDENTIFIER,
    AchievementID INT,
    UnlockedAt DATETIME2,

    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (AchievementID) REFERENCES Achievements(AchievementID)
);

--TABLA DE RESEÑA
CREATE TABLE Reviews (
    ReviewID INT IDENTITY(1,1) PRIMARY KEY,
    UserID UNIQUEIDENTIFIER,
    GameID UNIQUEIDENTIFIER,
    IsRecommended BIT,
    Comment NVARCHAR(1000),
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2,

    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

-- TABLA DE RESPUESTAS DE RESEÑAS
CREATE TABLE ReviewAnswers (
	ReviewAnswersId INT NOT NULL IDENTITY(1,1),
    ReviewId INT,
    UserID UNIQUEIDENTIFIER,
	Comment NVARCHAR(1000),
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2,
    DeletedAt DATETIME2,

    PRIMARY KEY (ReviewId, UserID),

    FOREIGN KEY (ReviewId) REFERENCES Reviews(ReviewID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- TABLA DE LIBRERIA
CREATE TABLE Library (
    LibraryID INT IDENTITY(1,1) PRIMARY KEY,
    UserID UNIQUEIDENTIFIER,
    GameID UNIQUEIDENTIFIER,
    PurchasePrice DECIMAL(10,2),
    PurchaseDate DATETIME2,

    CONSTRAINT UQ_Library_User_Game UNIQUE (UserID, GameID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

-- TABLA LISTA DE DESEOS
CREATE TABLE Wishlist (
    WishlistID INT IDENTITY(1,1) PRIMARY KEY,
    UserID UNIQUEIDENTIFIER,
    GameID UNIQUEIDENTIFIER,
    AddedAt DATETIME2 DEFAULT SYSUTCDATETIME(),

    CONSTRAINT UQ_Wishlist_User_Game UNIQUE (UserID, GameID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

-- TABLA DE SESIONES

CREATE TABLE Sessions (
    SessionID INT IDENTITY(1,1) PRIMARY KEY,
    UserID UNIQUEIDENTIFIER,
    GameID UNIQUEIDENTIFIER,
    StartTime DATETIME2,
    EndTime DATETIME2,

    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

-- TABLA DE DLCs
CREATE TABLE DLCs (
    DLCID INT IDENTITY(1,1) PRIMARY KEY,
    DLCName NVARCHAR(200),
    Price DECIMAL(10,2),
    AddedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    GameID UNIQUEIDENTIFIER,

    FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

-- TABLA DE GENEROS
CREATE TABLE Genre (
    GenreID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100)
);

--TABLA DE GENERO DE JUEGOS:
CREATE TABLE Game_Genre (
    GameGenreID INT IDENTITY(1,1) PRIMARY KEY,
    GameID UNIQUEIDENTIFIER,
    GenreID INT,

    CONSTRAINT UQ_GameGenre_Game_Genre UNIQUE (GameID, GenreID),
    FOREIGN KEY (GameID) REFERENCES Games(GameID),
    FOREIGN KEY (GenreID) REFERENCES Genre(GenreID)
);

-- TABLA DE OFERTAS:
CREATE TABLE Offers (
    OfferID INT IDENTITY(1,1) PRIMARY KEY,
    GameID UNIQUEIDENTIFIER,
    Discount INT,
    StartDate DATETIME2,
    EndDate DATETIME2,

    FOREIGN KEY (GameID) REFERENCES Games(GameID)
);

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Status' AND xtype='U')
BEGIN
    CREATE TABLE Status (
        StatusId INT NOT NULL PRIMARY KEY,
        Name NVARCHAR(50) NOT NULL
    );
END


--ACTUALIZACION 
USE [PLAYVERSE];
GO

SET NOCOUNT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    -------------------------------------------------------------------------
    -- 0. LOCALIZAR IDS DUPLICADOS DE ESTADOS EQUIVALENTES
    -------------------------------------------------------------------------
    DECLARE @ActiveDuplicateIds TABLE (StatusID INT PRIMARY KEY);
    DECLARE @InactiveDuplicateIds TABLE (StatusID INT PRIMARY KEY);

    INSERT INTO @ActiveDuplicateIds (StatusID)
    SELECT StatusID
    FROM dbo.Status
    WHERE StatusID <> 1
      AND (
            UPPER(ISNULL(Code, '')) = 'ACTIVE'
         OR UPPER(ISNULL(ShowName, '')) IN ('ACTIVO', 'ACTIVE')
      );

    INSERT INTO @InactiveDuplicateIds (StatusID)
    SELECT StatusID
    FROM dbo.Status
    WHERE StatusID <> 0
      AND (
            UPPER(ISNULL(Code, '')) = 'INACTIVE'
         OR UPPER(ISNULL(ShowName, '')) IN ('DESACTIVO', 'INACTIVE')
      );

    -------------------------------------------------------------------------
    -- 1. ASEGURAR STATUS BASE 0 Y 1
    -------------------------------------------------------------------------
    SET IDENTITY_INSERT dbo.Status ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Status WHERE StatusID = 0)
    BEGIN
        INSERT INTO dbo.Status (StatusID, Code, ShowName)
        VALUES (0, 'INACTIVE', 'Desactivo');
    END
    ELSE
    BEGIN
        UPDATE dbo.Status
        SET Code = 'INACTIVE',
            ShowName = 'Desactivo'
        WHERE StatusID = 0;
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.Status WHERE StatusID = 1)
    BEGIN
        INSERT INTO dbo.Status (StatusID, Code, ShowName)
        VALUES (1, 'ACTIVE', 'Activo');
    END
    ELSE
    BEGIN
        UPDATE dbo.Status
        SET Code = 'ACTIVE',
            ShowName = 'Activo'
        WHERE StatusID = 1;
    END

    SET IDENTITY_INSERT dbo.Status OFF;

    -------------------------------------------------------------------------
    -- 2. REASIGNAR USUARIOS QUE APUNTAN A IDS DUPLICADOS
    -------------------------------------------------------------------------
    UPDATE U
    SET U.StatusID = 1
    FROM dbo.Users U
    INNER JOIN @ActiveDuplicateIds D ON U.StatusID = D.StatusID;

    UPDATE U
    SET U.StatusID = 0
    FROM dbo.Users U
    INNER JOIN @InactiveDuplicateIds D ON U.StatusID = D.StatusID;

    -------------------------------------------------------------------------
    -- 3. NORMALIZAR USUARIOS SEGUN DeletedAt
    -------------------------------------------------------------------------
    UPDATE dbo.Users
    SET StatusID = 1
    WHERE DeletedAt IS NULL
      AND (StatusID IS NULL OR StatusID <> 1);

    UPDATE dbo.Users
    SET StatusID = 0
    WHERE DeletedAt IS NOT NULL
      AND (StatusID IS NULL OR StatusID <> 0);

    -------------------------------------------------------------------------
    -- 4. ELIMINAR ESTADOS DUPLICADOS YA SIN REFERENCIAS
    -------------------------------------------------------------------------
    DELETE S
    FROM dbo.Status S
    INNER JOIN @ActiveDuplicateIds D ON S.StatusID = D.StatusID
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.Users U
        WHERE U.StatusID = S.StatusID
    );

    DELETE S
    FROM dbo.Status S
    INNER JOIN @InactiveDuplicateIds D ON S.StatusID = D.StatusID
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.Users U
        WHERE U.StatusID = S.StatusID
    );

    -------------------------------------------------------------------------
    -- 5. ACTUALIZAR/CREAR EMAIL TEMPLATES
    -------------------------------------------------------------------------
    IF EXISTS (SELECT 1 FROM dbo.EmailTemplates WHERE Name = 'COLLABORATOR_REGISTER')
    BEGIN
        UPDATE dbo.EmailTemplates
        SET Name = 'USER_REGISTER',
            Subject = 'Bienvenido a PlayVerse',
            Body = 'Tu cuenta fue creada correctamente.'
        WHERE Name = 'COLLABORATOR_REGISTER';
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.EmailTemplates WHERE Name = 'USER_REGISTER')
    BEGIN
        INSERT INTO dbo.EmailTemplates (Name, Subject, Body)
        VALUES ('USER_REGISTER', 'Bienvenido a PlayVerse', 'Tu cuenta fue creada correctamente.');
    END
    ELSE
    BEGIN
        UPDATE dbo.EmailTemplates
        SET Subject = 'Bienvenido a PlayVerse',
            Body = 'Tu cuenta fue creada correctamente.'
        WHERE Name = 'USER_REGISTER';
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.EmailTemplates WHERE Name = 'AUTH_LOGIN_SUCCESS')
    BEGIN
        INSERT INTO dbo.EmailTemplates (Name, Subject, Body)
        VALUES ('AUTH_LOGIN_SUCCESS', 'Inicio de sesion correcto', 'Tu inicio de sesion fue exitoso el {{datetime}}.');
    END
    ELSE
    BEGIN
        UPDATE dbo.EmailTemplates
        SET Subject = 'Inicio de sesion correcto',
            Body = 'Tu inicio de sesion fue exitoso el {{datetime}}.'
        WHERE Name = 'AUTH_LOGIN_SUCCESS';
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.EmailTemplates WHERE Name = 'AUTH_LOGIN_FAILED')
    BEGIN
        INSERT INTO dbo.EmailTemplates (Name, Subject, Body)
        VALUES ('AUTH_LOGIN_FAILED', 'Intento de inicio de sesion fallido', 'Se detecto un intento fallido de inicio de sesion en tu cuenta.');
    END
    ELSE
    BEGIN
        UPDATE dbo.EmailTemplates
        SET Subject = 'Intento de inicio de sesion fallido',
            Body = 'Se detecto un intento fallido de inicio de sesion en tu cuenta.'
        WHERE Name = 'AUTH_LOGIN_FAILED';
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.EmailTemplates WHERE Name = 'AUTH_REGISTER_INIT')
    BEGIN
        INSERT INTO dbo.EmailTemplates (Name, Subject, Body)
        VALUES ('AUTH_REGISTER_INIT', 'Token de registro', 'Usa este token para completar tu registro: {{token}}');
    END
    ELSE
    BEGIN
        UPDATE dbo.EmailTemplates
        SET Subject = 'Token de registro',
            Body = 'Usa este token para completar tu registro: {{token}}'
        WHERE Name = 'AUTH_REGISTER_INIT';
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.EmailTemplates WHERE Name = 'AUTH_RECOVER_PASSWORD_OTP')
    BEGIN
        INSERT INTO dbo.EmailTemplates (Name, Subject, Body)
        VALUES ('AUTH_RECOVER_PASSWORD_OTP', 'Codigo de recuperacion', 'Tu codigo OTP para recuperar la contrasena es: {{code}}');
    END
    ELSE
    BEGIN
        UPDATE dbo.EmailTemplates
        SET Subject = 'Codigo de recuperacion',
            Body = 'Tu codigo OTP para recuperar la contrasena es: {{code}}'
        WHERE Name = 'AUTH_RECOVER_PASSWORD_OTP';
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.EmailTemplates WHERE Name = 'AUTH_PASSWORD_CHANGED')
    BEGIN
        INSERT INTO dbo.EmailTemplates (Name, Subject, Body)
        VALUES ('AUTH_PASSWORD_CHANGED', 'Contrasena actualizada', 'Tu contrasena fue actualizada correctamente.');
    END
    ELSE
    BEGIN
        UPDATE dbo.EmailTemplates
        SET Subject = 'Contrasena actualizada',
            Body = 'Tu contrasena fue actualizada correctamente.'
        WHERE Name = 'AUTH_PASSWORD_CHANGED';
    END

    COMMIT TRANSACTION;
    PRINT 'Actualizacion completada correctamente.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    BEGIN TRY
        SET IDENTITY_INSERT dbo.Status OFF;
    END TRY
    BEGIN CATCH
    END CATCH

    --THROW;
END CATCH;
GO

SELECT StatusID, Code, ShowName
FROM dbo.Status
ORDER BY StatusID;

SELECT UserId, Email, UserName, StatusID, DeletedAt
FROM dbo.Users
ORDER BY Email;

SELECT EmailTemplateId, Name, Subject, Body, CreatedAt
FROM dbo.EmailTemplates
ORDER BY Name;
GO

-- ============================================================
--  ROLES Y PERMISOS PARA PLAYVERSE
-- ============================================================

IF OBJECT_ID('dbo.Permissions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Permissions (
        Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        Code NVARCHAR(100) NOT NULL,
        Module NVARCHAR(50) NOT NULL,
        Action NVARCHAR(50) NOT NULL,
        Name NVARCHAR(150) NOT NULL,
        Description NVARCHAR(500) NULL,
        Specificity NVARCHAR(20) NOT NULL DEFAULT 'ByAssignment',
        IsActive BIT NOT NULL DEFAULT 1,
        CONSTRAINT PK_Permissions PRIMARY KEY (Id),
        CONSTRAINT UQ_Permissions_Code UNIQUE (Code),
        CONSTRAINT CK_Permissions_Specificity CHECK (Specificity IN ('Own', 'ByAssignment'))
    );
END
GO

IF OBJECT_ID('dbo.Roles', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Roles (
        Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500) NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        CONSTRAINT PK_Roles PRIMARY KEY (Id),
        CONSTRAINT UQ_Roles_Name UNIQUE (Name)
    );
END
GO

IF OBJECT_ID('dbo.RolePermissions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.RolePermissions (
        RoleId UNIQUEIDENTIFIER NOT NULL,
        PermissionId UNIQUEIDENTIFIER NOT NULL,
        AssignedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        CONSTRAINT PK_RolePermissions PRIMARY KEY (RoleId, PermissionId),
        CONSTRAINT FK_RolePermissions_Role FOREIGN KEY (RoleId)
            REFERENCES dbo.Roles (Id) ON DELETE CASCADE,
        CONSTRAINT FK_RolePermissions_Permission FOREIGN KEY (PermissionId)
            REFERENCES dbo.Permissions (Id) ON DELETE CASCADE
    );
END
GO

IF OBJECT_ID('dbo.UserRoles', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.UserRoles (
        UserId UNIQUEIDENTIFIER NOT NULL,
        RoleId UNIQUEIDENTIFIER NOT NULL,
        AssignedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        AssignedBy UNIQUEIDENTIFIER NULL,
        CONSTRAINT PK_UserRoles PRIMARY KEY (UserId, RoleId),
        CONSTRAINT FK_UserRoles_User FOREIGN KEY (UserId)
            REFERENCES dbo.Users (UserID) ON DELETE CASCADE,
        CONSTRAINT FK_UserRoles_Role FOREIGN KEY (RoleId)
            REFERENCES dbo.Roles (Id) ON DELETE CASCADE,
        CONSTRAINT FK_UserRoles_AssignedBy FOREIGN KEY (AssignedBy)
            REFERENCES dbo.Users (UserID)
    );
END
GO

MERGE dbo.Permissions AS target
USING (
    VALUES
        ('USERS_READ', 'USERS', 'READ', 'Ver usuarios', 'Permite consultar usuarios del sistema', 'ByAssignment', 1),
        ('USERS_MANAGE', 'USERS', 'MANAGE', 'Gestionar usuarios', 'Permite administrar usuarios y desactivarlos', 'ByAssignment', 1),
        ('ROLES_READ', 'ROLES', 'READ', 'Ver roles', 'Permite consultar roles y permisos', 'ByAssignment', 1),
        ('ROLES_ASSIGN', 'ROLES', 'ASSIGN', 'Asignar roles', 'Permite asignar y quitar roles', 'ByAssignment', 1),
        ('CATALOG_MANAGE', 'CATALOG', 'MANAGE', 'Gestionar catalogo', 'Permite administrar developers, publishers y generos', 'ByAssignment', 1),
        ('GAMES_MANAGE', 'GAMES', 'MANAGE', 'Gestionar juegos', 'Permite crear y editar juegos, DLCs, logros y ofertas', 'ByAssignment', 1),
        ('WISHLIST_MANAGE', 'WISHLIST', 'MANAGE', 'Gestionar wishlist', 'Permite gestionar la wishlist personal', 'Own', 1),
        ('LIBRARY_PURCHASE', 'LIBRARY', 'PURCHASE', 'Comprar juegos', 'Permite comprar juegos y consultar la biblioteca personal', 'Own', 1),
        ('REVIEWS_MANAGE', 'REVIEWS', 'MANAGE', 'Gestionar reseñas', 'Permite crear, editar y responder reseñas', 'Own', 1),
        ('FRIENDS_MANAGE', 'FRIENDS', 'MANAGE', 'Gestionar amigos', 'Permite administrar amistades', 'Own', 1),
        ('SESSIONS_PLAY', 'SESSIONS', 'PLAY', 'Gestionar sesiones', 'Permite iniciar y cerrar sesiones de juego', 'Own', 1),
        ('ACHIEVEMENTS_UNLOCK', 'ACHIEVEMENTS', 'UNLOCK', 'Desbloquear logros', 'Permite desbloquear logros de juegos propios', 'Own', 1)
) AS source (Code, Module, Action, Name, Description, Specificity, IsActive)
ON target.Code = source.Code
WHEN MATCHED THEN
    UPDATE SET
        Module = source.Module,
        Action = source.Action,
        Name = source.Name,
        Description = source.Description,
        Specificity = source.Specificity,
        IsActive = source.IsActive
WHEN NOT MATCHED THEN
    INSERT (Id, Code, Module, Action, Name, Description, Specificity, IsActive)
    VALUES (NEWID(), source.Code, source.Module, source.Action, source.Name, source.Description, source.Specificity, source.IsActive);
GO

MERGE dbo.Roles AS target
USING (
    VALUES
        ('admin', 'Acceso total al sistema y gestion de roles, usuarios y catalogo', 1),
        ('developer', 'Gestiona catalogo y juegos, y puede usar los flujos propios de usuario', 1),
        ('user', 'Rol base para jugar, comprar, reseñar y gestionar su cuenta', 1)
) AS source (Name, Description, IsActive)
ON target.Name = source.Name
WHEN MATCHED THEN
    UPDATE SET
        Description = source.Description,
        IsActive = source.IsActive,
        UpdatedAt = SYSUTCDATETIME()
WHEN NOT MATCHED THEN
    INSERT (Id, Name, Description, IsActive, CreatedAt, UpdatedAt)
    VALUES (NEWID(), source.Name, source.Description, source.IsActive, SYSUTCDATETIME(), SYSUTCDATETIME());
GO

DELETE rp
FROM dbo.RolePermissions rp
INNER JOIN dbo.Roles r ON r.Id = rp.RoleId
WHERE r.Name IN ('admin', 'developer', 'user');
GO

INSERT INTO dbo.RolePermissions (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM dbo.Roles r
CROSS JOIN dbo.Permissions p
WHERE r.Name = 'admin';
GO

INSERT INTO dbo.RolePermissions (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM dbo.Roles r
INNER JOIN dbo.Permissions p ON p.Code IN (
    'USERS_READ',
    'CATALOG_MANAGE',
    'GAMES_MANAGE',
    'WISHLIST_MANAGE',
    'LIBRARY_PURCHASE',
    'REVIEWS_MANAGE',
    'FRIENDS_MANAGE',
    'SESSIONS_PLAY',
    'ACHIEVEMENTS_UNLOCK'
)
WHERE r.Name = 'developer';
GO

INSERT INTO dbo.RolePermissions (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM dbo.Roles r
INNER JOIN dbo.Permissions p ON p.Code IN (
    'WISHLIST_MANAGE',
    'LIBRARY_PURCHASE',
    'REVIEWS_MANAGE',
    'FRIENDS_MANAGE',
    'SESSIONS_PLAY',
    'ACHIEVEMENTS_UNLOCK'
)
WHERE r.Name = 'user';
GO


--ACT
USE [PLAYVERSE];
GO

IF DB_NAME() <> 'PLAYVERSE'
BEGIN
    THROW 50001, 'Debes ejecutar este script dentro de la base PLAYVERSE.', 1;
END
GO

SET NOCOUNT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    -------------------------------------------------------------------------
    -- 0. VALIDACION BASE
    -------------------------------------------------------------------------
    IF OBJECT_ID('dbo.Users', 'U') IS NULL
    BEGIN
        THROW 50002, 'La tabla dbo.Users no existe en la base PLAYVERSE.', 1;
    END

    -------------------------------------------------------------------------
    -- 1. ELIMINAR TABLAS DE ROLES/PERMISOS SI YA EXISTEN
    -------------------------------------------------------------------------
    IF OBJECT_ID('dbo.UserRoles', 'U') IS NOT NULL
        DROP TABLE dbo.UserRoles;

    IF OBJECT_ID('dbo.RolePermissions', 'U') IS NOT NULL
        DROP TABLE dbo.RolePermissions;

    IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL
        DROP TABLE dbo.Roles;

    IF OBJECT_ID('dbo.Permissions', 'U') IS NOT NULL
        DROP TABLE dbo.Permissions;

    -------------------------------------------------------------------------
    -- 2. CREAR TABLA DE PERMISOS
    -------------------------------------------------------------------------
    CREATE TABLE dbo.Permissions (
        Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        Code NVARCHAR(100) NOT NULL,
        Module NVARCHAR(50) NOT NULL,
        Action NVARCHAR(50) NOT NULL,
        Name NVARCHAR(150) NOT NULL,
        Description NVARCHAR(500) NULL,
        Specificity NVARCHAR(20) NOT NULL DEFAULT 'ByAssignment',
        IsActive BIT NOT NULL DEFAULT 1,

        CONSTRAINT PK_Permissions PRIMARY KEY (Id),
        CONSTRAINT UQ_Permissions_Code UNIQUE (Code),
        CONSTRAINT CK_Permissions_Specificity CHECK (Specificity IN ('Own', 'ByAssignment'))
    );

    -------------------------------------------------------------------------
    -- 3. CREAR TABLA DE ROLES
    -------------------------------------------------------------------------
    CREATE TABLE dbo.Roles (
        Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500) NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

        CONSTRAINT PK_Roles PRIMARY KEY (Id),
        CONSTRAINT UQ_Roles_Name UNIQUE (Name)
    );

    -------------------------------------------------------------------------
    -- 4. CREAR RELACION ROLE -> PERMISSION
    -------------------------------------------------------------------------
    CREATE TABLE dbo.RolePermissions (
        RoleId UNIQUEIDENTIFIER NOT NULL,
        PermissionId UNIQUEIDENTIFIER NOT NULL,
        AssignedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

        CONSTRAINT PK_RolePermissions PRIMARY KEY (RoleId, PermissionId),
        CONSTRAINT FK_RolePermissions_Role FOREIGN KEY (RoleId)
            REFERENCES dbo.Roles (Id) ON DELETE CASCADE,
        CONSTRAINT FK_RolePermissions_Permission FOREIGN KEY (PermissionId)
            REFERENCES dbo.Permissions (Id) ON DELETE CASCADE
    );

    -------------------------------------------------------------------------
    -- 5. CREAR RELACION USER -> ROLE
    -------------------------------------------------------------------------
    CREATE TABLE dbo.UserRoles (
        UserId UNIQUEIDENTIFIER NOT NULL,
        RoleId UNIQUEIDENTIFIER NOT NULL,
        AssignedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        AssignedBy UNIQUEIDENTIFIER NULL,

        CONSTRAINT PK_UserRoles PRIMARY KEY (UserId, RoleId),
        CONSTRAINT FK_UserRoles_User FOREIGN KEY (UserId)
            REFERENCES dbo.Users (UserID) ON DELETE CASCADE,
        CONSTRAINT FK_UserRoles_Role FOREIGN KEY (RoleId)
            REFERENCES dbo.Roles (Id) ON DELETE CASCADE,
        CONSTRAINT FK_UserRoles_AssignedBy FOREIGN KEY (AssignedBy)
            REFERENCES dbo.Users (UserID)
    );

    -------------------------------------------------------------------------
    -- 6. INSERTAR PERMISOS
    -------------------------------------------------------------------------
    INSERT INTO dbo.Permissions (Id, Code, Module, Action, Name, Description, Specificity, IsActive)
    VALUES
        (NEWID(), 'USERS_READ', 'USERS', 'READ', 'Ver usuarios', 'Permite consultar usuarios del sistema', 'ByAssignment', 1),
        (NEWID(), 'USERS_MANAGE', 'USERS', 'MANAGE', 'Gestionar usuarios', 'Permite administrar usuarios y desactivarlos', 'ByAssignment', 1),
        (NEWID(), 'ROLES_READ', 'ROLES', 'READ', 'Ver roles', 'Permite consultar roles y permisos', 'ByAssignment', 1),
        (NEWID(), 'ROLES_ASSIGN', 'ROLES', 'ASSIGN', 'Asignar roles', 'Permite asignar y quitar roles', 'ByAssignment', 1),
        (NEWID(), 'CATALOG_MANAGE', 'CATALOG', 'MANAGE', 'Gestionar catalogo', 'Permite administrar developers, publishers y generos', 'ByAssignment', 1),
        (NEWID(), 'GAMES_MANAGE', 'GAMES', 'MANAGE', 'Gestionar juegos', 'Permite crear y editar juegos, DLCs, logros y ofertas', 'ByAssignment', 1),
        (NEWID(), 'WISHLIST_MANAGE', 'WISHLIST', 'MANAGE', 'Gestionar wishlist', 'Permite gestionar la wishlist personal', 'Own', 1),
        (NEWID(), 'LIBRARY_PURCHASE', 'LIBRARY', 'PURCHASE', 'Comprar juegos', 'Permite comprar juegos y consultar la biblioteca personal', 'Own', 1),
        (NEWID(), 'REVIEWS_MANAGE', 'REVIEWS', 'MANAGE', 'Gestionar reseñas', 'Permite crear, editar y responder reseñas', 'Own', 1),
        (NEWID(), 'FRIENDS_MANAGE', 'FRIENDS', 'MANAGE', 'Gestionar amigos', 'Permite administrar amistades', 'Own', 1),
        (NEWID(), 'SESSIONS_PLAY', 'SESSIONS', 'PLAY', 'Gestionar sesiones', 'Permite iniciar y cerrar sesiones de juego', 'Own', 1),
        (NEWID(), 'ACHIEVEMENTS_UNLOCK', 'ACHIEVEMENTS', 'UNLOCK', 'Desbloquear logros', 'Permite desbloquear logros de juegos propios', 'Own', 1);

    -------------------------------------------------------------------------
    -- 7. INSERTAR ROLES
    -------------------------------------------------------------------------
    DECLARE @AdminRoleId UNIQUEIDENTIFIER = NEWID();
    DECLARE @DeveloperRoleId UNIQUEIDENTIFIER = NEWID();
    DECLARE @UserRoleId UNIQUEIDENTIFIER = NEWID();

    INSERT INTO dbo.Roles (Id, Name, Description, IsActive, CreatedAt, UpdatedAt)
    VALUES
        (@AdminRoleId, 'admin', 'Acceso total al sistema', 1, SYSUTCDATETIME(), SYSUTCDATETIME()),
        (@DeveloperRoleId, 'developer', 'Gestiona catalogo y juegos, y puede usar flujos de usuario', 1, SYSUTCDATETIME(), SYSUTCDATETIME()),
        (@UserRoleId, 'user', 'Rol base para jugar, comprar, reseñar y gestionar su cuenta', 1, SYSUTCDATETIME(), SYSUTCDATETIME());

    -------------------------------------------------------------------------
    -- 8. ASIGNAR PERMISOS A ADMIN
    -------------------------------------------------------------------------
    INSERT INTO dbo.RolePermissions (RoleId, PermissionId)
    SELECT @AdminRoleId, p.Id
    FROM dbo.Permissions p;

    -------------------------------------------------------------------------
    -- 9. ASIGNAR PERMISOS A DEVELOPER
    -------------------------------------------------------------------------
    INSERT INTO dbo.RolePermissions (RoleId, PermissionId)
    SELECT @DeveloperRoleId, p.Id
    FROM dbo.Permissions p
    WHERE p.Code IN (
        'USERS_READ',
        'CATALOG_MANAGE',
        'GAMES_MANAGE',
        'WISHLIST_MANAGE',
        'LIBRARY_PURCHASE',
        'REVIEWS_MANAGE',
        'FRIENDS_MANAGE',
        'SESSIONS_PLAY',
        'ACHIEVEMENTS_UNLOCK'
    );

    -------------------------------------------------------------------------
    -- 10. ASIGNAR PERMISOS A USER
    -------------------------------------------------------------------------
    INSERT INTO dbo.RolePermissions (RoleId, PermissionId)
    SELECT @UserRoleId, p.Id
    FROM dbo.Permissions p
    WHERE p.Code IN (
        'WISHLIST_MANAGE',
        'LIBRARY_PURCHASE',
        'REVIEWS_MANAGE',
        'FRIENDS_MANAGE',
        'SESSIONS_PLAY',
        'ACHIEVEMENTS_UNLOCK'
    );

    -------------------------------------------------------------------------
    -- 11. ASIGNAR ROL USER A TODOS LOS USUARIOS EXISTENTES
    -------------------------------------------------------------------------
    INSERT INTO dbo.UserRoles (UserId, RoleId, AssignedAt, AssignedBy)
    SELECT u.UserID, @UserRoleId, SYSUTCDATETIME(), NULL
    FROM dbo.Users u;

    COMMIT TRANSACTION;
    PRINT 'Roles y permisos recreados correctamente.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO

---------------------------------------------------------------------------
-- 12. VERIFICACION
---------------------------------------------------------------------------
SELECT Id, Name, Description, IsActive
FROM dbo.Roles
ORDER BY Name;

SELECT Id, Code, Module, Action, Specificity, IsActive
FROM dbo.Permissions
ORDER BY Code;

SELECT r.Name AS RoleName, p.Code AS PermissionCode
FROM dbo.RolePermissions rp
INNER JOIN dbo.Roles r ON r.Id = rp.RoleId
INNER JOIN dbo.Permissions p ON p.Id = rp.PermissionId
ORDER BY r.Name, p.Code;

SELECT u.Email, r.Name AS RoleName, ur.AssignedAt
FROM dbo.UserRoles ur
INNER JOIN dbo.Users u ON u.UserID = ur.UserId
INNER JOIN dbo.Roles r ON r.Id = ur.RoleId
ORDER BY u.Email, r.Name;
GO

-- ============================================================
--  ACTUALIZACION FINAL FRONT/BACKEND - 2026-05-29
--  Imagen de juegos, compras no duplicadas y root admin.
-- ============================================================
USE [PLAYVERSE];
GO

SET NOCOUNT ON;
GO

IF COL_LENGTH('dbo.Games', 'ImageUrl') IS NULL
BEGIN
    ALTER TABLE dbo.Games ADD ImageUrl NVARCHAR(600) NULL;
END
GO

;WITH duplicated AS (
    SELECT
        LibraryID,
        ROW_NUMBER() OVER (
            PARTITION BY UserID, GameID
            ORDER BY PurchaseDate DESC, LibraryID DESC
        ) AS rn
    FROM dbo.Library
    WHERE UserID IS NOT NULL
      AND GameID IS NOT NULL
)
DELETE FROM duplicated
WHERE rn > 1;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'UQ_Library_User_Game'
      AND object_id = OBJECT_ID('dbo.Library')
)
BEGIN
    CREATE UNIQUE INDEX UQ_Library_User_Game
    ON dbo.Library (UserID, GameID)
    WHERE UserID IS NOT NULL AND GameID IS NOT NULL;
END
GO

;WITH duplicated AS (
    SELECT
        WishlistID,
        ROW_NUMBER() OVER (
            PARTITION BY UserID, GameID
            ORDER BY AddedAt DESC, WishlistID DESC
        ) AS rn
    FROM dbo.Wishlist
    WHERE UserID IS NOT NULL
      AND GameID IS NOT NULL
)
DELETE FROM duplicated
WHERE rn > 1;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'UQ_Wishlist_User_Game'
      AND object_id = OBJECT_ID('dbo.Wishlist')
)
BEGIN
    CREATE UNIQUE INDEX UQ_Wishlist_User_Game
    ON dbo.Wishlist (UserID, GameID)
    WHERE UserID IS NOT NULL AND GameID IS NOT NULL;
END
GO

;WITH duplicated AS (
    SELECT
        GameGenreID,
        ROW_NUMBER() OVER (
            PARTITION BY GameID, GenreID
            ORDER BY GameGenreID DESC
        ) AS rn
    FROM dbo.Game_Genre
    WHERE GameID IS NOT NULL
      AND GenreID IS NOT NULL
)
DELETE FROM duplicated
WHERE rn > 1;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'UQ_GameGenre_Game_Genre'
      AND object_id = OBJECT_ID('dbo.Game_Genre')
)
BEGIN
    CREATE UNIQUE INDEX UQ_GameGenre_Game_Genre
    ON dbo.Game_Genre (GameID, GenreID)
    WHERE GameID IS NOT NULL AND GenreID IS NOT NULL;
END
GO

DECLARE @RootEmail NVARCHAR(150) = 'changeme@userroot.com';
DECLARE @RootUserName NVARCHAR(100) = 'User Root';
DECLARE @RootPasswordHash NVARCHAR(255) = 'agRkCE7bm3ED9YJOttvYuXcOCYkgSFJkClUw05p8a4o=;PlayVerseRootSalt2026';
DECLARE @RootUserId UNIQUEIDENTIFIER;
DECLARE @AdminRoleId UNIQUEIDENTIFIER;
DECLARE @UserRoleId UNIQUEIDENTIFIER;

SELECT @RootUserId = UserID
FROM dbo.Users
WHERE Email = @RootEmail;

IF @RootUserId IS NULL
BEGIN
    SET @RootUserId = NEWID();

    INSERT INTO dbo.Users (UserID, UserName, Email, Password, CreatedAt, DeletedAt, StatusID)
    VALUES (@RootUserId, @RootUserName, @RootEmail, @RootPasswordHash, SYSUTCDATETIME(), NULL, 1);
END
ELSE
BEGIN
    UPDATE dbo.Users
    SET UserName = COALESCE(NULLIF(UserName, ''), @RootUserName),
        DeletedAt = NULL,
        StatusID = 1
    WHERE UserID = @RootUserId;
END

SELECT @AdminRoleId = Id
FROM dbo.Roles
WHERE Name = 'admin'
  AND IsActive = 1;

SELECT @UserRoleId = Id
FROM dbo.Roles
WHERE Name = 'user'
  AND IsActive = 1;

IF @RootUserId IS NOT NULL
   AND @AdminRoleId IS NOT NULL
   AND NOT EXISTS (
       SELECT 1
       FROM dbo.UserRoles
       WHERE UserId = @RootUserId
         AND RoleId = @AdminRoleId
   )
BEGIN
    INSERT INTO dbo.UserRoles (UserId, RoleId, AssignedAt, AssignedBy)
    VALUES (@RootUserId, @AdminRoleId, SYSUTCDATETIME(), NULL);
END

IF @RootUserId IS NOT NULL
   AND @UserRoleId IS NOT NULL
BEGIN
    DELETE FROM dbo.UserRoles
    WHERE UserId = @RootUserId
      AND RoleId = @UserRoleId;
END
GO
