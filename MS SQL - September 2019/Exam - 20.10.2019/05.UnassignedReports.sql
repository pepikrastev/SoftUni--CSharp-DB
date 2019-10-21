SELECT r.[Description],
		--CAST(OpenDate AS date)
		FORMAT(OpenDate, 'dd-MM-yyyy') AS [OpenDate]
FROM Reports AS r
WHERE EmployeeId IS NULL
ORDER BY r.OpenDate,
		r.[Description]