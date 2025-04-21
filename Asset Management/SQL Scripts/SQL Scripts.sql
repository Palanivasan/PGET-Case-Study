CREATE DATABASE AssetDB

USE AssetDB

-- Creation of Employees table  

CREATE TABLE Employees (
    employee_id INT IDENTITY(1,1),
    name VARCHAR(255) NOT NULL,
    department VARCHAR(255),
    email VARCHAR(255) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    CONSTRAINT PK_Emp PRIMARY KEY (employee_id)
)

-- Creation of Assets table  

CREATE TABLE Assets (
    asset_id INT IDENTITY(1,1),
    name VARCHAR(255) NOT NULL,
    type VARCHAR(100) NOT NULL,
    serial_number VARCHAR(100) UNIQUE NOT NULL,
    purchase_date DATE NOT NULL,
    location VARCHAR(255),
    status VARCHAR(50) CHECK (status IN ('in use', 'decommissioned', 'under maintenance')),
    owner_id INT NULL,
    CONSTRAINT PK_Ast PRIMARY KEY (asset_id),
    CONSTRAINT FK_Ast_Emp FOREIGN KEY (owner_id) REFERENCES Employees(employee_id) ON DELETE SET NULL
)

-- Creation of MaintenanceRecords table  

CREATE TABLE MaintenanceRecords (
    maintenance_id INT IDENTITY(1,1),
    asset_id INT NOT NULL,
    maintenance_date DATE NOT NULL,
    description VARCHAR(255) NOT NULL,
    cost DECIMAL(10,2) CHECK (cost >= 0),
    CONSTRAINT PK_Mnt PRIMARY KEY (maintenance_id),
    CONSTRAINT FK_Mnt_Ast FOREIGN KEY (asset_id) REFERENCES Assets(asset_id) ON DELETE CASCADE
)

-- Creation of AssetAllocations table 

CREATE TABLE AssetAllocations (
    allocation_id INT IDENTITY(1,1),
    asset_id INT NOT NULL,
    employee_id INT NOT NULL,
    allocation_date DATE NOT NULL,
    return_date DATE NULL,
    CONSTRAINT PK_Alloc PRIMARY KEY (allocation_id),
    CONSTRAINT FK_Alloc_Ast FOREIGN KEY (asset_id) REFERENCES Assets(asset_id) ON DELETE CASCADE,
    CONSTRAINT FK_Alloc_Emp FOREIGN KEY (employee_id) REFERENCES Employees(employee_id) ON DELETE CASCADE
)

-- Creation of Reservations table  

CREATE TABLE Reservations (
    reservation_id INT IDENTITY(1,1),
    asset_id INT NOT NULL,
    employee_id INT NOT NULL,
    reservation_date DATE NOT NULL DEFAULT GETDATE(),
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    status VARCHAR(50) CHECK (status IN ('pending', 'approved', 'canceled')),
    CONSTRAINT PK_Res PRIMARY KEY (reservation_id),
    CONSTRAINT FK_Res_Ast FOREIGN KEY (asset_id) REFERENCES Assets(asset_id) ON DELETE CASCADE,
    CONSTRAINT FK_Res_Emp FOREIGN KEY (employee_id) REFERENCES Employees(employee_id) ON DELETE CASCADE
)

SELECT * FROM Assets WHERE asset_id = 9999
DBCC CHECKIDENT ('Assets', RESEED, 10);
delete from Assets
where AssetId>=1148;
select * from Employees
ALTER DATABASE AseestDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE AssetDB;
USE master;
GO

ALTER DATABASE AssetDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

DROP DATABASE AssetDB;
select * from Assets
-- Insert Employees
INSERT INTO Employees (name, department, email, password) VALUES
('Arun Kumar', 'IT', 'arun.kumar@company.com', 'password123'),
('Meena Ramesh', 'Finance', 'meena.ramesh@company.com', 'secure456'),
('Sathish Raj', 'Operations', 'sathish.raj@company.com', 'pass789'),
('Divya Shree', 'HR', 'divya.shree@company.com', 'divya@123'),
('Karthik M', 'IT', 'karthik.m@company.com', 'kar@123'),
('Janani Priya', 'Marketing', 'janani.priya@company.com', 'jana456'),
('Pradeep V', 'Finance', 'pradeep.v@company.com', 'prad321'),
('Revathi N', 'Operations', 'revathi.n@company.com', 'rev999'),
('Bharath Kumar', 'IT', 'bharath.kumar@company.com', 'bharath987'),
('Yamini R', 'HR', 'yamini.r@company.com', 'yamini@456');

-- Insert Assets
INSERT INTO Assets (name, type, serial_number, purchase_date, location, status, owner_id) VALUES
('Dell Laptop', 'Laptop', 'TN-LAP-001', '2023-01-10', 'Chennai', 'in use', 1),
('Canon Printer', 'Printer', 'TN-PRT-002', '2022-05-20', 'Coimbatore', 'under maintenance', NULL),
('HP Server', 'Server', 'TN-SRV-003', '2021-03-15', 'Madurai', 'in use', 3),
('Lenovo Desktop', 'Desktop', 'TN-DESK-004', '2024-02-01', 'Trichy', 'decommissioned', NULL),
('Acer Laptop', 'Laptop', 'TN-LAP-005', '2023-07-12', 'Salem', 'in use', 5),
('Brother Printer', 'Printer', 'TN-PRT-006', '2023-09-25', 'Chennai', 'in use', 2),
('Cisco Router', 'Networking', 'TN-NTW-007', '2022-11-05', 'Madurai', 'under maintenance', 7),
('Asus Laptop', 'Laptop', 'TN-LAP-008', '2023-12-11', 'Chennai', 'in use', 8),
('Samsung Monitor', 'Monitor', 'TN-MON-009', '2021-04-19', 'Coimbatore', 'in use', 9),
('HP Desktop', 'Desktop', 'TN-DESK-010', '2022-08-28', 'Trichy', 'decommissioned', NULL);

-- Insert MaintenanceRecords
INSERT INTO MaintenanceRecords (asset_id, maintenance_date, description, cost) VALUES
(2, '2024-03-10', 'Replaced toner and cleaned', 1500.00),
(3, '2023-12-05', 'Upgraded memory and replaced HDD', 5000.00),
(6, '2024-01-14', 'Paper jam fix', 800.00),
(7, '2024-04-05', 'Firmware update and port repair', 2000.00),
(4, '2024-02-25', 'Replaced motherboard', 7500.00),
(10, '2023-10-10', 'Replaced power supply', 1200.00),
(5, '2024-03-20', 'Screen replacement', 6500.00),
(1, '2024-04-01', 'Battery replacement', 3500.00),
(8, '2024-04-12', 'Keyboard replacement', 2500.00),
(9, '2024-04-15', 'Display issue fix', 4000.00);

-- Insert AssetAllocations
INSERT INTO AssetAllocations (asset_id, employee_id, allocation_date, return_date) VALUES
(1, 1, '2024-01-15', NULL),
(3, 3, '2023-11-01', '2024-03-01'),
(5, 5, '2024-02-10', NULL),
(6, 2, '2024-03-12', NULL),
(7, 7, '2024-04-03', NULL),
(8, 8, '2024-04-05', NULL),
(9, 9, '2024-04-08', NULL),
(2, 4, '2024-02-20', '2024-04-10'),
(4, 10, '2024-01-30', '2024-03-30'),
(10, 6, '2023-12-15', '2024-02-28');

-- Insert Reservations
INSERT INTO Reservations (asset_id, employee_id, reservation_date, start_date, end_date, status) VALUES
(2, 2, '2024-04-15', '2024-04-20', '2024-04-25', 'approved'),
(4, 4, '2024-04-16', '2024-05-01', '2024-05-05', 'pending'),
(5, 5, '2024-04-17', '2024-04-22', '2024-04-27', 'approved'),
(6, 6, '2024-04-18', '2024-04-23', '2024-04-29', 'canceled'),
(7, 7, '2024-04-19', '2024-04-24', '2024-04-30', 'pending'),
(8, 8, '2024-04-20', '2024-04-25', '2024-05-01', 'approved'),
(9, 9, '2024-04-21', '2024-04-26', '2024-05-02', 'pending'),
(1, 1, '2024-04-22', '2024-04-27', '2024-05-03', 'approved'),
(3, 3, '2024-04-23', '2024-04-28', '2024-05-04', 'pending'),
(10, 10, '2024-04-24', '2024-04-29', '2024-05-05', 'canceled');
