SELECT e.DepartmentId ,
	   SUM(e.Salary) AS [MinimumSalary]
FROM Employees AS e
GROUP BY e.DepartmentId 