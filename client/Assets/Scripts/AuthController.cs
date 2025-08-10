using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AuthController : MonoBehaviour
{
    // error output
    public TMP_InputField errorOutput;

    // register
    public TMP_InputField registerUsername;
    public TMP_InputField registerEmail;
    public TMP_InputField registerPassword;
    public TMP_InputField registerPassword2;

    [System.Serializable]
    public class RegisterData
    {
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Password2{ get; private set; }

        public RegisterData(string username, string email, string password, string password2) {
            Username = username;
            Email = email;
            Password = password;
            Password2 = password2;

            Debug.Log($"Username: {Username}");
        }
    }

    public IEnumerator SendRegisterData(RegisterData registerData)
    {
        // prepare data
        var json = JsonUtility.ToJson(registerData);
        byte[] raw = Encoding.UTF8.GetBytes(json);
        string registerEndpoint = NetworkController.ServerAdress;

        Debug.Log(json);
        // request
        using (UnityWebRequest request = new UnityWebRequest(registerEndpoint, "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(raw);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Register success");
                Debug.Log($"Server response: {request.downloadHandler.text}");
            }
            else
            {
                Debug.Log($"RegisterError: {request.error}");
                errorOutput.text = request.error;
            }
        }
    }

    public void Register()
    {
        StartCoroutine(SendRegisterData(new RegisterData(
            registerUsername.text,
            registerEmail.text,
            registerPassword.text,
            registerPassword2.text
        )));
    }

    // login
    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;
    public class LoginData
    {
        public string Email { get; private set; }
        public string Password { get; private set; }

        public LoginData(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    public IEnumerator SendLoginData(LoginData loginData)
    {
        yield return null;
    }

    public void Login()
    {
        Debug.Log(loginEmail.text);
        StartCoroutine(SendLoginData(new LoginData(
            loginEmail.text,
            loginPassword.text
        )));
    }
}
