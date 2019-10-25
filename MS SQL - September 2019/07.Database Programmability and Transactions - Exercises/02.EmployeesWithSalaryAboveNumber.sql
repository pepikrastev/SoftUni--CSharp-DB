CREATE PROC usp_GetEmployeesSalaryAboveNumber (@Number DECIMAL(18, 4))
AS
SELECT e.FirstName,
	   e.LastName
FROM Employees AS e
WHERE e.Salary >= @Number

--EXEC usp_GetEmployeesSalaryAboveNumber 48100