-- Database: smartmeterdb

-- DROP DATABASE IF EXISTS smartmeterdb;

CREATE DATABASE smartmeterdb
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'English_India.1252'
    LC_CTYPE = 'English_India.1252'
    LOCALE_PROVIDER = 'libc'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;


CREATE TABLE "User" (
  UserId         BIGSERIAL PRIMARY KEY,
  Username       VARCHAR(100) NOT NULL UNIQUE,
  PasswordHash   BYTEA NOT NULL,
  DisplayName    VARCHAR(150) NOT NULL,
  Email          VARCHAR(200) NULL,
  Phone          VARCHAR(30) NULL,
  LastLoginUtc   TIMESTAMP(3) NULL,
  IsActive       BOOLEAN NOT NULL DEFAULT TRUE
);
select * from "User";
CREATE TABLE OrgUnit (
  OrgUnitId SERIAL PRIMARY KEY,
  Type VARCHAR(20) NOT NULL CHECK (Type IN ('Zone','Substation','Feeder','DTR')), -- Zone>Substation>Feeder>DTR'  -- Add Index on type
  Name VARCHAR(100) NOT NULL,
  ParentId INT NULL REFERENCES OrgUnit(OrgUnitId)
);
CREATE INDEX IX_OrgUnit_Type_Including ON OrgUnit(Type) INCLUDE (Name, ParentId);
Select * from OrgUnit;
INSERT INTO OrgUnit (Type, Name, ParentId) 
VALUES 
('Substation', 'Downtown Substation', 1),
('Feeder', 'Main Feeder Line', 2),
('DTR', 'Transformer #A-125', 1);



CREATE TABLE Tariff (
  TariffId SERIAL PRIMARY KEY,
  Name VARCHAR(100) NOT NULL,
  EffectiveFrom DATE NOT NULL,
  EffectiveTo DATE NULL,    --Effective from<Effective To
  BaseRate DECIMAL(18,4) NOT NULL,                                            
  TaxRate DECIMAL(18,4) NOT NULL DEFAULT 0
);

INSERT INTO Tariff (Name, EffectiveFrom, EffectiveTo, BaseRate, TaxRate) 
VALUES 
('Premium Business', '2024-01-01', '2024-12-31', 25.5000, 2.2500),
('Economy Basic', '2024-03-01', '2024-04-11', 12.7500, 1.0200);

ALTER TABLE Tariff 
ADD CONSTRAINT CHK_EffectiveDate_Range CHECK (EffectiveTo IS NULL OR EffectiveFrom < EffectiveTo),
ADD CONSTRAINT CHK_BaseRate_Positive CHECK (BaseRate > 0);
select * from Tariff


-- During what time u are using what ammount of electricity
CREATE TABLE TodRule (                                      
  TodRuleId      SERIAL PRIMARY KEY,
  TariffId       INT NOT NULL REFERENCES Tariff(TariffId),
  Name           VARCHAR(50) NOT NULL,
  StartTime      TIME(0) NOT NULL, -- Peak hours
  EndTime        TIME(0) NOT NULL, 
  RatePerKwh     DECIMAL(18,6) NOT NULL
);
ALTER TABLE TodRule
ADD CONSTRAINT CHK_TodRule_TimeRange CHECK (EndTime > StartTime);
CREATE INDEX IX_TodRule_Name ON TodRule(Name);
ALTER TABLE TodRule 
ADD COLUMN Deleted BOOLEAN NOT NULL DEFAULT FALSE;


CREATE TABLE TariffSlab (
  TariffSlabId   SERIAL PRIMARY KEY,
  TariffId       INT NOT NULL REFERENCES Tariff(TariffId),
  FromKwh        DECIMAL(18,6) NOT NULL,
  ToKwh          DECIMAL(18,6) NOT NULL,
  RatePerKwh     DECIMAL(18,6) NOT NULL,
  CONSTRAINT CK_TariffSlab_Range CHECK (FromKwh >= 0 AND ToKwh > FromKwh)
);
ALTER TABLE TariffSlab 
ADD COLUMN Deleted BOOLEAN NOT NULL DEFAULT FALSE;
select * from TariffSlab
-- New table Address
-- AId, HouseNo, Lane/Locality, City, State, Pincode
CREATE TABLE Address (
  AId BIGSERIAL PRIMARY KEY,
  HouseNo VARCHAR(50) NOT NULL,
  LaneLocality VARCHAR(200) NOT NULL,
  City VARCHAR(100) NOT NULL,
  State VARCHAR(100) NOT NULL,
  Pincode VARCHAR(20) NOT NULL
);
select * from Address
INSERT INTO Address (HouseNo, LaneLocality, City, State, Pincode) 
VALUES 
('123', 'Main Street, Downtown', 'Mangaluru', 'Karnataka', '10001'),
('456', 'Oak Avenue, Riverside Colony', 'Jamshedpur', 'Jharkhand', '90210');

CREATE TABLE Consumer (
  ConsumerId BIGSERIAL PRIMARY KEY,
  Name VARCHAR(200) NOT NULL,
  AId BIGINT NULL REFERENCES Address(AId),
  Phone VARCHAR(30) NULL,
  Email VARCHAR(200) NULL,
  OrgUnitId INT NOT NULL REFERENCES OrgUnit(OrgUnitId),
  TariffId INT NOT NULL REFERENCES Tariff(TariffId),
  Status VARCHAR(20) NOT NULL DEFAULT 'Active' CHECK (Status IN ('Active','Inactive')),
  CreatedAt TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CreatedBy VARCHAR(100) NOT NULL DEFAULT 'system',
  UpdatedAt TIMESTAMP(3) NULL,
  UpdatedBy VARCHAR(100) NULL,
  Deleted BOOLEAN NOT NULL DEFAULT FALSE
);
select * from Consumer

INSERT INTO Consumer (Name, AId, Phone, Email, OrgUnitId, TariffId, Status) 
VALUES 
('Mantu Kumar', 1, '+91-8763478729', 'mantu.kumar@email.com', 4, 1, 'Active'),
('Sahil Khan', 2, '+91-9845082934', 'sahil.khan@email.com', 3, 2, 'Active'),
('Shreyansh Sohane', 1, '+91-9865932934', 'Shreyansh@email.com', 4, 1, 'Inactive');


CREATE TABLE Meter (
  MeterSerialNo VARCHAR(50) NOT NULL PRIMARY KEY,
  IpAddress VARCHAR(45) NOT NULL,
  ICCID VARCHAR(30) NOT NULL, -- Integrated circuit card identifier(ICCID)
  IMSI VARCHAR(30) NOT NULL, -- International mobile subscriber identifier(IMSI)
  Manufacturer VARCHAR(100) NOT NULL,
  Firmware VARCHAR(50) NULL,
  Category VARCHAR(50) NOT NULL,
  InstallTsUtc TIMESTAMP(3) NOT NULL,
  Status VARCHAR(20) NOT NULL DEFAULT 'Active'
           CHECK (Status IN ('Active','Inactive','Decommissioned')),
  ConsumerId BIGINT NULL REFERENCES Consumer(ConsumerId)
);
INSERT INTO Meter (MeterSerialNo, IpAddress, ICCID, IMSI, Manufacturer, Firmware, Category, InstallTsUtc, Status, ConsumerId) 
VALUES 
('MET-2025-001', '192.168.1.101', '89100423481105572219', '310150123456789', 'Schneider Electric', 'v2.1.5', 'Smart Meter', '2024-01-15 10:30:00', 'Active', 1),
('MET-2025-002', '192.168.1.102', '89100423481105572220', '310150123456790', 'Siemens', 'v3.0.2', 'Industrial Meter', '2024-02-20 14:15:00', 'Active', 2),
('MET-2025-003', '192.168.1.103', '89100423481105572221', '310150123456791', 'ABB', 'v1.8.3', 'Residential Meter', '2023-12-10 09:45:00', 'Inactive', 3);

INSERT INTO Meter (MeterSerialNo, IpAddress, ICCID, IMSI, Manufacturer, Firmware, Category, InstallTsUtc, Status, ConsumerId) 
VALUES 
('MET-2025-004', '192.168.1.104', '89100423574505572219', '310150123346789', 'Mini Electric', 'v2.1.6', 'Smart Meter', '2023-01-15 10:30:00', 'Active', 4)

select * from Meter;

-- A person is paying bill timely and also tempering the smart meter??

-- One more table Arrears -----
-- AId, ConsumerId, AType, PaidStatus, BId

CREATE TABLE Arrears (
  AId BIGSERIAL PRIMARY KEY,
  ConsumerId BIGINT NOT NULL REFERENCES Consumer(ConsumerId),
  AType VARCHAR(50) NOT NULL CHECK (AType IN ('Overdue', 'Penalty', 'Interest')),
  PaidStatus VARCHAR(20) NOT NULL DEFAULT 'Unpaid' CHECK (PaidStatus IN ('Paid', 'Unpaid', 'Disconnected')),
  BillId BIGINT NOT NULL REFERENCES Billing(BillId)
);
ALTER TABLE Arrears 
ALTER COLUMN BillId SET DATA TYPE INT;
-- After 2 to 4 months delay of overdue, penalty will be there

-- -- One more table Bill -----
-- BId, BDate, BAmmount, MetId, TarrifDetailId, PaymentDate, DueDate, CreatedAt, PreviousReading, PreviousReadingDate, CurrentReadingDate, CurrentReading, LoadFactor, PowerFactor, DisconnectedDate

CREATE TABLE Billing (
    BillId          SERIAL PRIMARY KEY,
    ConsumerId      BIGINT NOT NULL REFERENCES Consumer(ConsumerId) ON DELETE CASCADE,
    MeterId         VARCHAR(50) NOT NULL REFERENCES Meter(MeterSerialNo) ON DELETE CASCADE,
    BillingPeriodStart DATE NOT NULL,
    BillingPeriodEnd   DATE NOT NULL,
    TotalUnitsConsumed NUMERIC(18,6) NOT NULL,
    BaseAmount      NUMERIC(18,4) NOT NULL,
    TaxAmount       NUMERIC(18,4) NOT NULL DEFAULT 0,
    TotalAmount     NUMERIC(18,4) GENERATED ALWAYS AS (BaseAmount + TaxAmount) STORED,
    GeneratedAt     TIMESTAMP(3) WITH TIME ZONE NOT NULL DEFAULT NOW(),
    DueDate         DATE NOT NULL,
    PaidDate        TIMESTAMP(3) WITH TIME ZONE,
    PaymentStatus   VARCHAR(20) NOT NULL DEFAULT 'Unpaid'
                    CHECK (PaymentStatus IN ('Unpaid', 'Paid')),
	DisconnectionDate TIMESTAMP(3) WITH TIME ZONE
);

 select * from Billing;
 -- One more table TariffDetails -----
-- Id, TariffId, TariffSlabId, TariffTodId
CREATE TABLE TariffDetails (
  TariffDetailsId BIGSERIAL PRIMARY KEY,
  TariffId INT NOT NULL REFERENCES Tariff(TariffId),
  TariffSlabId INT NOT NULL REFERENCES TariffSlab(TariffSlabId),
  TariffTodId INT NOT NULL REFERENCES TodRule(TodRuleId)
);

-- One more table MeterReading that will store meter readings detail after every 15/30 minutes --
-- MeterReadingId, MeterReadingDate(DateTime)->Default system date, MeterReadingIntervels->fixed value, EnergyConsumed->Not Null, MeterId, Current->Not Null, Voltage->Not Null
CREATE TABLE MeterReading (
    MeterReadingId SERIAL PRIMARY KEY,
    MeterId        VARCHAR(50) NOT NULL REFERENCES Meter(MeterSerialNo),
    MeterReadingDate   TIMESTAMP(3) WITH TIME ZONE NOT NULL,
    EnergyConsumed NUMERIC(18,6),
    Voltage        NUMERIC(10,3) NOT NULL,
    Current        NUMERIC(10,3) NOT NULL,   
	 constraint ck_Energy check(EnergyConsumed>=0)
);

-- Insert sample meter readings
INSERT INTO MeterReading (MeterId, MeterReadingDate, EnergyConsumed, Voltage, Current)
VALUES 
('MET-2025-002', '2023-12-31 23:59:59', 1000, 220, 10),  -- Previous reading
('MET-2025-002', '2024-01-15 12:00:00', 1200, 220, 10),  -- Mid-month reading
('MET-2025-002', '2024-01-31 23:59:59', 1500, 220, 10);  -- End-of-month reading (current)


select * from MeterReading
select * from Billing

-- CREATE TABLE Consumer (
--   ConsumerId BIGSERIAL PRIMARY KEY,
--   Name VARCHAR(200) NOT NULL,
--   Address VARCHAR(500) NULL,
--   Phone VARCHAR(30) NULL,
--   Email VARCHAR(200) NULL,
--   OrgUnitId INT NOT NULL REFERENCES OrgUnit(OrgUnitId),
--   TariffId INT NOT NULL REFERENCES Tariff(TariffId),
--   Status VARCHAR(20) NOT NULL DEFAULT 'Active' CHECK (Status IN ('Active','Inactive')),
--   CreatedAt TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP,
--   CreatedBy VARCHAR(100) NOT NULL DEFAULT 'system',
--   UpdatedAt TIMESTAMP(3) NULL,
--   UpdatedBy VARCHAR(100) NULL
-- );
-- ALTER TABLE Consumer 
-- ADD COLUMN Deleted BOOLEAN NOT NULL DEFAULT FALSE;

-- ALTER TABLE Consumer 
-- ADD COLUMN AId BIGINT NULL REFERENCES Address(AId)
-- ALTER TABLE Consumer DROP COLUMN Address;
