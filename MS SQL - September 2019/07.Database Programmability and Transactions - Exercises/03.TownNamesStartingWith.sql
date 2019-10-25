CREATE PROC usp_GetTownsStartingWith (@String VARCHAR(10))
AS 
SELECT t.Name
FROM Towns AS t
WHERE LEFT(t.Name, LEN(@String)) = @String

--EXEC usp_GetTownsStartingWith 'b'