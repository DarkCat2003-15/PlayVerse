USE [PLAYVERSE];
GO

SET NOCOUNT ON;
GO

DECLARE @DeveloperId UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';
DECLARE @PublisherId UNIQUEIDENTIFIER = '22222222-2222-2222-2222-222222222222';
DECLARE @GameId UNIQUEIDENTIFIER = '33333333-3333-3333-3333-333333333333';

IF NOT EXISTS (SELECT 1 FROM dbo.Developer WHERE DeveloperID = @DeveloperId)
BEGIN
    INSERT INTO dbo.Developer (DeveloperID, DeveloperName, Email, Password, CreatedAt, DeletedAt)
    VALUES (@DeveloperId, 'Codex Studio', 'studio@codex.local', NULL, SYSUTCDATETIME(), NULL);
END
ELSE
BEGIN
    UPDATE dbo.Developer
    SET DeveloperName = 'Codex Studio',
        Email = 'studio@codex.local',
        DeletedAt = NULL
    WHERE DeveloperID = @DeveloperId;
END

IF NOT EXISTS (SELECT 1 FROM dbo.Publishers WHERE PublisherID = @PublisherId)
BEGIN
    INSERT INTO dbo.Publishers (PublisherID, PublisherName, CreatedAt, DeletedAt)
    VALUES (@PublisherId, 'Codex Publishing', SYSUTCDATETIME(), NULL);
END
ELSE
BEGIN
    UPDATE dbo.Publishers
    SET PublisherName = 'Codex Publishing',
        DeletedAt = NULL
    WHERE PublisherID = @PublisherId;
END

IF NOT EXISTS (SELECT 1 FROM dbo.Genre WHERE Name = 'Action')
    INSERT INTO dbo.Genre (Name) VALUES ('Action');

IF NOT EXISTS (SELECT 1 FROM dbo.Genre WHERE Name = 'Indie')
    INSERT INTO dbo.Genre (Name) VALUES ('Indie');

IF NOT EXISTS (SELECT 1 FROM dbo.Games WHERE GameID = @GameId)
BEGIN
    INSERT INTO dbo.Games (GameID, Title, Description, ImageUrl, ReleaseDate, Price, DeveloperID, PublisherID, DeletedAt)
    VALUES (
        @GameId,
        'Codex Arena',
        'Juego demo para probar la API de PlayVerse',
        'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/1245620/header.jpg',
        '2026-05-01',
        29.99,
        @DeveloperId,
        @PublisherId,
        NULL
    );
END
ELSE
BEGIN
    UPDATE dbo.Games
    SET Title = 'Codex Arena',
        Description = 'Juego demo para probar la API de PlayVerse',
        ImageUrl = 'https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/1245620/header.jpg',
        ReleaseDate = '2026-05-01',
        Price = 29.99,
        DeveloperID = @DeveloperId,
        PublisherID = @PublisherId,
        DeletedAt = NULL
    WHERE GameID = @GameId;
END

DECLARE @ActionGenreId INT = (SELECT TOP 1 GenreID FROM dbo.Genre WHERE Name = 'Action');
DECLARE @IndieGenreId INT = (SELECT TOP 1 GenreID FROM dbo.Genre WHERE Name = 'Indie');

IF NOT EXISTS (SELECT 1 FROM dbo.Game_Genre WHERE GameID = @GameId AND GenreID = @ActionGenreId)
    INSERT INTO dbo.Game_Genre (GameID, GenreID) VALUES (@GameId, @ActionGenreId);

IF NOT EXISTS (SELECT 1 FROM dbo.Game_Genre WHERE GameID = @GameId AND GenreID = @IndieGenreId)
    INSERT INTO dbo.Game_Genre (GameID, GenreID) VALUES (@GameId, @IndieGenreId);

IF NOT EXISTS (SELECT 1 FROM dbo.Achievements WHERE GameID = @GameId AND Name = 'Primer paso')
BEGIN
    INSERT INTO dbo.Achievements (Name, Description, GameID)
    VALUES ('Primer paso', 'Inicia tu primera partida', @GameId);
END

IF NOT EXISTS (SELECT 1 FROM dbo.DLCs WHERE GameID = @GameId AND DLCName = 'Supporter Pack')
BEGIN
    INSERT INTO dbo.DLCs (DLCName, Price, AddedAt, GameID)
    VALUES ('Supporter Pack', 4.99, SYSUTCDATETIME(), @GameId);
END

IF NOT EXISTS (
    SELECT 1
    FROM dbo.Offers
    WHERE GameID = @GameId
      AND Discount = 25
      AND StartDate = '2026-05-01T00:00:00'
      AND EndDate = '2027-05-01T00:00:00'
)
BEGIN
    INSERT INTO dbo.Offers (GameID, Discount, StartDate, EndDate)
    VALUES (@GameId, 25, '2026-05-01T00:00:00', '2027-05-01T00:00:00');
END
GO

SELECT TOP (10) DeveloperID, DeveloperName, Email
FROM dbo.Developer
ORDER BY DeveloperName;

SELECT TOP (10) PublisherID, PublisherName
FROM dbo.Publishers
ORDER BY PublisherName;

SELECT TOP (10) GameID, Title, Price
FROM dbo.Games
ORDER BY Title;
GO
