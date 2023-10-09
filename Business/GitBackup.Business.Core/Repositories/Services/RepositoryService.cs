using GitBackup.Business.Core.Backups.Services;
using GitBackup.Business.Core.GitHub.Services;
using GitBackup.Business.Core.Repositories.Models;

namespace GitBackup.Business.Core.Repositories.Services;

public class RepositoryService : IRepositoryService
{
    private readonly IGitHubService gitHubService;
    private readonly IBackupService backupService;

    public RepositoryService(IGitHubService gitHubService, IBackupService backupService)
    {
        this.gitHubService = gitHubService;
        this.backupService = backupService;
    }

    public async Task<IEnumerable<RepositoryModel>> GetRepositories()
    {
        var repositories = await GetPrivateRepositories();
        var repositoryIds = repositories.Select(r => r.Id);
        var backups = await backupService.GetBackedUpRepositoriesByIds(repositoryIds);

        return backups.UnionBy(repositories, r => r.Id)
            .OrderByDescending(r => r.LastBackupDate)
            .ToList();
    }

    private async Task<List<RepositoryModel>> GetPrivateRepositories()
    {
        var repositories = await gitHubService.GetPrivateRepositories();
        return repositories.Select(r => new RepositoryModel
        {
            Id = r.Id,
            Name = r.Name
        }).ToList();
    }
}
