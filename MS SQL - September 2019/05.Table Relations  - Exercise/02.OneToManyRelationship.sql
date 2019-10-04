CREATE TABLE Manufacturers (
	ManufacturerID INT NOT NULL IDENTITY(1,1),
	Name NVARCHAR(20) NOT NULL,
	EstablishedOn DATE NOT NULL,

	CONSTRAINT PK_Manufacturers PRIMARY KEY(ManufacturerID)
)

CREATE TABLE Models (
	ModelID INT NOT NULL IDENTITY(1, 1),
	Name NVARCHAR(20) NOT NULL,
	ManufacturerID INT NOT NULL,

	CONSTRAINT PK_Models PRIMARY KEY (ModelID),

	CONSTRAINT FK_Models_Manufacturers FOREIGN KEY (ManufacturerID) REFERENCES Manufacturers(ManufacturerID)
)

INSERT INTO Manufacturers
VALUES ('BMW', '07/03/1916'),
	   ('Tesla', '01/01/2003'),
	   ('Lada', '01/05/1966')

INSERT INTO Models
VALUES('X1', 1),
	  ('i6', 1),
	  ('Model S', 2),
	  ('Model X', 2),
	  ('Model 3', 2),
	  ('Nova', 3)



