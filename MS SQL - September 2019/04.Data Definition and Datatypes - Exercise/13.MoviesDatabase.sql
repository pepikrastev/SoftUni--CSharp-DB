CREATE DATABASE Movies

USE Movies

GO

CREATE TABLE Directors(
	Id INT PRIMARY KEY IDENTITY,
	DirectorName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
) 

CREATE TABLE Genres (
       Id INT IDENTITY(1, 1),
       GenreName NVARCHAR(50) NOT NULL,
       Notes NVARCHAR(50),

       CONSTRAINT PK_Genres
       PRIMARY KEY (Id)
)

CREATE TABLE Categories (
       Id INT IDENTITY(1, 1),
       CategoryName NVARCHAR(50) NOT NULL,
       Notes NVARCHAR(50)

       CONSTRAINT PK_Categories
       PRIMARY KEY (Id)
)

CREATE TABLE Movies (
       Id INT IDENTITY(1, 1),
       Title NVARCHAR(50) NOT NULL,
       DirectorId INT,
       CopyrightYear INT NOT NULL,
       Length INT NOT NULL,
       GenreId INT,
       CategoryId INT,
       Rating DECIMAL(3, 1) NOT NULL,
       Notes NVARCHAR(MAX),

       CONSTRAINT PK_Moveis
       PRIMARY KEY (Id),

       CONSTRAINT FK_Movies_Directors
       FOREIGN KEY (DirectorId)
       REFERENCES Directors(Id),

       CONSTRAINT FK_Movies_Genres
       FOREIGN KEY (GenreId)
       REFERENCES Genres(Id),

       CONSTRAINT FK_Movies_Categories
       FOREIGN KEY (CategoryId)
       REFERENCES Categories(Id)
)

INSERT INTO Directors (DirectorName, Notes)
	 VALUES ('First Director', 'First Note'),
		    ('Second Director', 'Second Note'), 
		    ('Third Director', 'Third Note'), 	
		    ('Four Director', 'Four Note'), 
		    ('Five Director', 'Five Note') 	  
			
INSERT INTO Genres(GenreName, Notes)
	  VALUES ('First GenreName', 'First Note'),
		    ('Second GenreName', 'Second Note'), 
		    ('Third GenreName', 'Third Note'), 	
		    ('Four GenreName', 'Four Note'), 
		    ('Five GenreName', 'Five Note')	
			
INSERT INTO Categories (CategoryName, Notes)
	  VALUES ('First CategoryName', 'First Note'),
		    ('Second CategoryName', 'Second Note'), 
		    ('Third CategoryName', 'Third Note'), 	
		    ('Four CategoryName', 'Four Note'), 
		    ('Five CategoryName', 'Five Note')
			
INSERT INTO Movies (Title, DirectorId, CopyrightYear, Length, GenreId, CategoryId, Rating, Notes) 
	  VALUES ('First Title', 1, 1928, 25, 1, 1, 12.2, 'First Note'),
		    ('Second Title', 2, 2019, 125, 4, 2, 5.2, 'Second Note'),
		    ('Third Title', 3, 2000, 215, 3, 2, 8.5, 'Third Note'),	
		    ('Four Title', 4, 1991, 251, 2, 1, 12.3, 'Four Note'),	
		    ('Five Title', 5, 1988, 235, 1, 1, 13.2, 'Five Note')
			
--SELECT * FROM Movies								  			  				  										  			  				  										  			  				  											  			  				  