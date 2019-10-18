CREATE PROC usp_ExcludeFromSchool(@StudentId INT)
AS

DECLARE @currentStudentId INT = (SELECT Id FROM Students
								WHERE Id = @StudentId)

IF(@currentStudentId IS NULL)
	BEGIN
	 RAISERROR('This school has no student with the provided id!', 16, 1)
      RETURN
	END

DELETE StudentsTeachers
WHERE StudentId = @currentStudentId

DELETE StudentsSubjects
WHERE StudentId = @currentStudentId

DELETE StudentsExams
WHERE StudentId = @currentStudentId

DELETE Students
WHERE Id = @currentStudentId

--EXEC usp_ExcludeFromSchool 1
--SELECT COUNT(*) FROM Students


