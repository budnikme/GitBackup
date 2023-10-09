using GitBackup.Business.Core.GitHub.Models;
using GitBackup.Business.Core.GitHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitBackup.Web.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RepositoriesController : ControllerBase
{
    private readonly IGitHubService gitHubService;

    public RepositoriesController(IGitHubService gitHubService)
    {
        this.gitHubService = gitHubService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RepositoryModel>>> Get()
    {
        var repositories = await gitHubService.GetPrivateRepositories();
        var result = repositories.Select(r => new RepositoryModel
        {
            Id = r.Id,
            Name = r.Name
        });
        return Ok(result);
    }

}
