SELECT TOP(10)
	   e.FirstName,
	   e.LastName,
	   e.DepartmentId
FROM Employees AS e
WHERE e.Salary > (SELECT AVG(em.Salary)
					FROM Employees AS em
					WHERE e.DepartmentId = em.DepartmentId
				 )
ORDER BY e.DepartmentId