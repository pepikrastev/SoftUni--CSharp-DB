SELECT TOP (5) e.EmployeeID,
	   e.FirstName,
	   e.Salary,
	   d.Name AS [DepartmentName]
FROM Employees AS e
	JOIN Departments As d 
	ON e.DepartmentID = d.DepartmentID 
	AND e.Salary > 15000 -- WHERE e.Salary > 15000
ORDER BY e.DepartmentID