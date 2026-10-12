-- Drop existing foreign keys and tables if any exist
IF OBJECT_ID('Warranties', 'U') IS NOT NULL DROP TABLE Warranties;
IF OBJECT_ID('InventoryItems', 'U') IS NOT NULL DROP TABLE InventoryItems;
IF OBJECT_ID('Sellers', 'U') IS NOT NULL DROP TABLE Sellers;
IF OBJECT_ID('Categories', 'U') IS NOT NULL DROP TABLE Categories;
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;

-- Create Users table
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);

-- Create Categories table
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255),
    IsActive BIT DEFAULT 1
);

-- Create Sellers table
CREATE TABLE Sellers (
    SellerID INT IDENTITY(1,1) PRIMARY KEY,
    SellerName NVARCHAR(100) NOT NULL,
    ContactNumber NVARCHAR(20),
    Email NVARCHAR(100),
    Address NVARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- Create InventoryItems table
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

-- Create Warranties table
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

-- Indexes
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_InventoryItems_UserID ON InventoryItems(UserID);
CREATE INDEX IX_InventoryItems_CategoryID ON InventoryItems(CategoryID);
CREATE INDEX IX_InventoryItems_PurchaseDate ON InventoryItems(PurchaseDate);
CREATE INDEX IX_InventoryItems_WarrantyExpiryDate ON InventoryItems(WarrantyExpiryDate);
CREATE INDEX IX_InventoryItems_SerialNumber ON InventoryItems(SerialNumber);

-- Seed Categories
INSERT INTO Categories (CategoryName, Description) VALUES
('Electronics', 'General electronics, televisions, and gadgets'),
('Appliances', 'Major and small home appliances'),
('Furniture', 'Living room, bedroom, and office furniture'),
('Kitchen', 'Kitchen utensils, cooktops, and appliances'),
('Computer & Accessories', 'Computers, laptops, monitors, and peripherals'),
('Mobile & Gadgets', 'Smartphones, tablets, and wearable devices'),
('Entertainment', 'Audio systems, gaming consoles, and accessories'),
('Other', 'Miscellaneous household items');

-- Seed Users
DECLARE @PasswordHash NVARCHAR(255) = UPPER(CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', CAST('Test@123' AS VARBINARY(MAX))), 2));

INSERT INTO Users (FullName, Email, PasswordHash, CreatedDate, IsActive) VALUES
('Rahul Sharma', 'rahul@email.com', @PasswordHash, '2026-09-10 10:30:00', 1),
('Priya Patel', 'priya@email.com', @PasswordHash, '2026-09-12 14:15:00', 1),
('Amit Kumar', 'amit@email.com', @PasswordHash, '2026-09-15 09:45:00', 1);

-- Seed Sellers
INSERT INTO Sellers (SellerName, ContactNumber, Email, Address, CreatedDate) VALUES
('Amazon India', '1800-3000-9009', 'support@amazon.in', 'Brigade Gateway, 26/1 Dr. Rajkumar Road, Bangalore', '2026-09-01 11:00:00'),
('Flipkart', '1800-208-9898', 'cs@flipkart.com', 'Buildings Alyssa, Begonia & Clove, Embassy Tech Village, Bangalore', '2026-09-01 11:30:00'),
('Croma', '1800-572-7662', 'customersupport@croma.com', 'Infinity Retail Ltd, Malad West, Mumbai', '2026-09-02 10:00:00'),
('Reliance Digital', '1800-889-1055', 'reliancedigital@ril.com', 'Reliance Retail Ltd, Ghansoli, Navi Mumbai', '2026-09-02 12:00:00'),
('Vijay Sales', '1800-267-9977', 'support@vijaysales.com', 'Harman House, Prabhadevi, Mumbai', '2026-09-03 15:00:00');

-- Seed InventoryItems (20 items for UserID=1)
INSERT INTO InventoryItems (UserID, CategoryID, SellerID, ProductName, Brand, ModelNumber, SerialNumber, PurchaseDate, PurchasePrice, Location, ItemCondition, Description, WarrantyStartDate, WarrantyExpiryDate, CreatedDate, UpdatedDate, IsDeleted) VALUES
(1, 1, 1, 'Samsung Smart TV 55"', 'Samsung', 'UA55CUE60', 'SN-SAM-55-9821', '2024-01-15', 45000.00, 'Living Room', 'New', '55 inch Crystal 4K UHD Smart TV with HDR10+', '2024-01-15', '2027-01-15', '2026-09-10 11:00:00', '2026-09-10 11:00:00', 0),
(1, 2, 3, 'LG Refrigerator 260L', 'LG', 'GL-S292RDSY', 'SN-LG-REF-7712', '2022-05-10', 28500.00, 'Kitchen', 'Good', 'Double door frost free inverter refrigerator with smart diagnosis', '2022-05-10', '2026-10-10', '2026-09-10 11:15:00', '2026-09-10 11:15:00', 0),
(1, 2, 4, 'Whirlpool Washing Machine', 'Whirlpool', 'WM-ROYAL-70', 'SN-WP-70-1193', '2021-08-20', 22000.00, 'Balcony', 'Good', 'Fully automatic top load 7kg washing machine with 6th sense', '2021-08-20', '2023-08-20', '2026-09-10 11:30:00', '2026-09-10 11:30:00', 0),
(1, 5, 1, 'HP Laptop 15s', 'HP', '15S-DU3564TU', 'SN-HP-8832-IN', '2023-11-05', 65000.00, 'Home Office', 'New', 'Intel Core i5 12th Gen, 16GB RAM, 512GB NVMe SSD, 15.6 inch FHD', '2023-11-05', '2026-11-05', '2026-09-10 11:45:00', '2026-09-10 11:45:00', 0),
(1, 5, 2, 'Dell Monitor 24"', 'Dell', 'P2422H', 'SN-DEL-24-4401', '2023-12-10', 15500.00, 'Home Office', 'New', '24 inch IPS borderless monitor with pivot and height adjust stand', '2023-12-10', '2026-12-10', '2026-09-10 12:00:00', '2026-09-10 12:00:00', 0),
(1, 6, 1, 'Apple iPhone 15', 'Apple', 'A3090-256', 'SN-APL-IP15-66', '2025-09-01', 79900.00, 'Master Bedroom', 'New', 'Blue Titanium 256GB with dynamic island and A16 Bionic chip', '2025-09-01', '2026-09-28', '2026-09-11 09:30:00', '2026-09-11 09:30:00', 0),
(1, 7, 3, 'Sony PlayStation 5', 'Sony', 'CFI-1208A', 'SN-SNY-PS5-90', '2024-06-25', 49990.00, 'Living Room', 'Excellent', 'Standard Disc Edition with DualSense wireless controller and 825GB SSD', '2024-06-25', '2027-06-25', '2026-09-11 10:00:00', '2026-09-11 10:00:00', 0),
(1, 4, 1, 'Philips Microwave 23L', 'Philips', 'HD9252-90', 'SN-PH-MW-3301', '2022-11-11', 8500.00, 'Kitchen', 'Good', 'Digital air fryer and convection microwave oven with rapid air technology', NULL, NULL, '2026-09-11 10:30:00', '2026-09-11 10:30:00', 0),
(1, 4, 5, 'Bajaj Mixer Grinder', 'Bajaj', 'GX-1-PRO', 'SN-BJ-MIX-4412', '2023-03-15', 4200.00, 'Kitchen', 'Good', '750 Watt copper motor mixer grinder with 3 stainless steel jars', '2023-03-15', '2027-03-15', '2026-09-11 11:00:00', '2026-09-11 11:00:00', 0),
(1, 2, 4, 'LG Air Conditioner 1.5T', 'LG', 'MS-Q18YNZA', 'SN-LG-AC-0921', '2025-08-25', 38000.00, 'Master Bedroom', 'New', '1.5 Ton 5 Star AI DUAL Inverter Split AC with anti-virus protection', '2025-08-25', '2026-09-25', '2026-09-12 10:00:00', '2026-09-12 10:00:00', 0),
(1, 7, 1, 'Samsung Soundbar', 'Samsung', 'HW-B670-XL', 'SN-SAM-SB-1102', '2023-07-20', 12500.00, 'Living Room', 'Good', '5.1 Channel soundbar with wireless subwoofer and Dolby Digital 5.1', '2023-07-20', '2024-07-20', '2026-09-12 10:30:00', '2026-09-12 10:30:00', 0),
(1, 3, 5, 'Godrej Wardrobe', 'Godrej', 'GW-SLIM-3D', 'SN-GD-WD-9943', '2021-12-01', 25000.00, 'Master Bedroom', 'Excellent', '3 door CRCA steel wardrobe with locker and full length mirror', '2021-12-01', '2026-12-01', '2026-09-12 11:00:00', '2026-09-12 11:00:00', 0),
(1, 3, 2, 'Ikea Study Table', 'Ikea', 'MICKE-DESK-W', 'SN-IK-DSK-7711', '2022-02-14', 8900.00, 'Home Office', 'Good', 'Clean white study desk with integrated cable outlet and drawer unit', NULL, NULL, '2026-09-12 11:30:00', '2026-09-12 11:30:00', 0),
(1, 4, 3, 'Prestige Induction Cooktop', 'Prestige', 'PIC-20-PRO', 'SN-PR-IC-5502', '2023-09-10', 3200.00, 'Kitchen', 'Good', '2000W induction cooktop with Indian menu preset buttons and timer', '2023-09-10', '2024-09-10', '2026-09-13 14:00:00', '2026-09-13 14:00:00', 0),
(1, 5, 1, 'Canon Printer', 'Canon', 'PIXMA-G3000', 'SN-CN-PR-8819', '2024-04-18', 11800.00, 'Home Office', 'Good', 'All-in-one wireless ink tank color printer for home and small office', '2024-04-18', '2027-04-18', '2026-09-13 14:30:00', '2026-09-13 14:30:00', 0),
(1, 2, 2, 'Xiaomi Air Purifier', 'Xiaomi', 'Mi-AirPurifier-4', 'SN-XI-AP-6623', '2023-10-30', 9500.00, 'Living Room', 'New', 'True HEPA filter air purifier with OLED touch display and app control', '2023-10-30', '2026-09-30', '2026-09-13 15:00:00', '2026-09-13 15:00:00', 0),
(1, 7, 1, 'Boat Bluetooth Speaker', 'Boat', 'Stone-1000-BLK', 'SN-BT-ST-4491', '2025-01-15', 2500.00, 'Guest Bedroom', 'New', '14W rugged portable stereo speaker with IPX5 water resistance', NULL, NULL, '2026-09-14 09:00:00', '2026-09-14 09:00:00', 0),
(1, 6, 4, 'Samsung Galaxy Tab', 'Samsung', 'Galaxy-Tab-A9', 'SN-SAM-TB-8802', '2025-08-20', 28000.00, 'Living Room', 'New', '11 inch 90Hz display, 8GB RAM, 128GB ROM, quad speakers with Dolby Atmos', '2025-08-20', '2026-10-15', '2026-09-14 09:30:00', '2026-09-14 09:30:00', 0),
(1, 2, 5, 'Havells Ceiling Fan', 'Havells', 'Stealth-Air-BLDC', 'SN-HV-FN-1130', '2022-06-05', 3800.00, 'Guest Bedroom', 'Good', '1200mm energy saving BLDC ceiling fan with remote control', NULL, NULL, '2026-09-14 10:00:00', '2026-09-14 10:00:00', 0),
(1, 3, 2, 'Wooden Bookshelf', 'Urban Ladder', 'UL-BOOK-4T', 'SN-UL-BK-5521', '2021-03-12', 6500.00, 'Home Office', 'Good', '4 tier solid Sheesham wood open bookshelf with mahogany finish', NULL, NULL, '2026-09-14 10:30:00', '2026-09-14 10:30:00', 0);

-- Seed Warranties (15 warranties linked to ItemIDs)
-- 8 Active Warranties (Expiry > 30 days from now)
INSERT INTO Warranties (ItemID, WarrantyType, ProviderName, StartDate, ExpiryDate, WarrantyPeriodMonths, WarrantyTerms, ContactInformation, CreatedDate) VALUES
(1, 'Extended', 'Samsung Care+', '2024-01-15', '2027-01-15', 36, 'Comprehensive protection covering display panel, motherboard, and sound circuit with free home pickup.', '1800-407-267864 (Toll-Free) | support@samsungcare.in', '2026-09-10 11:00:00'),
(4, 'Manufacturer', 'HP Support India', '2023-11-05', '2026-11-05', 36, 'Complete on-site warranty for hardware defects, motherboard, and keyboard. Accidental damage excluded.', '1800-258-7170 | in.support@hp.com', '2026-09-10 11:45:00'),
(5, 'Manufacturer', 'Dell India Support', '2023-12-10', '2026-12-10', 36, '3 year Next Business Day Onsite service covering panel backlight, HDMI ports, and power adapter.', '1800-425-4026 | dell_india_support@dell.com', '2026-09-10 12:00:00'),
(7, 'Extended', 'Sony PlayStation Protect', '2024-06-25', '2027-06-25', 36, 'Full console replacement coverage for HDMI IC failure, disc drive malfunction, and internal power supply.', '1800-103-7799 | sonyindia.care@sony.com', '2026-09-11 10:00:00'),
(9, 'Manufacturer', 'Bajaj Electricals Care', '2023-03-15', '2027-03-15', 48, '4 year comprehensive motor warranty and 2 years on product body against manufacturing flaws.', '1800-102-5963 | consumercare@bajajelectricals.com', '2026-09-11 11:00:00'),
(12, 'Seller', 'Godrej Interio', '2021-12-01', '2026-12-01', 60, '5 year warranty covering lock mechanism, hinges, anti-rust coating, and sheet alignment.', '1800-267-0077 | interio@godrej.com', '2026-09-12 11:00:00'),
(15, 'Manufacturer', 'Canon India Care', '2024-04-18', '2027-04-18', 36, '3 year or 30,000 prints standard on-site warranty covering logic board, scanner unit, and rollers.', '1860-180-3366 | ci-support@canon.co.in', '2026-09-13 14:30:00'),
(6, 'Manufacturer', 'AppleCare India', '2025-09-01', '2026-09-28', 12, 'Standard 1 year limited hardware warranty with global repair coverage at authorized Apple Service Centers.', '000800-1009009 | contactus.in@apple.com', '2026-09-11 09:30:00');

-- 4 Expiring Soon Warranties (Within next 30 days)
INSERT INTO Warranties (ItemID, WarrantyType, ProviderName, StartDate, ExpiryDate, WarrantyPeriodMonths, WarrantyTerms, ContactInformation, CreatedDate) VALUES
(10, 'Manufacturer', 'LG Electronics India', '2025-08-25', '2026-09-25', 13, '1 year comprehensive machine warranty and free annual maintenance service inspection.', '1800-315-9999 | serviceindia@lge.com', '2026-09-12 10:00:00'),
(16, 'Seller', 'Xiaomi India Support', '2023-10-30', '2026-09-30', 35, 'Comprehensive coverage for internal motor, air quality sensor, and WiFi antenna. Consumable HEPA filter excluded.', '1800-103-6286 | service.in@xiaomi.com', '2026-09-13 15:00:00'),
(2, 'Manufacturer', 'LG Electronics India', '2022-05-10', '2026-10-10', 53, 'Extended compressor coverage under smart inverter warranty program. Gas refill chargeable.', '1800-315-9999 | serviceindia@lge.com', '2026-09-10 11:15:00'),
(18, 'Extended', 'Samsung Care+ Mobile', '2025-08-20', '2026-10-15', 14, 'Screen damage and liquid spill protection with nominal claim processing deductible.', '1800-407-267864 | support@samsungcare.in', '2026-09-14 09:30:00');

-- 3 Expired Warranties
INSERT INTO Warranties (ItemID, WarrantyType, ProviderName, StartDate, ExpiryDate, WarrantyPeriodMonths, WarrantyTerms, ContactInformation, CreatedDate) VALUES
(3, 'Manufacturer', 'Whirlpool India Ltd', '2021-08-20', '2023-08-20', 24, '2 year comprehensive warranty covering drum, motor, and drain pump. Expired.', '1800-208-1800 | helpdeskindia@whirlpool.com', '2026-09-10 11:30:00'),
(11, 'Manufacturer', 'Samsung India Audio', '2023-07-20', '2024-07-20', 12, '1 year standard warranty on amplifier board, Bluetooth transceiver, and subwoofer driver. Expired.', '1800-407-267864 | support@samsung.com', '2026-09-12 10:30:00'),
(14, 'Manufacturer', 'Prestige TTK Care', '2023-09-10', '2024-09-10', 12, '1 year limited warranty for induction coil and control PCB against voltage fluctuations. Expired.', '1800-123-334411 | customercare@ttkprestige.com', '2026-09-13 14:00:00');
