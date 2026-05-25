-- ============================================================
--  SocietiesDB — Complete Fixed Schema
--  Drop & recreate all tables to match C# service layer
-- ============================================================

-- Step 1: Create or switch to the database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SocietiesDB')
    CREATE DATABASE SocietiesDB;
GO

USE SocietiesDB;
GO

-- ============================================================
-- Step 2: Drop old tables (in reverse FK order)
-- ============================================================
IF OBJECT_ID('EventRegistrations', 'U') IS NOT NULL DROP TABLE EventRegistrations;
IF OBJECT_ID('Events',             'U') IS NOT NULL DROP TABLE Events;
IF OBJECT_ID('Tasks',              'U') IS NOT NULL DROP TABLE Tasks;
IF OBJECT_ID('Memberships',        'U') IS NOT NULL DROP TABLE Memberships;
IF OBJECT_ID('Societies',          'U') IS NOT NULL DROP TABLE Societies;
IF OBJECT_ID('Users',              'U') IS NOT NULL DROP TABLE Users;
IF OBJECT_ID('Students',           'U') IS NOT NULL DROP TABLE Students;
GO

-- ============================================================
-- Step 3: Create correct tables
-- ============================================================

-- Users (replaces old Students table — supports all roles)
CREATE TABLE Users (
    UserId   INT PRIMARY KEY IDENTITY,
    Name     NVARCHAR(100)  NOT NULL,
    Email    NVARCHAR(100)  NOT NULL UNIQUE,
    Password NVARCHAR(100)  NOT NULL,
    Role     NVARCHAR(50)   NOT NULL DEFAULT 'Student'
    -- Role values: 'Student', 'SocietyHead', 'Admin'
);
GO

-- Societies (added Status and HeadId columns)
CREATE TABLE Societies (
    SocietyId   INT PRIMARY KEY IDENTITY,
    Name        NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Status      NVARCHAR(50)  NOT NULL DEFAULT 'Pending',
    -- Status values: 'Pending', 'Approved', 'Suspended'
    HeadId      INT NULL,
    FOREIGN KEY (HeadId) REFERENCES Users(UserId)
);
GO

-- Memberships (StudentId now references Users)
CREATE TABLE Memberships (
    MembershipId INT PRIMARY KEY IDENTITY,
    StudentId    INT          NOT NULL,
    SocietyId    INT          NOT NULL,
    Status       NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    -- Status values: 'Pending', 'Approved', 'Rejected'
    FOREIGN KEY (StudentId) REFERENCES Users(UserId),
    FOREIGN KEY (SocietyId) REFERENCES Societies(SocietyId)
);
GO

-- Events (added Description, Status, MaxTickets columns)
CREATE TABLE Events (
    EventId     INT PRIMARY KEY IDENTITY,
    Title       NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Date        DATETIME      NOT NULL,
    SocietyId   INT           NOT NULL,
    Status      NVARCHAR(50)  NOT NULL DEFAULT 'Pending',
    -- Status values: 'Pending', 'Approved', 'Cancelled'
    MaxTickets  INT           NOT NULL DEFAULT 0,
    -- 0 = unlimited
    FOREIGN KEY (SocietyId) REFERENCES Societies(SocietyId)
);
GO

-- EventRegistrations (added Status column)
CREATE TABLE EventRegistrations (
    RegistrationId INT PRIMARY KEY IDENTITY,
    StudentId      INT          NOT NULL,
    EventId        INT          NOT NULL,
    Status         NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    -- Status values: 'Pending', 'Approved', 'Rejected'
    FOREIGN KEY (StudentId) REFERENCES Users(UserId),
    FOREIGN KEY (EventId)   REFERENCES Events(EventId)
);
GO

-- Tasks (new table — required by TaskService)
CREATE TABLE Tasks (
    TaskId      INT PRIMARY KEY IDENTITY,
    Title       NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    AssignedTo  INT           NOT NULL,
    SocietyId   INT           NOT NULL,
    Status      NVARCHAR(50)  NOT NULL DEFAULT 'Pending',
    -- Status values: 'Pending', 'InProgress', 'Completed'
    FOREIGN KEY (AssignedTo) REFERENCES Users(UserId),
    FOREIGN KEY (SocietyId)  REFERENCES Societies(SocietyId)
);
GO

-- ============================================================
-- Step 4: Seed data for testing
-- ============================================================

-- Admin user
INSERT INTO Users (Name, Email, Password, Role) VALUES
('Admin User',   'admin@uni.edu',   'Admin@99',  'Admin'),
('Head Ali',     'head@uni.edu',    'Head@55',   'SocietyHead'),
('Head Sara',    'head2@uni.edu',   'Head@66',   'SocietyHead'),
('Student Ahmed','ahmed@uni.edu',   'Ahmed@1',   'Student'),
('Student Zara', 'zara@uni.edu',    'Zara@1',    'Student');
GO

-- Societies
INSERT INTO Societies (Name, Description, Status, HeadId) VALUES
('Gaming Society',    'All about gaming',        'Approved',  2),
('Developers Club',   'Coding and tech',         'Approved',  3),
('Media Society',     'Photography & video',     'Pending',   NULL),
('Sports Society',    'All sports activities',   'Pending',   NULL);
GO

-- Memberships
INSERT INTO Memberships (StudentId, SocietyId, Status) VALUES
(4, 1, 'Approved'),
(5, 1, 'Pending'),
(4, 2, 'Pending');
GO

-- Events
INSERT INTO Events (Title, Description, Date, SocietyId, Status, MaxTickets) VALUES
('Game Jam 2025',   'Annual gaming event',   '2025-07-10', 1, 'Approved', 50),
('Hackathon 2025',  '24hr coding challenge', '2025-08-01', 2, 'Pending',  0),
('Photo Walk',      'City photography tour', '2025-06-15', 3, 'Pending',  20);
GO

-- Event Registrations
INSERT INTO EventRegistrations (StudentId, EventId, Status) VALUES
(4, 1, 'Approved'),
(5, 1, 'Pending');
GO

-- Tasks
INSERT INTO Tasks (Title, Description, AssignedTo, SocietyId, Status) VALUES
('Design Poster',  'A3 size banner',     4, 1, 'Pending'),
('Write Blog',     'Event recap post',   5, 1, 'Pending'),
('Setup Laptop',   'Dev environment',    4, 2, 'Completed');
GO

-- ============================================================
-- Step 5: Verify all tables created correctly
-- ============================================================
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';
GO
