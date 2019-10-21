CREATE FUNCTION udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
RETURNS INT 
AS
BEGIN
	IF(@StartDate IS NULL)
	BEGIN
		RETURN 0
	END

	IF(@EndDate IS NULL)
	BEGIN
		RETURN 0
	END

	DECLARE @totalHours INT = DATEDIFF(HOUR, @StartDate, @EndDate)

	RETURN  @totalHours
END