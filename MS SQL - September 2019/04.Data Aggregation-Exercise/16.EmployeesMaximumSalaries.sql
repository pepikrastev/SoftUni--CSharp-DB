SELECT e.DepartmentId,
	   MAX(e.Salary) AS [MaxSalary]
FROM Employees AS e
GROUP BY e.DepartmentId
HAVING MAX(e.Salary) NOT BETWEEN 30000 AND 70000