SELECT p.Name,
	   p.Seats,
	   COUNT(t.PassengerId) AS [Passengers Count]
FROM Planes AS p
LEFT JOIN Flights AS f ON f.PlaneId = p.Id
LEFT JOIN Tickets AS t ON t.FlightId = f.Id
GROUP BY p.Name, p.Seats
ORDER BY [Passengers Count] DESC,
		 p.Name,
		 p.Seats