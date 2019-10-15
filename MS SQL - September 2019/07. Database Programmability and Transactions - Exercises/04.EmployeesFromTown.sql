CREATE PROC usp_GetEmployeesFromTown (@Town VARCHAR(10))
AS
SELECT e.FirstName,
	   e.LastName 
FROM Employees AS e
JOIN Addresses AS a ON e.AddressID = a.AddressID
JOIN Towns AS t ON t.TownID = a.TownID
WHERE t.Name = @Town

--EXEC usp_GetEmployeesFromTown 'Sofia'