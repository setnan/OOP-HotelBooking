-- init_postgres.sql

CREATE TABLE "User" (
"UserId" SERIAL PRIMARY KEY,
"Name" VARCHAR(100),
"Email" VARCHAR(100) UNIQUE,
"Password" VARCHAR(100),
"Role" VARCHAR(20) CHECK ("Role" IN ('Admin', 'Receptionist'))
);

CREATE TABLE "Admin" (
 "AdminId" INT PRIMARY KEY,
 "EmployeeCode" VARCHAR(50),
 FOREIGN KEY ("AdminId") REFERENCES "User"("UserId")
);

CREATE TABLE "Receptionist" (
"ReceptionistId" INT PRIMARY KEY,
"EmployeeCode" VARCHAR(50),
FOREIGN KEY ("ReceptionistId") REFERENCES "User"("UserId")
);

CREATE TABLE "Hotel" (
 "HotelId" SERIAL PRIMARY KEY,
 "Name" VARCHAR(100),
 "Address" VARCHAR(200)
);

CREATE TABLE "Room" (
"RoomId" SERIAL PRIMARY KEY,
"HotelId" INT,
"RoomNumber" VARCHAR(20),
"Type" VARCHAR(50),
"Price" DECIMAL(10,2),
"IsAvailable" BOOLEAN,
FOREIGN KEY ("HotelId") REFERENCES "Hotel"("HotelId")
);

CREATE TABLE "Guest" (
 "GuestId" SERIAL PRIMARY KEY,
 "Name" VARCHAR(100),
 "ContactNumber" VARCHAR(20),
 "Email" VARCHAR(100)
);

CREATE TABLE "Booking" (
"BookingId" SERIAL PRIMARY KEY,
"GuestId" INT,
"RoomId" INT,
"CheckIn" DATE,
"CheckOut" DATE,
FOREIGN KEY ("GuestId") REFERENCES "Guest"("GuestId"),
FOREIGN KEY ("RoomId") REFERENCES "Room"("RoomId")
);

INSERT INTO "User" ("Name", "Email", "Password", "Role") VALUES
('Alice Admin', 'alice@hotel.com', 'pass123', 'Admin'),
('Bob Resepsjonist', 'bob@hotel.com', 'pass123', 'Receptionist');

INSERT INTO "Admin" ("AdminId", "EmployeeCode") VALUES (1, 'ADM001');
INSERT INTO "Receptionist" ("ReceptionistId", "EmployeeCode") VALUES (2, 'REC001');

INSERT INTO "Hotel" ("Name", "Address") VALUES
    ('Ocean View Hotel', 'Strandgata 42, Oslo');

INSERT INTO "Room" ("HotelId", "RoomNumber", "Type", "Price", "IsAvailable") VALUES
(1, '101', 'Single', 899.00, TRUE),
(1, '102', 'Double', 1199.00, TRUE),
(1, '201', 'Suite', 1899.00, TRUE),
(1, '202', 'Family', 1499.00, TRUE),
(1, '301', 'Deluxe', 1599.00, TRUE),
(1, '302', 'Presidential Suite', 2999.00, TRUE),
(1, '401', 'Twin', 999.00, TRUE);

INSERT INTO "Guest" ("Name", "ContactNumber", "Email") VALUES
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

INSERT INTO "Booking" ("GuestId", "RoomId", "CheckIn", "CheckOut") VALUES
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
