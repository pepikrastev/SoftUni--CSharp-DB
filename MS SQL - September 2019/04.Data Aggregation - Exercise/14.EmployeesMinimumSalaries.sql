SELECT e.DepartmentId ,
	   MIN(e.Salary) AS [MinimumSalary]
FROM Employees AS e
WHERE e.DepartmentId IN (2, 5, 7) 
AND HireDate > '01/01/2000'
GROUP BY e.DepartmentId 

