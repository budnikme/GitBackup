namespace GitBackup.Business.Core.Backups.Models;

public class IssueModel
{
    public required string Title { get; set; }

    public string? Body { get; set; }
}
