using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data model for register request.
/// </summary>
[System.Serializable]
public class RegisterRequest
{
    public string username;
    public string email;
    public string password;
    public string password2;

    public RegisterRequest(string _username, string _email, string _password, string _password2)
    {
        username = _username;
        email = _username;
        password = _username;
        password2 = _username;
    }
}