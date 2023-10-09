namespace GitBackup.Web.Api;

public static class BuilderHelper
{
    public static void ConfigureOptions<T>(WebApplicationBuilder builder) where T : class
    {
        builder.Services.AddOptions<T>()
            .BindConfiguration(typeof(T).Name)
            .ValidateDataAnnotations();
    }
}
