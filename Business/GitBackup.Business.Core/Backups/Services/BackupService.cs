using GitBackup.Business.Core.Backups.Models;
using GitBackup.Business.Core.Common.Services;
using GitBackup.Business.Core.Encryption.Services;
using GitBackup.Business.Core.GitHub.Services;
using GitBackup.Business.Core.Repositories.Models;
using GitBackup.Common.Utilities.Extensions;
using GitBackup.Data;
using GitBackup.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GitBackup.Business.Core.Backups.Services;

public class BackupService : IBackupService
{
    private readonly IGitHubService gitHubService;
    private readonly IEncryptionService encryptionService;
    private readonly IFileStorageProvider fileStorageProvider;
    private readonly GitBackupDbContext dbContext;

    public BackupService(
        IGitHubService gitHubService,
        IEncryptionService encryptionService,
        IFileStorageProvider fileStorageProvider,
        GitBackupDbContext dbContext)
    {
        this.gitHubService = gitHubService;
        this.encryptionService = encryptionService;
        this.fileStorageProvider = fileStorageProvider;
        this.dbContext = dbContext;
    }

    public async Task CreateBackup(long repositoryId)
    {
        var issues = await GetIssuesForRepository(repositoryId);
        var encryptedIssues = encryptionService.Encrypt(issues);
        var repositoryName = await gitHubService.GetRepositoryNameById(repositoryId);
        var currentDateTime = DateTime.UtcNow;
        var fileName = GetFileName(repositoryName, currentDateTime);
        fileStorageProvider.CreateFile(fileName, encryptedIssues);
        await SaveBackupToDatabase(repositoryId, repositoryName, currentDateTime);
    }

    public async Task RestoreBackup(long repositoryId)
    {
        var backup = await GetLatestBackupForRepository(repositoryId);
        var backupFileName = GetFileName(backup.RepositoryName, backup.Date);
        var repositoryName = $"{backup.RepositoryName}-{Guid.NewGuid()}";
        var repository = await gitHubService.CreatePrivateRepository(repositoryName);
        await RestoreIssues(repository.Id, backupFileName);
    }

    public async Task<IEnumerable<RepositoryModel>> GetBackedUpRepositoriesByIds(IEnumerable<long> repositoryIds)
    {
        return await dbContext.Backups
            .Where(b => repositoryIds.Contains(b.RepositoryId))
            .Select(b => new RepositoryModel
            {
                Name = b.RepositoryName,
                Id = b.RepositoryId,
                LastBackupDate = b.Date
            })
            .OrderByDescending(b => b.LastBackupDate)
            .ToListAsync();
    }

    private async Task RestoreIssues(long repositoryId, string backupFileName)
    {
        var encryptedIssues = fileStorageProvider.ReadFromFile(backupFileName);
        var issues = encryptionService.Decrypt<List<IssueModel>>(encryptedIssues);
        await RestoreIssuesToRepository(repositoryId, issues);
    }

    private async Task RestoreIssuesToRepository(long repositoryId, List<IssueModel>? issues)
    {
        if (issues.IsNullOrEmpty())
        {
            return;
        }

        foreach (var issue in issues!)
        {
            await gitHubService.CreateIssue(repositoryId, issue.Title, issue.Body);
        }
    }

    private async Task<Backup> GetLatestBackupForRepository(long repositoryId)
    {
        return await dbContext.Backups
            .Where(b => b.RepositoryId == repositoryId)
            .OrderByDescending(b => b.Date)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException("Backup not found");
    }

    private async Task<IEnumerable<IssueModel>> GetIssuesForRepository(long repositoryId)
    {
        var issues = await gitHubService.GetIssuesForRepository(repositoryId);
        return issues.Select(i => new IssueModel
        {
            Title = i.Title,
            Body = i.Body
        });
    }

    private async Task SaveBackupToDatabase(long repositoryId, string repositoryName, DateTime date)
    {
        var backup = new Backup
        {
            RepositoryName = repositoryName,
            RepositoryId = repositoryId,
            Date = date
        };
        dbContext.Backups.Add(backup);
        await dbContext.SaveChangesAsync();
    }

    private static string GetFileName(string repositoryName, DateTime currentDateTime)
    {
        return $"{repositoryName}-{currentDateTime:yyyy-MM-dd_HH-mm-ss}";
    }
}
