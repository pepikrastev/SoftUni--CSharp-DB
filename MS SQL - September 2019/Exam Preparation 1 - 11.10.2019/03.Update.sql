--UPDATE Tickets
--SET Price  += Price * 0.13
--WHERE FlightId = 41

UPDATE Tickets
SET Price  *= 1.13
WHERE FlightId IN (SELECT Id FROM Flights
					WHERE Destination = 'Carlsbad')