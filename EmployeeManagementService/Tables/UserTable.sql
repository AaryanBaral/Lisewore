CREATE TABLE Users (
    Id NVARCHAR(50) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL ,
    Email NVARCHAR(256) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL
);
