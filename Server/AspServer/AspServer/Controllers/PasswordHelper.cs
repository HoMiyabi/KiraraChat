using System.Security.Cryptography;
using System.Text;

namespace AspServer.Controllers;

public class PasswordHelper
{
    public static string GetPasswordSaltSha512Hex(string password, string saltHex)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] saltHexBytes = Encoding.UTF8.GetBytes(saltHex);
        byte[] passwordSaltBytes = new byte[passwordBytes.Length + saltHexBytes.Length];
        Buffer.BlockCopy(passwordBytes, 0, passwordSaltBytes, 0, passwordBytes.Length);
        Buffer.BlockCopy(saltHexBytes, 0, passwordSaltBytes, passwordBytes.Length, saltHexBytes.Length);
        byte[] passwordSaltSha512Bytes = SHA512.HashData(passwordSaltBytes);
        string passwordSaltSha512Hex = Convert.ToHexString(passwordSaltSha512Bytes);

        return passwordSaltSha512Hex;
    }

    public static (string saltHex, string passwordSaltSha512Hex) SaltSha512Hex(string password)
    {
        string saltHex = Guid.NewGuid().ToString("N");
        string passwordSaltSha512Hex = GetPasswordSaltSha512Hex(password, saltHex);
        return (saltHex, passwordSaltSha512Hex);
    }

    public static bool ValidatePassword(string password, string saltHex, string storedPasswordSaltSha512Hex)
    {
        return GetPasswordSaltSha512Hex(password, saltHex) == storedPasswordSaltSha512Hex;
    }

    public static string GenSaltHex()
    {
        return Guid.NewGuid().ToString("N");
    }
}