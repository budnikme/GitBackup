using GitBackup.Business.Core.Repositories.Models;

namespace GitBackup.Business.Core.Backups.Services;

public interface IBackupService
{
    Task CreateBackup(long repositoryId);

    Task RestoreBackup(long repositoryId);

    Task<IEnumerable<RepositoryModel>> GetBackedUpRepositoriesByIds(IEnumerable<long> repositoryIds);
}
