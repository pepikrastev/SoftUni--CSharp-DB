CREATE  FUNCTION udf_CalculateTickets(@origin VARCHAR(30), @destination VARCHAR(30), @peopleCount INT) 
RETURNS VARCHAR(50)
AS
BEGIN
	IF(@peopleCount <= 0)
	BEGIN
		RETURN 'Invalid people count!'
	END

	DECLARE @flightId INT = (SELECT /* TOP(1) */ f.Id
							 FROM Flights AS f
							 --JOIN Tickets AS t ON t.FlightId = f.Id
							 WHERE f.Destination =  @destination AND
								   f.Origin = @origin)
	IF(@flightId IS NULL)
	BEGIN
		RETURN 'Invalid flight!'
	END

	DECLARE @totalPrice DECIMAL(18, 2) = (SELECT /* TOP(1) */ t.Price
										  FROM Tickets AS t
										  WHERE t.FlightId = @flightId) * @peopleCount

	RETURN 'Total price ' + CAST(@totalPrice AS VARCHAR(18))
	--RETURN CONCAT('Total price ', @totalPrice)
END

--SELECT dbo.udf_CalculateTickets('Kolyshley','Rancabolang', 33)
--SELECT dbo.udf_CalculateTickets('Kolyshley','Rancabolang', -1)
--SELECT dbo.udf_CalculateTickets('Invalid','Rancabolang', 33)