-- init.sql

CREATE DATABASE IF NOT EXISTS HotelBooking;
USE HotelBooking;

CREATE TABLE User (
    UserId INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100),
    Email VARCHAR(100) UNIQUE,
    Password VARCHAR(100),
    Role ENUM('Admin', 'Receptionist') NOT NULL
);


CREATE TABLE Hotel (
    HotelId INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100),
    Address VARCHAR(200)
);

CREATE TABLE Room (
    RoomId INT AUTO_INCREMENT PRIMARY KEY,
    HotelId INT,
    RoomNumber VARCHAR(20),
    Type VARCHAR(50),
    Price DECIMAL(10,2),
    IsAvailable BOOLEAN,
    FOREIGN KEY (HotelId) REFERENCES Hotel(HotelId)
);

CREATE TABLE Guest (
    GuestId INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100),
    ContactNumber VARCHAR(20),
    Email VARCHAR(100)
);

CREATE TABLE Booking (
    BookingId INT AUTO_INCREMENT PRIMARY KEY,
    GuestId INT,
    RoomId INT,
    CheckIn DATE,
    CheckOut DATE,
    FOREIGN KEY (GuestId) REFERENCES Guest(GuestId),
    FOREIGN KEY (RoomId) REFERENCES Room(RoomId)
);

INSERT INTO User (Name, Email, Password, Role) VALUES
('Alice Admin', 'alice@hotel.com', 'pass123', 'Admin'),
('Bob Resepsjonist', 'bob@hotel.com', 'pass123', 'Receptionist');

INSERT INTO Hotel (Name, Address) VALUES
('Ocean View Hotel', 'Strandgata 42, Oslo');

INSERT INTO Room (HotelId, RoomNumber, Type, Price, IsAvailable) VALUES
(1, '101', 'Single', 899.00, TRUE),
(1, '102', 'Double', 1199.00, TRUE);

INSERT INTO Guest (Name, ContactNumber, Email) VALUES
('Charlie Gjest', '+4711223344', 'charlie@mail.com');

INSERT INTO Booking (GuestId, RoomId, CheckIn, CheckOut) VALUES
(1, 1, '2025-04-01', '2025-04-03');
