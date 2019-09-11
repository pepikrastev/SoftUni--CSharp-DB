--SELECT FirstName, LastName 
--FROM Employees
--WHERE DepartmentID > 4 OR DepartmentID < 4

--SELECT FirstName, LastName 
--FROM Employees
--WHERE DepartmentID != 4

SELECT FirstName, LastName 
FROM Employees
WHERE DepartmentID <> 4

--SELECT FirstName, LastName 
--FROM Employees
--WHERE DepartmentID NOT IN(4)