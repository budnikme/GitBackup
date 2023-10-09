using System.Reflection;
using GitBackup.Business.Core.Backups.Services;
using GitBackup.Business.Core.Common.Services;
using GitBackup.Business.Core.Encryption.Services;
using GitBackup.Business.Core.GitHub.Services;
using GitBackup.Business.Core.Repositories.Services;
using GitBackup.Common.Utilities.Settings;
using GitBackup.Data;
using GitBackup.Web.Api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var migrationAssembly = typeof(GitBackupDbContext).GetTypeInfo().Assembly.GetName().Name;
builder.Services.AddDbContext<GitBackupDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(migrationAssembly)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGitHubService, GitHubService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IFileStorageProvider, FileSystemProvider>();
builder.Services.AddScoped<IBackupService, BackupService>();
builder.Services.AddScoped<IRepositoryService, RepositoryService>();

BuilderHelper.ConfigureOptions<GitHubSettings>(builder);
BuilderHelper.ConfigureOptions<EncryptionSettings>(builder);

const string corsPolicyName = "default";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<GitBackupDbContext>();

    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();
app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
