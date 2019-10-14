SELECT e.FirstName,
	   e.LastName,
	   e.HireDate,
	   d.Name AS [DeptName]
FROM Employees AS e
	JOIN Departments AS d
	ON d.DepartmentID = e.DepartmentID
	AND e.HireDate > '1991-01-01'
	AND d.Name IN('Sales', 'Finance')
ORDER BY e.HireDate