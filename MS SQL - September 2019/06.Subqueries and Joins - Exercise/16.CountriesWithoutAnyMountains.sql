SELECT COUNT(*) - COUNT(mc.MountainId) AS [Count]
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc
	ON mc.CountryCode = c.CountryCode
