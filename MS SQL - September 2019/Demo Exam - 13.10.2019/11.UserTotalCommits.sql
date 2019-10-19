CREATE FUNCTION udf_UserTotalCommits(@username VARCHAR(30)) 
RETURNS INT
AS
BEGIN
	DECLARE @count INT = (SELECT COUNT(c.Id) FROM Users AS u
						  JOIN Commits AS c ON c.ContributorId = u.Id
						  WHERE u.Username = @username)
						  
	RETURN @count
END