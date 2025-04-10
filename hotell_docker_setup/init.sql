-- init.sql

CREATE DATABASE IF NOT EXISTS HotelBooking
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

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
    Status VARCHAR(50) NOT NULL DEFAULT 'Confirmed',
    FOREIGN KEY (GuestId) REFERENCES Guest(GuestId),
    FOREIGN KEY (RoomId) REFERENCES Room(RoomId)
);


CREATE TABLE Client (
    ClientId INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    BillingAddress VARCHAR(200),
    ContactPerson VARCHAR(100),
    ContactNumber VARCHAR(20)
);

CREATE TABLE Event (
    EventId INT AUTO_INCREMENT PRIMARY KEY,
    HotelId INT,
    Name VARCHAR(100),
    OrganiserId INT,
    StartDate DATE,
    EndDate DATE,
    StartTime TIME,
    EndTime TIME,
    FOREIGN KEY (HotelId) REFERENCES Hotel(HotelId),
    FOREIGN KEY (OrganiserId) REFERENCES Client(ClientId)

);

CREATE TABLE Meal (
    MealId INT AUTO_INCREMENT PRIMARY KEY,
    OrganiserId INT,
    RoomId INT,
    MealDate DATE,
    StartTime TIME,
    EndTime TIME,
    Attendees INT,
    DietaryNotes TEXT,
    FOREIGN KEY (OrganiserId) REFERENCES Client(ClientId),
    FOREIGN KEY (RoomId) REFERENCES Room(RoomId)
);

CREATE TABLE EventClient (
    EventId INT,
    ClientId INT,

    PRIMARY KEY (EventId, ClientId),
    FOREIGN KEY (EventId) REFERENCES Event(EventId),
    FOREIGN KEY (ClientId) REFERENCES Client(ClientId)
);

CREATE TABLE EventRoom (
    EventId INT,
    RoomId INT,

    PRIMARY KEY (EventId, RoomId),
    FOREIGN KEY (EventId) REFERENCES Event(EventId),
    FOREIGN KEY (RoomId) REFERENCES Room(RoomId)
);

ALTER TABLE Event
ADD CONSTRAINT unique_event UNIQUE (Name, StartDate);


INSERT INTO User (Name, Email, Password, Role) VALUES
('Alice Admin', 'alice@hotel.com', 'pass123', 'Admin'),
('Bob Resepsjonist', 'bob@hotel.com', 'pass123', 'Receptionist');

INSERT INTO Hotel (Name, Address) VALUES
('Ocean View Hotel', 'Strandgata 42, Oslo'),
('Mountain Retreat Hotel', 'Fjellveien 12, Bergen');

INSERT INTO Room (HotelId, RoomNumber, Type, Price, IsAvailable) VALUES
(1, '101', 'Single', 899.00, 1),
(1, '102', 'Double', 1199.00, 1),
(1, '201', 'Suite', 1899.00, 1),
(1, '202', 'Family', 1499.00, 1),
(1, '301', 'Deluxe', 1599.00, 1),
(1, '302', 'Presidential Suite', 2999.00, 1),
(1, '401', 'Twin', 999.00, 1),
(2, '101', 'Single', 799.00, 1),
(2, '102', 'Double', 1199.00, 1),
(2, '201', 'Suite', 2099.00, 1),
(2, '202', 'Family', 1599.00, 1),
(2, '301', 'Deluxe', 1799.00, 1);

INSERT INTO Guest (Name, ContactNumber, Email) VALUES
('Anna Berg', '+4798765432', 'anna.berg92@gmail.com'),
('James Dean', '+12135550102', 'james.d1955@gmail.com'),
('Lars Olsen', '+4744556677', 'lars.olsen88@hotmail.com'),
('Elizabeth Taylor', '+12135550104', 'elizabeth.taylor1952@gmail.com'),
('Maja Nilsen', '+4798123456', 'maja.nilsen77@gmail.com'),
('Humphrey Bogart', '+12135550103', 'h.bogart47@hotmail.com'),
('Lucas Müller', '+4915123456789', 'lucas.m89@gmail.com'),
('Sophie Dubois', '+33123456789', 'sophie.dubois91@hotmail.com'),
('Rock Hudson', '+12135550105', 'rock.h_1950@gmail.com'),
('Carlos García', '+34912345678', 'carlosg.84@gmail.com'),
('Fatima Al-Fulan', '+971501234567', 'fatima.af1970@hotmail.com'),
('Jonas Moe', '+4798771122', 'jonas.moe93@gmail.com'),
('Cary Grant', '+441234567891', 'cary.g1953@gmail.com'),
('Emily Johnson', '+16135550123', 'emily.j_1986@hotmail.com'),
('Grace Kelly', '+331122334455', 'grace.kelly55@gmail.com'),
('Isabella Rossi', '+39061234567', 'isabella.rossi90@gmail.com'),
('Akira Tanaka', '+81312345678', 'akira.tanaka1983@hotmail.com'),
('Nora Skaug', '+4744332211', 'nora.skaug74@gmail.com'),
('Audrey Hepburn', '+441234567890', 'audreyh_59@gmail.com'),
('Linh Nguyen', '+84912345678', 'linh.nguyen95@gmail.com'),
('John Smith', '+441632960001', 'john.smith837@gmail.com'),
('Thomas Lie', '+4744123321', 'thomas.lie78@hotmail.com'),
('Marilyn Monroe', '+12135550101', 'marilyn.m58@gmail.com'),
('Charlie Gjest', '+4711223344', 'charlie@mail.com');

INSERT INTO Booking (GuestId, RoomId, CheckIn, CheckOut) VALUES
(1, 1, '2015-05-10', '2015-05-13'),
(2, 2, '2016-07-04', '2016-07-07'),
(3, 1, '2017-09-01', '2017-09-03'),
(4, 2, '2018-03-15', '2018-03-18'),
(5, 1, '2019-11-20', '2019-11-23'),
(6, 2, '2020-01-10', '2020-01-13'),
(7, 1, '2021-04-05', '2021-04-08'),
(8, 2, '2022-06-01', '2022-06-04'),
(9, 1, '2023-08-12', '2023-08-15'),
(10, 2, '2024-02-01', '2024-02-03'),
(11, 1, '2025-03-01', '2025-03-03'),
(12, 2, '2025-04-15', '2025-04-18'),
(13, 1, '2025-06-20', '2025-06-23'),
(14, 2, '2025-07-10', '2025-07-13'),
(15, 1, '2025-09-01', '2025-09-04'),
(16, 2, '2025-10-05', '2025-10-08'),
(17, 1, '2025-11-15', '2025-11-18'),
(18, 2, '2025-12-01', '2025-12-04'),
(19, 1, '2026-01-10', '2026-01-13'),
(20, 2, '2026-02-05', '2026-02-08'),
(21, 1, '2026-03-01', '2026-03-04'),
(22, 2, '2026-04-10', '2026-04-13'),
(23, 1, '2026-05-15', '2026-05-18'),
(24, 2, '2026-06-20', '2026-06-23');

INSERT INTO Client (Name, BillingAddress, ContactPerson, ContactNumber) VALUES
('Innovatech AS', 'Billingstadsletta 17, 1396 Billingstad', 'Lene Hansen', '+4798456123'),
('Nordic Weddings', 'post@nordicweddings.no', 'Martin Haug', '+4740551122'),
('Oslo Kommune', 'Rådhuset, 0037 Oslo', 'Kari Nilsen', '+4723000000'),
('Fjord IT', 'fjordsupport@fjordit.no', 'Eirik Blom', '+4790123456'),
('HealthNet', 'billing@healthnet.com', 'Sara Lund', '+4712345678'),
('Gokstad Rotary', 'Torggata 2, 3210 Sandefjord', 'Nils Ervik', '+4798765432'),
('Eiker Konferanser', 'Billing@eiker-konferanse.no', 'Randi Foss', '+4744332211'),
('Fest og Moro', 'fest@moro.no', 'Per Humor', '+4799887766'),
('Arktis Reiser', 'arktis@reiser.no', 'Line Nord', '+4797123456'),
('Kaffe og Kode AS', 'kaffe@kode.no', 'Anders Dev', '+4791123581');

INSERT INTO Event (HotelId,Name, OrganiserId, StartDate, EndDate, StartTime, EndTime) VALUES
(1, 'Event 1',1, '2025-05-10', '2025-05-10', '09:00:00', '15:00:00'),
(1, 'Event 2',2, '2025-06-15', '2025-06-15', '13:00:00', '23:00:00'),
(1, 'Event 3',3, '2025-07-01', '2025-07-03', '08:00:00', '17:00:00'),
(1, 'Event 4',4, '2025-08-20', '2025-08-20', '10:00:00', '14:00:00'),
(1, 'Event 5',5, '2025-09-05', '2025-09-05', '19:00:00', '23:00:00'),
(1, 'Event 6',6, '2025-10-12', '2025-10-12', '09:00:00', '17:00:00'),
(1, 'Event 7',7, '2025-11-01', '2025-11-01', '15:00:00', '20:00:00'),
(1, 'Event 8',8, '2025-12-24', '2025-12-24', '18:00:00', '23:59:00'),
(1, 'Event 9',9,  '2026-01-14', '2026-01-14', '12:00:00', '16:00:00'),
(1, 'Event 10',10, '2026-02-01', '2026-02-01', '10:00:00', '14:00:00');

INSERT INTO Meal (OrganiserId, RoomId, MealDate, StartTime, EndTime, Attendees, DietaryNotes) VALUES
(1, 3, '2025-05-10', '12:00:00', '13:00:00', 30, 'Vegetarian options needed'),
(2, 4, '2025-06-15', '18:00:00', '20:00:00', 100, 'Gluten-free & vegan options'),
(3, 5, '2025-07-02', '08:00:00', '09:00:00', 15, ''),
(4, 3, '2025-08-20', '11:30:00', '12:30:00', 40, 'Nut allergies'),
(5, 6, '2025-09-05', '19:30:00', '21:00:00', 80, 'Halal meals required'),
(6, 3, '2025-10-12', '13:00:00', '14:00:00', 20, ''),
(7, 5, '2025-11-01', '17:00:00', '18:00:00', 50, 'Lactose-free dessert'),
(8, 4, '2025-12-24', '20:00:00', '21:00:00', 60, 'Traditional Norwegian food'),
(9, 6, '2026-01-14', '13:00:00', '14:00:00', 25, 'Vegetarian only'),
(10, 3, '2026-02-01', '12:30:00', '13:30:00', 12, '');

-- Meals på nytt hotell i Bergen
INSERT INTO Meal (OrganiserId, RoomId, MealDate, StartTime, EndTime, Attendees, DietaryNotes) VALUES
(1, 201, '2025-09-12', '12:00:00', '13:30:00', 40, 'Vegetarian friendly'),
(2, 202, '2025-10-05', '18:00:00', '20:00:00', 60, 'Vegan and gluten-free options'),
(3, 301, '2025-11-22', '08:00:00', '09:30:00', 25, 'No nuts, lactose-free milk'),
(4, 102, '2026-01-15', '12:30:00', '14:00:00', 30, 'Kosher meals required'),
(5, 101, '2026-02-20', '19:00:00', '21:00:00', 45, 'Halal, vegan dessert');

-- Flere meals på Ocean View Hotel (Oslo)
INSERT INTO Meal (OrganiserId, RoomId, MealDate, StartTime, EndTime, Attendees, DietaryNotes) VALUES
(6, 3, '2025-08-18', '13:00:00', '14:30:00', 35, 'Pescatarian options'),
(7, 4, '2025-12-31', '20:00:00', '23:59:00', 80, 'Festive menu, includes traditional dishes'),
(8, 5, '2026-03-17', '07:30:00', '09:00:00', 20, 'Gluten-free pastries'),
(9, 6, '2026-04-25', '18:00:00', '20:30:00', 50, 'Low-carb options'),
(10, 1, '2026-05-05', '12:00:00', '13:00:00', 15, 'Vegetarian only');

INSERT INTO EventClient (EventId, ClientId) VALUES
(1, 1),
(1, 2),
(2, 3),
(2, 4),
(3, 5),
(4, 6),
(4, 7),
(5, 8),
(6, 9),
(7, 10);

INSERT INTO EventRoom (EventId, RoomId) VALUES
(1, 1),
(1, 3),
(2, 4),
(2, 5),
(3, 201),
(3, 202),
(4, 301),
(5, 102),
(6, 101),
(7, 202);

INSERT INTO Event (HotelId, Name, OrganiserId, StartDate, EndDate, StartTime, EndTime) VALUES
(2, 'Bergen Tech Meetup', 3, '2025-11-05', '2025-11-05', '09:00:00', '17:00:00'),
(2, 'Nordic Sustainability Conference', 4, '2026-02-10', '2026-02-12', '08:30:00', '16:30:00'),
(2, 'Mountain Retreat Yoga', 5, '2026-03-20', '2026-03-22', '07:00:00', '15:00:00'),
(2, 'Bergen Business Gala', 2, '2026-04-15', '2026-04-15', '18:00:00', '23:59:00'),
(2, 'Outdoor Adventure Expo', 1, '2026-05-05', '2026-05-07', '10:00:00', '18:00:00');

INSERT INTO EventClient (EventId, ClientId) VALUES
(11, 3),
(11, 4),
(12, 5),
(13, 6),
(13, 7),
(14, 2),
(15, 1),
(15, 8);


INSERT INTO EventRoom (EventId, RoomId) VALUES
(11, 201), -- Bergen rom
(11, 202),
(12, 301),
(12, 102),
(13, 101),
(14, 202),
(14, 301),
(15, 101),
(15, 202);


