using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviour
{
    public static NetworkController Instance;
    private static string _serverAdress = "https://localhost:3000";
    public static string ServerAdress { get => _serverAdress; private set => _serverAdress = value; }

}
