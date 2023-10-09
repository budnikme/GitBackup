using Octokit;

namespace GitBackup.Business.Core.GitHub.Services;

public interface IGitHubService
{
    Task<IEnumerable<Repository>> GetPrivateRepositories();

    Task<IEnumerable<Issue>> GetIssuesForRepository(long repositoryId);

    Task<string> GetRepositoryNameById(long repositoryId);

    Task<Repository> CreatePrivateRepository(string name);

    Task<Issue> CreateIssue(long repositoryId, string title, string? body);
}
