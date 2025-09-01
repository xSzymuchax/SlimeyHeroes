using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Network helper. Contains server adress and user's token.
/// </summary>
public class NetworkHelper : MonoBehaviour
{
    private static string _serverAdress = "https://localhost:3000";
    private static string _token;
    public static string ServerAdress { get => _serverAdress; private set => _serverAdress = value; }

    public static string TokenJWT { get => _token; private set => _token = value; }

    public static void SetToken(string token)
    {
        _token = token;
    }
}
