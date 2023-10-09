using GitBackup.Business.Core.Backups.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitBackup.Web.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BackupsController : ControllerBase
{
    private readonly IBackupService backupService;

    public BackupsController(IBackupService backupService)
    {
        this.backupService = backupService;
    }

    [HttpPost("{repositoryId:long}")]
    public async Task<ActionResult> CreateBackup(long repositoryId)
    {
        await backupService.CreateBackup(repositoryId);
        return Ok();
    }

    [HttpPost("{repositoryId:long}/restore")]
    public async Task<ActionResult> RestoreBackup(long repositoryId)
    {
        try
        {
            await backupService.RestoreBackup(repositoryId);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }

        return Ok();
    }
}
