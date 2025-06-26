CREATE PROCEDURE GetUserByEmail
    @Email NVARCHAR(256)
AS
BEGIN
    SELECT * FROM Users WHERE Email = @Email;
END
GO

CREATE PROCEDURE GetUserById
    @Id NVARCHAR(50)
AS
BEGIN
    SELECT * FROM Users WHERE Id = @Id;
END
GO

CREATE PROCEDURE AddUser
    @Id NVARCHAR(50),
    @UserName NVARCHAR(100),
    @Email NVARCHAR(256),
    @PasswordHash NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Users (Id, UserName, Email, PasswordHash)
    VALUES (@Id, @UserName, @Email, @PasswordHash);
END
GO

CREATE PROCEDURE UpdateUser
    @Id NVARCHAR(50),
    @UserName NVARCHAR(100),
    @Email NVARCHAR(256),
    @PasswordHash NVARCHAR(MAX)
AS
BEGIN
    UPDATE Users
    SET UserName = @UserName,
        Email = @Email,
        PasswordHash = @PasswordHash
    WHERE Id = @Id;
END
GO

CREATE PROCEDURE DeleteUser
    @Id NVARCHAR(50)
AS
BEGIN
    DELETE FROM Users WHERE Id = @Id;
END
GO


CREATE PROCEDURE GetAllUsers
AS
BEGIN
    SELECT     
    Id ,
    UserName ,
    Email ,
    PasswordHash  FROM Users;
END
GO
