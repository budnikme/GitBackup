using GitBackup.Business.Core.GitHub.Services;
using GitBackup.Common.Utilities.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGitHubService, GitHubService>();

builder.Services.AddOptions<GitHubSettings>()
    .Bind(builder.Configuration.GetSection(nameof(GitHubSettings)))
    .ValidateDataAnnotations();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
