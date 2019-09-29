SELECT Username,
	   SUBSTRING(Email, CHARINDEX('@', Email) + 1, LEN(Email)) AS [Emile Provider]
 FROM Users
 ORDER BY [Emile Provider],
		  Username