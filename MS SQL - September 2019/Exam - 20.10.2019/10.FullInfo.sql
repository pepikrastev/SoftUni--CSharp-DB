SELECT ISNULL(e.FirstName + ' ' + 
		 e.LastName, 'None') AS Employee,
		 ISNULL(d.Name, 'None'),
		 ISNULL(c.Name, 'None'),
		 ISNULL(r.[Description], 'None'),
		 ISNULL( FORMAT(r.OpenDate, 'dd.MM.yyyy', 'no'), 'None'),
		 ISNULL(s.Label, 'None'),
		 ISNULL(u.Name, 'None')
FROM Reports AS r
LEFT JOIN Employees AS e ON r.EmployeeId =e.Id
LEFT JOIN Departments AS d ON e.DepartmentId = d.Id
LEFT JOIN Categories AS c ON r.CategoryId = c.Id
LEFT JOIN [Status] AS s ON r.StatusId = s.Id
LEFT JOIN Users AS u ON r.UserId = u.Id
ORDER BY e.FirstName DESC,
		 e.LastName DESC,
		 d.Name,
		 c.Name,
		 r.[Description],
		 r.OpenDate,
		 s.Label,
		 u.Username
