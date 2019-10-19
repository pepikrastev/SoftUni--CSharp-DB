SELECT TOP 5 r.Id,
			r.Name,
			COUNT(c.Id) AS [Commits]
FROM Repositories AS r
JOIN RepositoriesContributors AS rc ON rc.RepositoryId = r.Id
JOIN Commits AS c ON c.RepositoryId = r.Id
GROUP BY r.Id,
		 r.Name
ORDER BY COUNT(c.RepositoryId) DESC,
		 r.Id,
		 r.Name