using GitBackup.Business.Core.Repositories.Models;
using GitBackup.Business.Core.Repositories.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitBackup.Web.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RepositoriesController : ControllerBase
{
    private readonly IRepositoryService repositoryService;

    public RepositoriesController(IRepositoryService repositoryService)
    {
        this.repositoryService = repositoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RepositoryModel>>> GetRepositories()
    {
        var repositories = await repositoryService.GetRepositories();
        return Ok(repositories);
    }
}
