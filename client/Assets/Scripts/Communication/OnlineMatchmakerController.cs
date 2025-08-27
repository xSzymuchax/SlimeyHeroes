using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


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

        connection.On<string, string>("MatchFound", (matchId, opponentId) => {
            Debug.Log($"Match found! -> matchID: {matchId}, opponent: {opponentId}");
            text.text += $"Match found! -> matchID: {matchId}, opponent: {opponentId}";
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
}
