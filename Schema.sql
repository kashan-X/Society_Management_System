USE master;
GO

IF DB_ID('SocietiesDB') IS NOT NULL
BEGIN
    ALTER DATABASE SocietiesDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SocietiesDB;
END
GO

CREATE DATABASE SocietiesDB;
GO

USE SocietiesDB;
GO

-- Users (Students, Society Heads, Admins)
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Role NVARCHAR(50) DEFAULT 'Student' -- 'Student', 'SocietyHead', 'Admin'
);
GO

-- Societies
CREATE TABLE Societies (
    SocietyId INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Status NVARCHAR(50) DEFAULT 'Pending', -- 'Pending', 'Approved', 'Suspended'
    HeadId INT NULL FOREIGN KEY REFERENCES Users(UserId)
);
GO

-- Memberships
CREATE TABLE Memberships (
    MembershipId INT PRIMARY KEY IDENTITY,
    StudentId INT FOREIGN KEY REFERENCES Users(UserId),
    SocietyId INT FOREIGN KEY REFERENCES Societies(SocietyId),
    Status NVARCHAR(50) DEFAULT 'Pending' -- 'Pending', 'Approved', 'Rejected'
);
GO

-- Events
CREATE TABLE Events (
    EventId INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Date DATETIME NOT NULL,
    SocietyId INT FOREIGN KEY REFERENCES Societies(SocietyId),
    Status NVARCHAR(50) DEFAULT 'Pending', -- 'Pending', 'Approved', 'Cancelled'
    MaxTickets INT DEFAULT 0
);
GO

-- Event Registrations (Tickets)
CREATE TABLE EventRegistrations (
    RegistrationId INT PRIMARY KEY IDENTITY,
    StudentId INT FOREIGN KEY REFERENCES Users(UserId),
    EventId INT FOREIGN KEY REFERENCES Events(EventId),
    Status NVARCHAR(50) DEFAULT 'Pending' -- 'Pending', 'Approved', 'Rejected'
);
GO

-- Tasks (for Society Members)
CREATE TABLE Tasks (
    TaskId INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    AssignedTo INT FOREIGN KEY REFERENCES Users(UserId),
    SocietyId INT FOREIGN KEY REFERENCES Societies(SocietyId),
    Status NVARCHAR(50) DEFAULT 'Pending' -- 'Pending', 'Completed'
);
GO

-- Insert Initial Data
INSERT INTO Users (Name, Email, Password, Role) VALUES 
('Admin User', 'admin@fast.edu', 'admin123', 'Admin'),
('John Head', 'john@fast.edu', 'head123', 'SocietyHead'),
('Alice Student', 'alice@fast.edu', 'student123', 'Student');

INSERT INTO Societies (Name, Description, Status, HeadId) VALUES
('Gaming Society', 'All about gaming', 'Approved', 2),
('Developers Club', 'Coding and tech', 'Approved', NULL),
('Media Society', 'Photography & video', 'Approved', NULL),
('Sports Society', 'All sports activities', 'Approved', NULL);
GO
