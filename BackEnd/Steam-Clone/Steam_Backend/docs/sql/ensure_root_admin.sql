USE [PLAYVERSE];
GO

DECLARE @Email NVARCHAR(150) = 'changeme@userroot.com';
DECLARE @UserName NVARCHAR(100) = 'User Root';
DECLARE @PasswordHash NVARCHAR(255) = 'agRkCE7bm3ED9YJOttvYuXcOCYkgSFJkClUw05p8a4o=;PlayVerseRootSalt2026';
DECLARE @UserId UNIQUEIDENTIFIER;
DECLARE @AdminRoleId UNIQUEIDENTIFIER;
DECLARE @UserRoleId UNIQUEIDENTIFIER;

SELECT @UserId = UserID
FROM dbo.Users
WHERE Email = @Email;

SELECT @AdminRoleId = Id
FROM dbo.Roles
WHERE Name = 'admin'
  AND IsActive = 1;

SELECT @UserRoleId = Id
FROM dbo.Roles
WHERE Name = 'user'
  AND IsActive = 1;

IF @UserId IS NULL
BEGIN
    SET @UserId = NEWID();

    INSERT INTO dbo.Users (UserID, UserName, Email, Password, CreatedAt, DeletedAt, StatusID)
    VALUES (@UserId, @UserName, @Email, @PasswordHash, SYSUTCDATETIME(), NULL, 1);
END
ELSE
BEGIN
    UPDATE dbo.Users
    SET UserName = COALESCE(NULLIF(UserName, ''), @UserName),
        DeletedAt = NULL,
        StatusID = 1
    WHERE UserID = @UserId;
END

IF @AdminRoleId IS NULL
BEGIN
    RAISERROR('No existe el rol admin activo.', 16, 1);
END;

IF @UserId IS NOT NULL
   AND @AdminRoleId IS NOT NULL
   AND NOT EXISTS (
       SELECT 1
       FROM dbo.UserRoles
       WHERE UserId = @UserId
         AND RoleId = @AdminRoleId
   )
BEGIN
    INSERT INTO dbo.UserRoles (UserId, RoleId, AssignedAt, AssignedBy)
    VALUES (@UserId, @AdminRoleId, SYSUTCDATETIME(), NULL);
END;

IF @UserId IS NOT NULL
   AND @UserRoleId IS NOT NULL
BEGIN
    DELETE FROM dbo.UserRoles
    WHERE UserId = @UserId
      AND RoleId = @UserRoleId;
END;
GO
