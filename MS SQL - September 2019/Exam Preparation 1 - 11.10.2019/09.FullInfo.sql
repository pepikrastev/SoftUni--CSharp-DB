SELECT p.FirstName + ' ' + p.LastName AS [Full Name],
	   pl.Name,
	   f.Origin + ' - ' + f.Destination AS [Trip],
	   lt.[Type]
FROM Passengers AS p
JOIN Tickets AS t ON p.Id = t.PassengerId
JOIN Flights AS f ON t.FlightId = f.Id
JOIN Luggages AS l ON l.Id = t.LuggageId
JOIN LuggageTypes AS lt ON lt.Id = l.LuggageTypeId
JOIN Planes AS pl ON pl.Id = f.PlaneId
ORDER BY [Full Name],
		 pl.Name,
		 f.Origin,
		 f.Destination,
		 lt.Type