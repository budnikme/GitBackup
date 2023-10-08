using Octokit;

namespace GitBackup.Business.Core.GitHub.Services;

public interface IGitHubService
{
    Task<IEnumerable<Repository>> GetPrivateRepositories();

    Task<IReadOnlyList<Issue>?> GetIssuesForRepository(long repositoryId);

    Task<Repository> CreateRepository(string name);

    Task<Issue> CreateIssue(long repositoryId, string title, string body);
}
