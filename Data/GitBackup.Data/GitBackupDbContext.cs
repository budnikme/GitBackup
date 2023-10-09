using GitBackup.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GitBackup.Data;

public class GitBackupDbContext : DbContext
{
    public GitBackupDbContext(DbContextOptions<GitBackupDbContext> options)
        : base(options)
    {
    }

    public DbSet<Backup> Backups { get; set; } = default!;
}
