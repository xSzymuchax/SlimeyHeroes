using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data model for login request.
/// </summary>
[System.Serializable]
public class LoginRequest
{
    public string email;
    public string password;

    public LoginRequest(string _email, string _password)
    {
        email = _email;
        password = _password;
    }
}