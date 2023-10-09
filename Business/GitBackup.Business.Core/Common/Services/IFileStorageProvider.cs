namespace GitBackup.Business.Core.Common.Services;

public interface IFileStorageProvider
{
    void CreateFile(string name, string data);

    string ReadFromFile(string name);
}
