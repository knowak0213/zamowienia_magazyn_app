USE [zm_app_db];
GO
SET QUOTED_IDENTIFIER ON;
GO
DECLARE @Email NVARCHAR(256) = 'karol@wp.pl';
DECLARE @UserId NVARCHAR(450);
DECLARE @ClientId INT;
SELECT @UserId = Id FROM AspNetUsers WHERE Email = @Email;
SELECT @ClientId = Id FROM Clients WHERE Email = @Email OR UserId = @UserId;
IF @ClientId IS NOT NULL OR @UserId IS NOT NULL
BEGIN
    DELETE FROM OrderItems WHERE OrderId IN (SELECT Id FROM Orders WHERE ClientId = @ClientId OR UserId = @UserId);
    DELETE FROM Orders WHERE ClientId = @ClientId OR UserId = @UserId;
END
DELETE FROM Clients WHERE Email = @Email OR UserId = @UserId;
IF @UserId IS NOT NULL
BEGIN
    DELETE FROM AspNetUserRoles WHERE UserId = @UserId;
    DELETE FROM AspNetUsers WHERE Id = @UserId;
END
PRINT 'Cleanup complete for ' + @Email;
GO
