﻿namespace GitBackup.Business.Core.Repositories.Models;

public class RepositoryModel
{
    public required long Id { get; set; }

    public required string Name { get; set; }

    public DateTime? LastBackupDate { get; set; }
}
