using AspServer.Models;
using Dm.filter.log;
using Microsoft.Win32;

namespace AspServer.Controllers;
public class Validator
{
    private static bool EnglishAlphaNumericUnderline(char ch)
    {
        return
            ('a' <= ch && ch <= 'z') ||
            ('A' <= ch && ch <= 'Z') ||
            ('0' <= ch && ch <= '9') ||
            ch == '_';
    }

    public static bool LoginModel(LoginCS login)
    {
        return
            Username(login.username) &&
            Password(login.password);
    }

    public static bool RegisterModel(RegisterCS register)
    {
        return
            Username(register.username) &&
            Password(register.password);
    }

    public static bool Username(string username)
    {
        return
            username.Length >= 5 && 
            username.Length <= 32 && 
            username.All(EnglishAlphaNumericUnderline);
    }

    public static bool Password(string password)
    {
        return
            password.Length >= 8 &&
            password.Length <= 32 &&
            password.All(EnglishAlphaNumericUnderline);
    }

    public static bool Signature(string signature)
    {
        return
            signature.Length <= 32;
    }
}

