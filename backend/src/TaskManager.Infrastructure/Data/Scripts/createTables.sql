-- Users table with unique username constraint
CREATE TABLE Users (
    Id TEXT PRIMARY KEY,
    Username TEXT COLLATE NOCASE NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    CreatedAt TEXT NOT NULL
);

-- Projects table with user relationship
CREATE TABLE Projects (
    Id TEXT PRIMARY KEY,
    UserId TEXT NOT NULL,
    Name TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    IsDefault INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Tasks table with project relationship
CREATE TABLE Tasks (
    Id TEXT PRIMARY KEY,
    ProjectId TEXT NOT NULL,
    Title TEXT NOT NULL,
    Description TEXT,
    CreatedAt TEXT NOT NULL,
    IsCompleted INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
);