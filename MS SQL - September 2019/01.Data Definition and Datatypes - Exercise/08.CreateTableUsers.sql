CREATE TABLE Users (
	Id BIGINT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) UNIQUE NOT NULL,
	[Password] VARCHAR(26) NOT NULL,
	ProfilePicture VARBINARY(MAX),
	CHECK(DATALENGTH(ProfilePicture) <= 921600),
	LastLoginTime DATETIME2,
	IsDeleted BIT 
)

INSERT INTO Users
 (Username, [Password], ProfilePicture, LastLoginTime, IsDeleted)
 VALUES 
 ('Pesho', '123', NULL, NULL, 0),
 ('Gosho', '123', NULL, NULL, 0),
 ('Ivan', '123', NULL, NULL, 0),
 ('Test', '123', NULL, NULL, 1),
 ('SecondTest', '123', NULL, NULL, 1)

