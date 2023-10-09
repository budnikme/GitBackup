using GitBackup.Common.Utilities;

namespace GitBackup.Business.Core.Common.Services;

public class FileSystemProvider : IFileStorageProvider
{
    public void CreateFile(string name, string data)
    {
        var path = Path.Combine(GetTempFolderPath(), name);
        File.WriteAllText(path, data);
    }

    public string ReadFromFile(string name)
    {
        var path = Path.Combine(GetTempFolderPath(), name);
        return File.ReadAllText(path);
    }

    private static string GetTempFolderPath()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), Constants.TempFolder);
        EnsureFolderExists(path);

        return path;
    }

    private static void EnsureFolderExists(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}
