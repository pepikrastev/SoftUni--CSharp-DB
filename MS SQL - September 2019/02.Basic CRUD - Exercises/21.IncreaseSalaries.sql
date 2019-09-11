--UPDATE Employees
--   SET Salary += Salary * 0.12
-- WHERE DepartmentID IN (1, 2, 4, 11)

--SELECT e.Salary
--  FROM Employees AS e

 UPDATE Employees
 SET Salary *= 1.12
 WHERE DepartmentID IN 
 (SELECT DepartmentID FROM Departments WHERE Name IN
 ('Engineering', 'Tool Design', 'Marketing', 'Information Services'))

SELECT Salary
  FROM Employees

  --  UPDATE Employees
--   SET Salary *= 1.12
-- WHERE DepartmentID IN (1, 2, 4, 11)

--SELECT Salary
--  FROM Employees