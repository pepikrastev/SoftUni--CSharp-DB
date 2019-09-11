--  SELECT c.CountryName,
--         c.CountryCode,
--         CASE 
--             WHEN c.CurrencyCode = 'EUR' 
--			 THEN 'Euro'
--             ELSE 'Not Euro'
--         END AS [Currency]
--    FROM Countries AS c
--ORDER BY c.CountryName

SELECT CountryName, CountryCode,
	CASE 
	 WHEN CurrencyCode = 'EUR' THEN 'Euro'
	 ELSE 'Not Euro' 
	END 
 AS [Currency]
FROM Countries
ORDER BY CountryName