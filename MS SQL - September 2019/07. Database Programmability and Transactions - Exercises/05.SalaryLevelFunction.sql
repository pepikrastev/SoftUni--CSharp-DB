CREATE FUNCTION ufn_GetSalaryLevel (@Salary DECIMAL(18,4)) 
RETURNS NVARCHAR(7)
AS
BEGIN
	DECLARE @SalaryLevel NVARCHAR(7)
	SET @SalaryLevel =
		CASE 
			WHEN @Salary < 30000 THEN 'Low'
			WHEN @Salary BETWEEN 30000 AND 50000 THEN 'Average'
			ELSE 'High'
		END
	RETURN @SalaryLevel
END

--SELECT e.FirstName, e.Salary, dbo.ufn_GetSalaryLevel(e.Salary) AS salary  FROM Employees AS e