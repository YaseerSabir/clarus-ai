using ClarusAI.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace ClarusAI.Business.Services;

public class EncryptionService : IEncryptionService
{
    private readonly IConfiguration _configuration;

    public EncryptionService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> EncryptAsync(string plainText, string key)
    {
        try
        {
            using var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(key);
            aes.GenerateIV(); // Generate random IV for security
            
            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedBytes = await Task.Run(() => encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length));
            
            // Prepend IV to encrypted data for storage
            var result = new byte[aes.IV.Length + encryptedBytes.Length];
            Array.Copy(aes.IV, 0, result, 0, aes.IV.Length);
            Array.Copy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);
            
            return Convert.ToBase64String(result);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Encryption failed", ex);
        }
    }

    public async Task<string> DecryptAsync(string cipherText, string key)
    {
        try
        {
            using var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(key);
            
            var encryptedData = Convert.FromBase64String(cipherText);
            
            // Extract IV from the beginning of encrypted data
            var iv = new byte[16];
            Array.Copy(encryptedData, 0, iv, 0, 16);
            aes.IV = iv;
            
            // Extract encrypted content
            var cipherData = new byte[encryptedData.Length - 16];
            Array.Copy(encryptedData, 16, cipherData, 0, cipherData.Length);
            
            using var decryptor = aes.CreateDecryptor();
            var decryptedBytes = await Task.Run(() => decryptor.TransformFinalBlock(cipherData, 0, cipherData.Length));
            
            return Encoding.UTF8.GetString(decryptedBytes);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Decryption failed", ex);
        }
    }

    public async Task<byte[]> EncryptFileAsync(byte[] fileData, string key)
    {
        try
        {
            using var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(key);
            aes.GenerateIV();
            
            using var encryptor = aes.CreateEncryptor();
            var encryptedData = await Task.Run(() => encryptor.TransformFinalBlock(fileData, 0, fileData.Length));
            
            // Prepend IV to encrypted data
            var result = new byte[aes.IV.Length + encryptedData.Length];
            Array.Copy(aes.IV, 0, result, 0, aes.IV.Length);
            Array.Copy(encryptedData, 0, result, aes.IV.Length, encryptedData.Length);
            
            return result;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("File encryption failed", ex);
        }
    }

    public async Task<byte[]> DecryptFileAsync(byte[] encryptedData, string key)
    {
        try
        {
            using var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(key);
            
            // Extract IV from the beginning of encrypted data
            var iv = new byte[16];
            Array.Copy(encryptedData, 0, iv, 0, 16);
            aes.IV = iv;
            
            // Extract encrypted content
            var cipherData = new byte[encryptedData.Length - 16];
            Array.Copy(encryptedData, 16, cipherData, 0, cipherData.Length);
            
            using var decryptor = aes.CreateDecryptor();
            var decryptedData = await Task.Run(() => decryptor.TransformFinalBlock(cipherData, 0, cipherData.Length));
            
            return decryptedData;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("File decryption failed", ex);
        }
    }

    public string GenerateKey()
    {
        using var aes = Aes.Create();
        aes.GenerateKey();
        return Convert.ToBase64String(aes.Key);
    }

    public string HashPassword(string password)
    {
        // BCrypt automatically handles salt generation and uses adaptive cost
        // Cost factor 12 provides good security vs performance balance
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        try
        {
            // BCrypt handles salt extraction and verification automatically
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        catch
        {
            return false;
        }
    }

}