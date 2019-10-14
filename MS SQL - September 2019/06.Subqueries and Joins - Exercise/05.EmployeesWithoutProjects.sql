SELECT TOP (3)
	  e.EmployeeID,
	  e.FirstName	
FROM Employees As e
LEFT JOIN EmployeesProjects AS ep
	ON ep.EmployeeID = e.EmployeeID 
LEFT JOIN Projects AS p
	ON p.ProjectID = ep.ProjectID
	WHERE p.ProjectID IS NULL
ORDER BY e.EmployeeID