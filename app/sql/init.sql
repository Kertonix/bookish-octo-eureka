
USE master
GO

DROP DATABASE IF EXISTS app;
CREATE DATABASE app
GO

USE app
GO

DROP TABLE IF EXISTS Posts;
CREATE TABLE Posts
(
    Id INT PRIMARY KEY IDENTITY,
    Title VARCHAR(max),
    Description VARCHAR(max),
    ImageUrl VARCHAR(max),
    Created DATETIME,
    Updated DATETIME
)
GO

SELECT *
FROM Posts; 