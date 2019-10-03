SELECT DISTINCT e.DepartmentId,
	   e.Salary
FROM (
      SELECT DepartmentId, Salary, DENSE_RANK() OVER(PARTITION BY DepartmentId ORDER BY Salary DESC) AS RangSalary
      FROM Employees 
     ) AS e
WHERE e.RangSalary = 3