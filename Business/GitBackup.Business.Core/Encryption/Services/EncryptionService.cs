using System.Text.Json;
using Crypto.AES;
using GitBackup.Common.Utilities.Settings;
using Microsoft.Extensions.Options;

namespace GitBackup.Business.Core.Encryption.Services;

public class EncryptionService : IEncryptionService
{
    private readonly string key;

    public EncryptionService(IOptions<EncryptionSettings> encryptionSettings)
    {
        key = encryptionSettings.Value.Key;
    }

    public string Encrypt<T>(T obj)
    {
        var json = JsonSerializer.Serialize(obj);
        return AES.EncryptString(key, json);
    }

    public T? Decrypt<T>(string cipherText)
    {
        var json = AES.DecryptString(key, cipherText);
        return JsonSerializer.Deserialize<T>(json);
    }
}
