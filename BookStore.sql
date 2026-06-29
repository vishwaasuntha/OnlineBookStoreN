CREATE DATABASE OnlineBookStoreDB;
USE OnlineBookStoreDB;

CREATE TABLE Categories (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100)
);

CREATE TABLE Books (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(200),
    Author VARCHAR(100),
    Price DECIMAL(10,2),
    ImagePath VARCHAR(255),
    CategoryId INT
);

CREATE TABLE Orders (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId VARCHAR(100),
    OrderDate DATETIME,
    TotalAmount DECIMAL(10,2)
);

CREATE TABLE OrderItems (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    OrderId INT,
    BookId INT,
    Quantity INT,
    Price DECIMAL(10,2)
);