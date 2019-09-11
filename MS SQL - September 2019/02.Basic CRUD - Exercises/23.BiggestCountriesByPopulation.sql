--EXEC sp_changedbowner 'sa'

--  SELECT TOP 30
--		 c.CountryName,
--		 c.Population
--    FROM Countries AS c
--   WHERE c.ContinentCode = 'EU'
--ORDER BY 
--         c.Population DESC,
--         c.CountryName

SELECT TOP (30) CountryName, [Population] 
FROM Countries
WHERE ContinentCode = 'EU'
ORDER BY [Population] DESC, CountryName ASC