CREATE PROC usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT)
AS
BEGIN
	DECLARE @repoDepartmentId INT = (SELECT c.DepartmentId FROM  Reports AS r
								JOIN Categories AS c ON r.CategoryId = c.Id
								WHERE r.Id =  @ReportId)
	
	DECLARE @employeeDepartmentId INT = (SELECT e.DepartmentId FROM Employees AS e
							WHERE e.Id =  @EmployeeId)


	IF(@repoDepartmentId = @employeeDepartmentId)
	BEGIN
	
		UPDATE Reports
		SET EmployeeId = @EmployeeId
		WHERE Id = @ReportId
	END
	ELSE
	BEGIN
		 RAISERROR('Employee doesn''t belong to the appropriate department!', 16, 1)
	END
	

END


--EXEC usp_AssignEmployeeToReport 17, 2