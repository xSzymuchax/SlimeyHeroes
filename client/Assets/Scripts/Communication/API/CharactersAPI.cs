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
    /// <summary>
    /// Class used for communication with server's characters API.
    /// </summary>
    public class CharactersAPI
    {
        /// <summary>
        /// Gets list of characters that is available for a player.
        /// </summary>
        /// <param name="onSuccess">success handler</param>
        /// <param name="onError">error handler</param>
        /// <returns>list of CharacterResponse objects</returns>
        public static async Task<List<CharacterResponse>> GetUnlockedCharacters()
        {
            string charactersEndpoint = NetworkHelper.ServerAdress + "/characters";

            using (UnityWebRequest request = new UnityWebRequest(charactersEndpoint, "GET"))
            {
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", $"Bearer {NetworkHelper.TokenJWT}");
                request.downloadHandler = new DownloadHandlerBuffer();

                var request_state = request.SendWebRequest();

                while (!request_state.isDone)
                    await Task.Yield();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string json = request.downloadHandler.text;
                    return JsonDecoder.FromJson<CharacterResponse>(json).ToList();
                }
                else
                {
                    Debug.Log($"Error decoding characters API response.");
                    throw new Exception(request.error);
                }
            }
        }
    }
}
