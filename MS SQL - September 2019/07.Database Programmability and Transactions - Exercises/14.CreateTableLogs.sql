CREATE TABLE Logs (
	LogId INT  IDENTITY(1, 1),
	AccountId INT NOT NULL,
	OldSum DECIMAL(18,2) ,
	NewSum DECIMAL(18,2) 

	CONSTRAINT PK_Logs PRIMARY KEY (LogId),
	CONSTRAINT FK_Logs_Accounts FOREIGN KEY (AccountId) REFERENCES Accounts(Id)
)
GO
-- FOR SUBMIT
CREATE TRIGGER tr_UpdateAccountInfo
ON Accounts
AFTER UPDATE
AS
	BEGIN 
	INSERT INTO Logs
	SELECT i.AccountHolderId,
		    d.Balance,
            i.Balance 
	FROM inserted AS i
	JOIN deleted AS d ON d.AccountHolderId = i.AccountHolderId
END