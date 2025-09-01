using Assets.Scripts.Communication;
using Assets.Scripts.Communication.ResponseModels;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    private SynchronizationContext _synchronizationContext;

    private void Start()
    {
        _synchronizationContext = SynchronizationContext.Current;
    }

    public async void ConnectToMatchmakerService()
    {
        matchmakingConnection = new HubConnectionBuilder()
            .WithUrl(NetworkHelper.ServerAdress + "/matchmaker", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(NetworkHelper.TokenJWT);
            })
            .WithAutomaticReconnect()
            .Build();

        // TODO - to sie chyba przewraca
        matchmakingConnection.On<string>("MatchFound", async (matchId) => {
            try
            {
                Debug.Log($"Match found! -> matchID: {matchId}");
                //text.text += $"Match found! -> matchID: {matchId}";

                _ = ConnectToLobby(matchId);

                Debug.Log("a czy to sie wykonuje?");

            }
            catch (Exception e)
            {
                Debug.LogError("blad handlera: " + e.Message);
            }
            //await matchmakingConnection.StopAsync();
        });

        matchmakingConnection.On("MatchmakingCancelled", () => {
            Debug.Log($"Matchmaking cancelled");
        });


        try
        {
            await matchmakingConnection.StartAsync();
            Debug.Log("Connected to server");

            await matchmakingConnection.InvokeAsync("FindMatch");
        }
        catch (Exception ex)
        {
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
            Debug.LogError("Connection failed" + ex.Message);
        }
    }

    public async Task ConnectToLobby(string matchId)
    {
        Debug.Log("konektuje do lobbi");
        lobbyConnection = new HubConnectionBuilder()
            .WithUrl(NetworkHelper.ServerAdress + $"/lobby", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(NetworkHelper.TokenJWT);
            })
            .WithAutomaticReconnect()
            .Build();

        List<int> elementsdIDs = new List<int>();
        lobbyConnection.On<List<int>>("DrawnElements", (elements) => {
            elementsdIDs = elements;

            _synchronizationContext.Post(_ =>
            {
                // TODO - load elements
            }, null);
            
        });

        lobbyConnection.On("GameStart", () =>
        {
            // TODO - go to the game (we are in matchmaker now)
        });

        // StartCountdown, seconds
        // TODO - counter
        lobbyConnection.On<int>("StartCountdown", (seconds) => {
            Console.WriteLine("received: StartCountdown");

            _synchronizationContext.Post(_ =>
            {
                GameController.Instance.ShowLobbyScreen(elementsdIDs);
                LoadMyCharacters();
            }, null); 
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
            Debug.LogError("Connection failed" + e.Message);
        }
    }

    

    public async Task LoadMyCharacters()
    {
        List<CharacterResponse> characters = await CharactersAPI.GetUnlockedCharacters();

        characters.ForEach(x =>
        {
            Debug.Log($"characterId: {x.characterId}, level: {x.level}, souls: {x.soulsAmount}");
        });

        characters.ForEach(x =>
        {
            try
            {
                Debug.Log("tworze postac");
                GameController.Instance.CreateCharacterPickOption(x);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            
        });
    }
}
