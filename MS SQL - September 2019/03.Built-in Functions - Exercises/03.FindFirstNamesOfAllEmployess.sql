SELECT FirstName 
 FROM Employees
WHERE DepartmentId = 3 or DepartmentId = 10
 AND YEAR(HireDate) BETWEEN 1995 AND 2005

 