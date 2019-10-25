CREATE PROC usp_EmployeesBySalaryLevel (@SalaryLevel NVARCHAR(50))
AS 
BEGIN
SELECT e.FirstName,
	   e.LastName
	   --e.Salary,
	   --dbo.ufn_GetSalaryLevel(e.Salary) AS [LevelSalary]
FROM Employees AS e
WHERE dbo.ufn_GetSalaryLevel(e.Salary) = @SalaryLevel
END

--EXEC usp_EmployeesBySalaryLevel 'high'