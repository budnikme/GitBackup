using GitBackup.Business.Core.Repositories.Models;

namespace GitBackup.Business.Core.Repositories.Services;

public interface IRepositoryService
{
    Task<IEnumerable<RepositoryModel>> GetRepositories();
}
