namespace ClarusAI.Core.Interfaces;

public interface IEncryptionService
{
    Task<string> EncryptAsync(string plainText, string key);
    Task<string> DecryptAsync(string cipherText, string key);
    Task<byte[]> EncryptFileAsync(byte[] fileData, string key);
    Task<byte[]> DecryptFileAsync(byte[] encryptedData, string key);
    string GenerateKey();
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}