CREATE DATABASE CarRental
    GO
   USE CarRental

CREATE TABLE Categories (
	Id INT PRIMARY KEY IDENTITY,
	CategoryName NVARCHAR(50) NOT NULL,
	DailyRate INT NOT NULL,
	WeeklyRate INT NOT NULL,
	MonthlyRate INT NOT NULL,
	WeekendRate INT NOT NULL
)

CREATE TABLE Cars (
       Id INT IDENTITY(1, 1),
       PlateNumber NVARCHAR(10) NOT NULL,
       Manufacturer NVARCHAR(20) NOT NULL,
       Model NVARCHAR(20) NOT NULL,
       CarYear INT NOT NULL,
       CategoryId INT,
       Doors INT NOT NULL,
       Picture VARBINARY(MAX),
       Condition NVARCHAR(MAX),
       Available BIT NOT NULL,
    
       CONSTRAINT PK_Cars
       PRIMARY KEY (Id),
       
       CONSTRAINT FK_Cars_Categories
       FOREIGN KEY (CategoryId)
       REFERENCES Categories(Id)
)

CREATE TABLE Employees (
       Id INT IDENTITY(1, 1),
       FirstName NVARCHAR(50) NOT NULL,
       LastName NVARCHAR(50) NOT NULL,
       Title NVARCHAR(50) NOT NULL,
       Notes NVARCHAR(MAX),
    
       CONSTRAINT PK_Employees
       PRIMARY KEY (Id)
)

CREATE TABLE Customers (
       Id INT IDENTITY(1, 1),
       DriverLicenseNumber INT NOT NULL,
       FullName NVARCHAR(100) NOT NULL,
       Adress NVARCHAR(100) NOT NULL,
       City NVARCHAR(50) NOT NULL,
       ZIPCode INT NOT NULL,
       Notes NVARCHAR(MAX),
    
       CONSTRAINT PK_Customers
       PRIMARY KEY (Id)
)

CREATE TABLE RentalOrders (
       Id INT IDENTITY(1, 1),
       EmployeeId INT,
       CustomerId INT,
       CarId INT FOREIGN KEY REFERENCES Cars(Id),
       TankLevel INT NOT NULL,
       KilometrageStart INT NOT NULL,
       KilometrageEnd INT NOT NULL,
       TotalKilometrage AS KilometrageEnd - KilometrageStart,
       StartDate DATE NOT NULL,
       EndDate DATE NOT NULL,
       TotalDays AS DATEDIFF(DAY, StartDate, EndDate),
       RateApplied DECIMAL(9, 2),
       TaxRate DECIMAL(9, 2),
       OrderStatus NVARCHAR(50),
       Notes NVARCHAR(MAX),
    
       CONSTRAINT PK_RentalOrders
       PRIMARY KEY (Id),
    
       CONSTRAINT FK_RentalOrders_Employees
       FOREIGN KEY (EmployeeId)
       REFERENCES Employees(Id),
    
       CONSTRAINT FK_RentalOrders_Customers
       FOREIGN KEY (CustomerId)
       REFERENCES Customers(Id),
)

INSERT INTO Categories VALUES
('Limousine', 65, 350, 1350, 120),
('SUV', 85, 500, 1800, 160),
('Economic', 40, 230, 850, 70)

INSERT INTO Cars (PlateNumber, Manufacturer, Model, CarYear, CategoryId, Doors, Picture, Condition, Available) 
       VALUES ('123456ABCD', 'Mazda', 'CX-5', 2016, 1, 5, 123456, 'Perfect', 1),
              ('asdafof145', 'Audi', 'A6 Allroad', 2017, 2, 3, 99999, 'Perfect', 1),
              ('asdp230456', 'BMW', 'X7', 2019, 3, 2, 123456, 'Perfect', 1)

INSERT INTO Employees (FirstName, LastName, Title, Notes) 
       VALUES ('Ivan', 'Ivanov', 'Seller', 'I am Ivan'),
              ('Georgi', 'Georgiev', 'Seller', 'I am Gosho'),
              ('Dimitar', 'Mitkov', 'Manager', 'I am Mitko')

INSERT INTO Customers (DriverLicenseNumber, FullName, Adress, City, ZIPCode, Notes)
       VALUES (123456789, 'Gogo Gogov', '??. ???????? 5', '?????', 1233, 'Good driver'),
              (347645231, 'Mara Mareva', '??. ???? ?????? 14', '?????', 5678, 'Bad driver'),
              (123574322, 'Strahil Strahilov', '??. ?????? 4', '???????', 5689, 'Good driver')

INSERT INTO RentalOrders (EmployeeId, CustomerId, CarId, TankLevel, KilometrageStart, KilometrageEnd, StartDate, EndDate) 
       VALUES (1, 1, 1, 54, 2189, 2456, '2017-11-05', '2017-11-08'),
              (2, 2, 2, 22, 13565, 14258, '2017-11-06', '2017-11-11'),
              (3, 3, 3, 180, 1202, 1964, '2017-11-09', '2017-11-12')