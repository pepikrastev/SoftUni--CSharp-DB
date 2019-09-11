--  SELECT TOP 7 
--		  e.FirstName, 
--		  e.LastName, 
--		  e.HireDate 
--    FROM Employees AS e
--ORDER BY e.HireDate DESC

 SELECT TOP (7) FirstName, LastName, HireDate 
    FROM Employees 
ORDER BY HireDate DESC