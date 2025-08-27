using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Authorization controller. Used for Register and Login of the users.
/// Class uses corresponding UI text elements, that should be assigned in editor.
/// </summary>
public class AuthController : MonoBehaviour
{
    // error output
    public TextMeshProUGUI errorOutput;

    // register
    public TMP_InputField registerUsername;
    public TMP_InputField registerEmail;
    public TMP_InputField registerPassword;
    public TMP_InputField registerPassword2;

    /// <summary>
    /// Sends Register Request to the server.
    /// </summary>
    /// <param name="registerData">new user data</param>
    /// <returns></returns>
    public IEnumerator SendRegisterData(RegisterRequest registerData)
    {
        // prepare data
        var json = JsonUtility.ToJson(registerData);
        byte[] raw = Encoding.UTF8.GetBytes(json);
        string registerEndpoint = NetworkController.ServerAdress + "/auth/register";

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
                errorOutput.gameObject.SetActive(true);
                errorOutput.text = request.error;
            }
        }
    }

    /// <summary>
    /// Call of Register.
    /// </summary>
    public void Register()
    {
        StartCoroutine(SendRegisterData(new RegisterRequest(
            registerUsername.text,
            registerEmail.text,
            registerPassword.text,
            registerPassword2.text
        )));
    }

    // login
    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;

    /// <summary>
    /// Sends Login Request to teh server. If login is successful, it saves acquired token.
    /// </summary>
    /// <param name="loginData">data sent to server.</param>
    /// <returns></returns>
    public IEnumerator SendLoginData(LoginRequest loginData)
    {
        // prepare data
        var json = JsonUtility.ToJson(loginData);
        byte[] raw = Encoding.UTF8.GetBytes(json);
        string loginEndpoint = NetworkController.ServerAdress + "/auth/login";

        using (UnityWebRequest request = new UnityWebRequest(loginEndpoint, "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(raw);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string serverResponse = request.downloadHandler.text;
                TokenResponse tokenResponse = JsonUtility.FromJson<TokenResponse>(serverResponse);

                NetworkController.SetToken(tokenResponse.token);
                Debug.Log($"Received authorization token: {NetworkController.TokenJWT}");
            }
            else
            {
                Debug.LogError($"Login error: {request.error}");
            }
        }

        yield return null;
    }

    /// <summary>
    /// Login Request call.
    /// </summary>
    public void Login()
    {
        Debug.Log(loginEmail.text);
        StartCoroutine(SendLoginData(new LoginRequest(
            loginEmail.text,
            loginPassword.text
        )));
    }
}
