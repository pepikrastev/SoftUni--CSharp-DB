SELECT *
 INTO NewTable
 FROM Employees AS e
 WHERE e.Salary > 30000

DELETE 
FROM NewTable 
WHERE ManagerID = 42

UPDATE NewTable
SET Salary += 5000
WHERE DepartmentId = 1

SELECT nt.DepartmentId,
	   AVG(nt.Salary) AS [AverageSalary]
FROM NewTable AS nt
GROUP BY nt.DepartmentId
