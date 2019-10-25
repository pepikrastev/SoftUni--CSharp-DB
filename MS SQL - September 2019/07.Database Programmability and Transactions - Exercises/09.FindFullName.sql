CREATE PROC usp_GetHoldersFullName 
AS
SELECT ah.FirstName + ' ' + ah.LastName AS [FullName]
FROM AccountHolders AS ah

--EXEC usp_GetHoldersFullName