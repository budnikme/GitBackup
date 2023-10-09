namespace GitBackup.Business.Core.Encryption.Services;

public interface IEncryptionService
{
    string Encrypt<T>(T obj);

    T? Decrypt<T>(string cipherText);
}
