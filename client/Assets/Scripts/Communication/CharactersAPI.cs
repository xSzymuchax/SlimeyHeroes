using Assets.Scripts.Communication.ResponseModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Communication
{
    [Serializable]
    public class Wrapper<T>
    {
        public T[] Items;
    }

    /// <summary>
    /// Class used for communication with characters API.
    /// </summary>
    public class CharactersAPI
    {
        /// <summary>
        /// Gets list of characters that is available for a player.
        /// </summary>
        /// <param name="onSuccess">success handler</param>
        /// <param name="onError">error handler</param>
        /// <returns>on success, leturns list of CharacterResponse objects</returns>
        public static IEnumerator SendGetCharactersRequest(System.Action<List<CharacterResponse>> onSuccess, System.Action<string> onError)
        {
            string charactersEndpoint = NetworkController.ServerAdress + "/characters";

            using (UnityWebRequest request = new UnityWebRequest())
            {
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", $"Bearer {NetworkController.TokenJWT}");
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                try
                {


                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        string json = request.downloadHandler.text;

                        var wrapper = JsonUtility.FromJson<Wrapper<CharacterResponse>>(json);
                        onSuccess?.Invoke(wrapper.Items.ToList());
                    }
                    else
                    {
                        onError?.Invoke(request.error);
                    }
                }
                catch (Exception e)
                {
                    onError?.Invoke(e.Message);
                }
            }
        }
    }
}
