--CREATE VIEW v_EmployeesSalaries AS
--	 SELECT e.FirstName,
--			e.LastName,
--			e.Salary
--	   FROM Employees AS e

CREATE VIEW v_EmployeesSalaries AS
SELECT FirstName, LastName, Salary 
FROM Employees

--SELECT * FROM v_EmployeesSalaries 