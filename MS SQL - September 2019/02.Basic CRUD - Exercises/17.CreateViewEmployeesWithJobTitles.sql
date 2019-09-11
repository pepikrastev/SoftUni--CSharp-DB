  
--CREATE VIEW V_EmployeeNameJobTitle AS
--	 SELECT e.FirstName + ' ' + ISNULL(e.MiddleName, '') + ' ' + e.LastName
--	     AS [Full Name], e.JobTitle
--	   FROM Employees AS e

CREATE VIEW V_EmployeeNameJobTitle AS
SELECT FirstName + ' ' + ISNULL (MiddleName, '') + ' ' + LastName AS [Full Name], JobTitle 
FROM Employees

--SELECT * FROM V_EmployeeNameJobTitle