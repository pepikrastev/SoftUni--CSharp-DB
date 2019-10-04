	CREATE TABLE Passports (
		PassportID INT NOT NULL IDENTITY(101, 1),
		PassportNumber CHAR(8) NOT NULL

		CONSTRAINT PK_Passports PRIMARY KEY (PassportID)
	)

	CREATE TABLE Persons (
		PersonID INT NOT NULL IDENTITY(1, 1),
		FirstName NVARCHAR(30) NOT NULL,
		Salary DECIMAL(8,2) NOT NULL,
		PassportID INT NOT NULL,

		CONSTRAINT PK_Persons PRIMARY KEY(PersonID),

		CONSTRAINT FK_Persons_Passports FOREIGN KEY (PassportID) REFERENCES Passports(PassportID)
	)

	INSERT INTO Passports
	VALUES ('N34FG21B'),
		   ('K65LO4R7'),
		   ('ZE657QP2')

	INSERT INTO Persons
	VALUES('Roberto', 43300.00, 102),
		  ('Tom', 56100, 103),
		  ('Yana', 60200.00, 101)

--SELECT * FROM Persons as p
--JOIN Passports as pa
--ON p.PassportID = pa.PassportID