USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'HomeInventoryTrackerDB')
BEGIN
    CREATE DATABASE HomeInventoryTrackerDB;
END
GO

USE HomeInventoryTrackerDB;
GO

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);

CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255),
    IsActive BIT DEFAULT 1
);

CREATE TABLE Sellers (
    SellerID INT IDENTITY(1,1) PRIMARY KEY,
    SellerName NVARCHAR(100) NOT NULL,
    ContactNumber NVARCHAR(20),
    Email NVARCHAR(100),
    Address NVARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE InventoryItems (
    ItemID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    CategoryID INT NOT NULL,
    SellerID INT,
    ProductName NVARCHAR(150) NOT NULL,
    Brand NVARCHAR(100),
    ModelNumber NVARCHAR(100),
    SerialNumber NVARCHAR(100),
    PurchaseDate DATE,
    PurchasePrice DECIMAL(18, 2),
    Location NVARCHAR(100),
    ItemCondition NVARCHAR(50),
    Description NVARCHAR(MAX),
    WarrantyStartDate DATE,
    WarrantyExpiryDate DATE,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME,
    IsDeleted BIT DEFAULT 0,
    CONSTRAINT FK_InventoryItems_Users FOREIGN KEY (UserID) REFERENCES Users(UserID),
    CONSTRAINT FK_InventoryItems_Categories FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID) ON DELETE CASCADE,
    CONSTRAINT FK_InventoryItems_Sellers FOREIGN KEY (SellerID) REFERENCES Sellers(SellerID) ON DELETE SET NULL
);

CREATE TABLE Warranties (
    WarrantyID INT IDENTITY(1,1) PRIMARY KEY,
    ItemID INT NOT NULL,
    WarrantyType NVARCHAR(50) NOT NULL,
    ProviderName NVARCHAR(100) NOT NULL,
    StartDate DATE NOT NULL,
    ExpiryDate DATE NOT NULL,
    WarrantyPeriodMonths INT,
    WarrantyTerms NVARCHAR(MAX),
    ContactInformation NVARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Warranties_InventoryItems FOREIGN KEY (ItemID) REFERENCES InventoryItems(ItemID) ON DELETE CASCADE
);

CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_InventoryItems_UserID ON InventoryItems(UserID);
CREATE INDEX IX_InventoryItems_CategoryID ON InventoryItems(CategoryID);
CREATE INDEX IX_InventoryItems_PurchaseDate ON InventoryItems(PurchaseDate);
CREATE INDEX IX_InventoryItems_WarrantyExpiryDate ON InventoryItems(WarrantyExpiryDate);
CREATE INDEX IX_InventoryItems_SerialNumber ON InventoryItems(SerialNumber);

INSERT INTO Categories (CategoryName, Description) VALUES
('Electronics', 'General electronics and gadgets'),
('Appliances', 'Home appliances'),
('Furniture', 'Home and office furniture'),
('Kitchen', 'Kitchen utensils and appliances'),
('Computer & Accessories', 'Computers, laptops, and accessories'),
('Mobile & Gadgets', 'Smartphones and small gadgets'),
('Entertainment', 'TVs, audio systems, gaming consoles'),
('Other', 'Other miscellaneous items');

DECLARE @PasswordHash NVARCHAR(255) = UPPER(CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', CAST('Test@123' AS VARBINARY(MAX))), 2));

INSERT INTO Users (FullName, Email, PasswordHash) VALUES
('Rahul Sharma', 'rahul@email.com', @PasswordHash),
('Priya Patel', 'priya@email.com', @PasswordHash),
('Amit Kumar', 'amit@email.com', @PasswordHash);

INSERT INTO Sellers (SellerName) VALUES
('Amazon India'),
('Flipkart'),
('Croma'),
('Reliance Digital'),
('Local Electronics Shop');

-- Insert 20 Items for UserID=1
-- Electronics (1), Appliances (2), Furniture (3), Kitchen (4), Computer & Accessories (5), Mobile & Gadgets (6), Entertainment (7)
INSERT INTO InventoryItems (UserID, CategoryID, SellerID, ProductName, Brand, ModelNumber, SerialNumber, PurchaseDate, PurchasePrice, Location, ItemCondition, WarrantyStartDate, WarrantyExpiryDate) VALUES
(1, 1, 1, 'Samsung Smart TV 55"', 'Samsung', 'UA55', 'SN123456', '2024-01-15', 45000, 'Living Room', 'New', '2024-01-15', '2027-01-15'),
(1, 2, 3, 'LG Refrigerator 260L', 'LG', 'GL260', 'SN234567', '2022-05-10', 28500, 'Kitchen', 'Good', '2022-05-10', '2025-05-10'),
(1, 2, 4, 'Whirlpool Washing Machine', 'Whirlpool', 'WM70', 'SN345678', '2021-08-20', 22000, 'Bathroom', 'Good', '2021-08-20', '2023-08-20'),
(1, 5, 1, 'HP Laptop 15s', 'HP', '15S-DU', 'SN456789', '2023-11-05', 65000, 'Office', 'New', '2023-11-05', '2026-11-05'),
(1, 5, 2, 'Dell Monitor 24"', 'Dell', 'P2419H', 'SN567890', '2023-12-10', 15500, 'Office', 'New', '2023-12-10', '2026-12-10'),
(1, 6, 1, 'Apple iPhone 15', 'Apple', 'A3090', 'SN678901', '2025-09-01', 79900, 'Bedroom', 'New', '2025-09-01', '2026-09-01'),
(1, 7, 3, 'Sony PlayStation 5', 'Sony', 'CFI-1200', 'SN789012', '2024-06-25', 49990, 'Living Room', 'Excellent', '2024-06-25', '2025-06-25'),
(1, 4, 1, 'Philips Microwave 23L', 'Philips', 'PM23', 'SN890123', '2022-11-11', 8500, 'Kitchen', 'Good', NULL, NULL),
(1, 4, 5, 'Bajaj Mixer Grinder', 'Bajaj', 'GX1', 'SN901234', '2023-03-15', 4200, 'Kitchen', 'Good', '2023-03-15', '2025-03-15'),
(1, 2, 4, 'LG Air Conditioner 1.5T', 'LG', 'LS-Q18', 'SN012345', '2025-08-25', 38000, 'Bedroom', 'New', '2025-08-25', '2026-09-25'),
(1, 7, 1, 'Samsung Soundbar', 'Samsung', 'HW-B670', 'SN112233', '2023-07-20', 12500, 'Living Room', 'Good', '2023-07-20', '2024-07-20'),
(1, 3, 5, 'Godrej Wardrobe', 'Godrej', 'GW100', 'SN223344', '2021-12-01', 25000, 'Bedroom', 'Excellent', '2021-12-01', '2026-12-01'),
(1, 3, 2, 'Ikea Study Table', 'Ikea', 'MICKE', 'SN334455', '2022-02-14', 8900, 'Office', 'Good', NULL, NULL),
(1, 4, 3, 'Prestige Induction Cooktop', 'Prestige', 'PIC20', 'SN445566', '2023-09-10', 3200, 'Kitchen', 'Good', '2023-09-10', '2024-09-10'),
(1, 5, 1, 'Canon Printer', 'Canon', 'G3000', 'SN556677', '2024-04-18', 7800, 'Office', 'Good', '2024-04-18', '2025-04-18'),
(1, 2, 2, 'Xiaomi Air Purifier', 'Xiaomi', 'Mi3', 'SN667788', '2023-10-30', 9500, 'Living Room', 'New', '2023-10-30', '2024-10-30'),
(1, 7, 1, 'Boat Bluetooth Speaker', 'Boat', 'Stone 1000', 'SN778899', '2025-01-15', 2500, 'Bedroom', 'New', NULL, NULL),
(1, 6, 4, 'Samsung Galaxy Tab', 'Samsung', 'Tab A8', 'SN889900', '2025-08-20', 28000, 'Living Room', 'New', '2025-08-20', '2026-10-15'),
(1, 2, 5, 'Havells Ceiling Fan', 'Havells', 'Enticer', 'SN990011', '2022-06-05', 3800, 'Bedroom', 'Good', NULL, NULL),
(1, 3, 2, 'Wooden Bookshelf', 'Unknown', 'WB1', 'SN001122', '2021-03-12', 6500, 'Office', 'Good', NULL, NULL);

-- 8 active (expiry > 2026-09-18)
INSERT INTO Warranties (ItemID, WarrantyType, ProviderName, StartDate, ExpiryDate, WarrantyPeriodMonths, WarrantyTerms, ContactInformation) VALUES
(1, 'Extended', 'Samsung Care+', '2024-01-15', '2027-01-15', 36, 'Covers panel and electronics', '1800-123-4567'),
(4, 'Manufacturer', 'HP Support', '2023-11-05', '2026-11-05', 36, 'Complete cover', '1800-234-5678'),
(5, 'Manufacturer', 'Dell India', '2023-12-10', '2026-12-10', 36, 'Onsite service', '1800-345-6789'),
(6, 'Manufacturer', 'Apple Care', '2025-09-01', '2026-09-01', 12, 'Limited warranty', '1800-456-7890'),
(12, 'Seller', 'Godrej', '2021-12-01', '2026-12-01', 60, 'Wood quality and hinges', '1800-567-8901'),
(7, 'Extended', 'Sony', '2024-06-25', '2027-06-25', 36, 'Console and controller', '1800-678-9012'),
(9, 'Manufacturer', 'Bajaj Care', '2023-03-15', '2027-03-15', 48, 'Motor warranty', '1800-789-0123'),
(15, 'Manufacturer', 'Canon India', '2024-04-18', '2027-04-18', 36, 'Print head not covered', '1800-890-1234');

-- 4 expiring soon (within 30 days from 2026-09-18)
INSERT INTO Warranties (ItemID, WarrantyType, ProviderName, StartDate, ExpiryDate, WarrantyPeriodMonths, WarrantyTerms, ContactInformation) VALUES
(10, 'Manufacturer', 'LG Support', '2025-08-25', '2026-09-25', 13, 'Comprehensive', '1800-901-2345'),
(18, 'Extended', 'Samsung Care+', '2025-08-20', '2026-10-15', 14, 'Screen protection', '1800-123-4567'),
(2, 'Manufacturer', 'LG India', '2022-05-10', '2026-10-10', 53, 'Compressor only', '1800-901-2345'),
(16, 'Seller', 'Xiaomi India', '2023-10-30', '2026-09-30', 35, 'Filter not covered', '1800-012-3456');

-- 3 expired
INSERT INTO Warranties (ItemID, WarrantyType, ProviderName, StartDate, ExpiryDate, WarrantyPeriodMonths, WarrantyTerms, ContactInformation) VALUES
(3, 'Manufacturer', 'Whirlpool India', '2021-08-20', '2023-08-20', 24, 'Complete machine', '1800-112-2233'),
(11, 'Manufacturer', 'Samsung', '2023-07-20', '2024-07-20', 12, 'Audio components', '1800-223-3344'),
(14, 'Manufacturer', 'Prestige', '2023-09-10', '2024-09-10', 12, 'Coil replacement', '1800-334-4455');
