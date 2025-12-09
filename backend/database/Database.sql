CREATE TABLE Users (
    UserID INT PRIMARY KEY identity(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(200) NOT NULL,
    IsAdmin BIT NOT NULL
);

CREATE TABLE TicketStatus (
    id INT PRIMARY KEY identity(1,1),
    Name NVARCHAR(50) NOT NULL
);

CREATE TABLE Tickets (
    id INT PRIMARY KEY identity(1,1),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    UserId INT NOT NULL,
    StatusId INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserID),
    FOREIGN KEY (StatusId) REFERENCES TicketStatus(id)
)

INSERT INTO TicketStatus (Name) VALUES 
('Öppen'),
('Pågående'),
('Löst'),
('Stängd');