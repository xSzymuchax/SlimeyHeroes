using Assets.Scripts.Communication;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
/// Connects client with mathmaking system. 
/// Then, it redirects player to the lobby. 
/// </summary>
public class OnlineMatchmakerController : MonoBehaviour
{
    private HubConnection matchmakingConnection;
    private HubConnection lobbyConnection;

    public TextMeshProUGUI text;
    public async void ConnectToMatchmakerService()
    {
        matchmakingConnection = new HubConnectionBuilder()
            .WithUrl(NetworkController.ServerAdress + "/matchmaker", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(NetworkController.TokenJWT);
            })
            .WithAutomaticReconnect()
            .Build();

        // TODO - to sie chyba przewraca
        matchmakingConnection.On<string>("MatchFound", async (matchId) => {
            Debug.Log($"Match found! -> matchID: {matchId}");
            text.text += $"Match found! -> matchID: {matchId}";

            _ = ConnectToLobby(matchId);

            //await matchmakingConnection.StopAsync();
        });

        matchmakingConnection.On("MatchmakingCancelled", () => {
            Debug.Log($"Matchmaking cancelled");
        });


        try
        {
            await matchmakingConnection.StartAsync();
            Debug.Log("Connected to server");
            text.text += "Connected to server";

            await matchmakingConnection.InvokeAsync("FindMatch");
        }
        catch (Exception ex)
        {
            text.text += "Connection failed" + ex.Message;
            Debug.LogError("Connection failed" + ex.Message);
        }
    }

    public async void CancellMatchmaking()
    {
        if (matchmakingConnection == null)
        {
            Debug.Log("Already disconnected.");
            return;
        }

        try
        {
            await matchmakingConnection.InvokeAsync("CancelMatchmaking");
            await matchmakingConnection.StopAsync();
            await matchmakingConnection.DisposeAsync();
            matchmakingConnection = null;

        }
        catch (Exception ex)
        {
            text.text += "Connection failed" + ex.Message;
            Debug.LogError("Connection failed" + ex.Message);
        }
    }

    public async Task ConnectToLobby(string matchId)
    {
        Debug.Log("konektuje do lobbi");
        lobbyConnection = new HubConnectionBuilder()
            .WithUrl(NetworkController.ServerAdress + $"/lobby/?matchId={matchId}", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(NetworkController.TokenJWT);
            })
            .WithAutomaticReconnect()
            .Build();

        List<int> elementsdIDs = new List<int>();

        lobbyConnection.On<List<int>>("DrawnElements", (elements) => {
            elementsdIDs = elements;
            GameController.Instance.ShowLobbyScreen(elements);
        });



        // StartCountdown, seconds
        // TODO - counter
        lobbyConnection.On<int>("StartCountdown", (seconds) => {
            Console.WriteLine("received: StartCountdown");
            GameController.Instance.ShowLobbyScreen(elementsdIDs);
            LoadMyCharacters();
        });

        // GameStart

        try
        {
            await lobbyConnection.StartAsync();
            Debug.Log("Stan:" + lobbyConnection.State);

            //await matchmakingConnection.StopAsync();

            await lobbyConnection.InvokeAsync("JoinLobby", matchId);

            // download my available characters

            // when im ready, send info
            

        } catch (Exception e)
        {
            text.text += "Connection failed" + e.Message;
            Debug.LogError("Connection failed" + e.Message);
        }
    }

    

    public void LoadMyCharacters()
    {
        StartCoroutine(CharactersAPI.SendGetCharactersRequest(
            onSuccess: (charactersList) => {
                foreach (var c in charactersList)
                    GameController.Instance.CreateCharacterPickOption(c);
            },
            onError: (a) => { }
            ));
    }
}
