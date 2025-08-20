using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Network controller. This should be the main part of communication.
/// </summary>
public class NetworkController : MonoBehaviour
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
