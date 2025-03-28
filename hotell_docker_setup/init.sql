-- init.sql

CREATE DATABASE IF NOT EXISTS HotelBooking;
USE HotelBooking;

CREATE TABLE User (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100),
    email VARCHAR(100) UNIQUE,
    password VARCHAR(100),
    role ENUM('Admin', 'Receptionist') NOT NULL
);

CREATE TABLE Admin (
    admin_id INT PRIMARY KEY,
    employee_code VARCHAR(50),
    FOREIGN KEY (admin_id) REFERENCES User(user_id)
);

CREATE TABLE Receptionist (
    receptionist_id INT PRIMARY KEY,
    employee_code VARCHAR(50),
    FOREIGN KEY (receptionist_id) REFERENCES User(user_id)
);

CREATE TABLE Hotel (
    hotel_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100),
    address VARCHAR(200)
);

CREATE TABLE Room (
    room_id INT AUTO_INCREMENT PRIMARY KEY,
    hotel_id INT,
    room_number VARCHAR(20),
    type VARCHAR(50),
    price DECIMAL(10,2),
    is_available BOOLEAN,
    FOREIGN KEY (hotel_id) REFERENCES Hotel(hotel_id)
);

CREATE TABLE Guest (
    guest_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100),
    contact_number VARCHAR(20),
    email VARCHAR(100)
);

CREATE TABLE Booking (
    booking_id INT AUTO_INCREMENT PRIMARY KEY,
    guest_id INT,
    room_id INT,
    check_in DATE,
    check_out DATE,
    FOREIGN KEY (guest_id) REFERENCES Guest(guest_id),
    FOREIGN KEY (room_id) REFERENCES Room(room_id)
);

INSERT INTO User (name, email, password, role) VALUES
('Alice Admin', 'alice@hotel.com', 'pass123', 'Admin'),
('Bob Resepsjonist', 'bob@hotel.com', 'pass123', 'Receptionist');

INSERT INTO Admin (admin_id, employee_code) VALUES (1, 'ADM001');
INSERT INTO Receptionist (receptionist_id, employee_code) VALUES (2, 'REC001');

INSERT INTO Hotel (name, address) VALUES
('Ocean View Hotel', 'Strandgata 42, Oslo');

INSERT INTO Room (hotel_id, room_number, type, price, is_available) VALUES
(1, '101', 'Single', 899.00, TRUE),
(1, '102', 'Double', 1199.00, TRUE);

INSERT INTO Guest (name, contact_number, email) VALUES
('Charlie Gjest', '+4711223344', 'charlie@mail.com');

INSERT INTO Booking (guest_id, room_id, check_in, check_out) VALUES
(1, 1, '2025-04-01', '2025-04-03');
