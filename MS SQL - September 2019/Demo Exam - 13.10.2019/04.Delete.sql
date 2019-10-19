DELETE Issues
WHERE RepositoryId IN (SELECT Id FROM Repositories
					   WHERE Name = 'Softuni-Teamwork')

DELETE RepositoriesContributors
WHERE RepositoryId IN (SELECT Id FROM Repositories
					   WHERE Name = 'Softuni-Teamwork')