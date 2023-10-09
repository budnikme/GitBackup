using System.ComponentModel.DataAnnotations.Schema;

namespace GitBackup.Data.Models;

public class Backup
{
    public int Id { get; set; }

    [Column("REPOSITORY_ID")]
    public required long RepositoryId { get; set; }

    [Column("REPOSITORY_NAME")]
    public required string RepositoryName { get; set; }

    [Column("DATE")]
    public DateTime Date { get; set; }
}
