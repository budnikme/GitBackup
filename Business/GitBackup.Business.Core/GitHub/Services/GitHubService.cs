using GitBackup.Common.Utilities;
using GitBackup.Common.Utilities.Settings;
using Microsoft.Extensions.Options;
using Octokit;

namespace GitBackup.Business.Core.GitHub.Services;

public class GitHubService : IGitHubService
{
    private readonly GitHubClient client;

    public GitHubService(IOptions<GitHubSettings> settings)
    {
        client = new GitHubClient(new ProductHeaderValue(Constants.GitHubApplicationName))
        {
            Credentials = new Credentials(settings.Value.AccessToken)
        };
    }

    public async Task<IEnumerable<Repository>> GetPrivateRepositories()
    {
        var repositories = await client.Repository.GetAllForCurrent();
        return repositories.Where(s => s.Private);
    }

    public async Task<IEnumerable<Issue>> GetIssuesForRepository(long repositoryId)
    {
        return await client.Issue.GetAllForRepository(repositoryId);
    }

    public async Task<string> GetRepositoryNameById(long repositoryId)
    {
        var repository = await client.Repository.Get(repositoryId);
        return repository.Name;
    }

    public async Task<Repository> CreatePrivateRepository(string name)
    {
        var repository = new NewRepository(name) { Private = true };
        return await client.Repository.Create(repository);
    }

    public async Task<Issue> CreateIssue(long repositoryId, string title, string? body)
    {
        var issue = new NewIssue(title) { Body = body };
        return await client.Issue.Create(repositoryId, issue);
    }
}
