SELECT FlightId AS [FlightId], 
		SUM(Price) AS [Price]
FROM Tickets
GROUP BY  FlightId
ORDER BY SUM(Price) DESC, 
		 FlightId