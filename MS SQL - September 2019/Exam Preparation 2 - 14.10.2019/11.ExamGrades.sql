CREATE FUNCTION udf_ExamGradesToUpdate(@studentId INT, @grade DECIMAL(3, 2))
RETURNS VARCHAR(100)
AS
BEGIN

	DECLARE @currentGrade INT = (SELECT Id FROM Students
								 WHERE Id =  @studentId)
	
	IF(@currentGrade IS NULL)
	BEGIN
		RETURN 'The student with provided id does not exist in the school!'
	END

	IF(@grade > 6)
	BEGIN
		RETURN 'Grade cannot be above 6.00!'
	END
		
	DECLARE @countOfGrades INT = (SELECT COUNT(Grade) FROM StudentsExams
								WHERE StudentId = @studentId
								AND Grade BETWEEN @grade AND @grade + 0.5)

 RETURN CONCAT('You have to update ', @countOfGrades, ' grades for the student ', (SELECT FirstName FROM Students WHERE Id = @studentId))
END

--SELECT dbo.udf_ExamGradesToUpdate(121, 5.50)
--SELECT dbo.udf_ExamGradesToUpdate(12, 5.50)