--CREATE /*OR ALTER*/ PROC usp_CancelFlights
--AS
--UPDATE Flights 
-- SET DepartureTime = NULL,
--     ArrivalTime = NULL
--WHERE ArrivalTime > DepartureTime


CREATE PROC usp_CancelFlights
AS
UPDATE Flights 
 SET DepartureTime = NULL,
     ArrivalTime = NULL
WHERE DATEDIFF(SECOND, DepartureTime, ArrivalTime) > 0
