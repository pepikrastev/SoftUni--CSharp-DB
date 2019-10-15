CREATE PROC usp_GetHoldersWithBalanceHigherThan (@Amount  DECIMAL(18, 2))
AS
SELECT ah.FirstName,
	   ah.LastName
FROM AccountHolders AS ah
JOIN Accounts AS a ON a.AccountHolderId = ah.Id
GROUP BY ah.FirstName, 
		 ah.LastName
HAVING SUM(a.Balance) > @Amount
ORDER BY ah.FirstName

--EXEC usp_GetHoldersWithBalanceHigherThan 20000