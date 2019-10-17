SELECT p.FirstName,
	   p.LastName,
	   p.Age 
FROM Passengers AS p
WHERE p.Id NOT IN (SELECT t.PassengerId 
					FROM Tickets AS t)
ORDER BY p.Age DESC,
		 p.FirstName,
		 p.LastName