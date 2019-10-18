DELETE StudentsTeachers
WHERE TeacherId IN (SELECT Id FROM Teachers
				   WHERE Phone LIKE '%72%')

--DELETE Teachers
--WHERE Phone LIKE '%72%'

DELETE Teachers
WHERE CHARINDEX('72', Phone) > 0