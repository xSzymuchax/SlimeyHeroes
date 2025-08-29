using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


/// <summary>
/// Connects client with mathmaking system. 
/// Then, it redirects player to the lobby. 
/// </summary>
public class OnlineMatchmakerController : MonoBehaviour
{
    private HubConnection connection;
    public TextMeshProUGUI text;
    public async void ConnectToMatchmakerService()
    {
        connection = new HubConnectionBuilder()
            .WithUrl(NetworkController.ServerAdress + "/matchmaker")
            .WithAutomaticReconnect()
            .Build();

        connection.On<string, string>("MatchFound", async (matchId, opponentId) => {
            Debug.Log($"Match found! -> matchID: {matchId}, opponent: {opponentId}");
            text.text += $"Match found! -> matchID: {matchId}, opponent: {opponentId}";

            await connection.StopAsync();

            await ConnectToLobby(matchId);
        });

        connection.On("MatchmakingCancelled", () => {
            Debug.Log($"Matchmaking cancelled");
        });


        try
        {
            await connection.StartAsync();
            Debug.Log("Connected to server");
            text.text += "Connected to server";

            await connection.InvokeAsync("FindMatch");
        }
        catch (Exception ex)
        {
            text.text += "Connection failed" + ex.Message;
            Debug.LogError("Connection failed" + ex.Message);
        }
    }

    public async void CancellMatchmaking()
    {
        if (connection == null)
        {
            Debug.Log("Already disconnected.");
            return;
        }

        try
        {
            await connection.InvokeAsync("CancelMatchmaking");
            await connection.StopAsync();
            await connection.DisposeAsync();
            connection = null;

        }
        catch (Exception ex)
        {
            text.text += "Connection failed" + ex.Message;
            Debug.LogError("Connection failed" + ex.Message);
        }
    }

    public async Task ConnectToLobby(string matchId)
    {
        connection = new HubConnectionBuilder()
            .WithUrl(NetworkController.ServerAdress + $"/lobby/?matchId={matchId}")
            .WithAutomaticReconnect()
            .Build();

        List<int> elementsdIDs = new List<int>();

        connection.On<List<int>>("DrawnElements", (elements) => {
            elementsdIDs = elements;
        });

        try
        {
            await connection.StartAsync();
            await connection.InvokeAsync("JoinLobby", matchId);

            GameController.Instance.ShowLobbyScreen(elementsdIDs);

        } catch (Exception e)
        {
            text.text += "Connection failed" + e.Message;
            Debug.LogError("Connection failed" + e.Message);
        }
        
    }
}
