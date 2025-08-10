using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public int boardWidth = 5;
    public int boardHeigth = 6;
    public float maxComboTime = 0.3f;
    public float fallingAnimationDuration = 0.5f;
    public GameObject[] elementPrefabs;
    public static GameController Instance;
    public Gameboard gameboard;
    public Camera playerCam;

    private void Start()
    {
        Instance = this;
        //StartGame();
    }

    public void ElementPressed(Position2D position2D)
    {
        gameboard.ElementPressed(position2D);
    }
    
    public void StartGame()
    {
        gameboard = this.AddComponent<Gameboard>();
        gameboard.Init(boardWidth, boardHeigth, elementPrefabs, maxComboTime);
        CalculateCameraSize();
    }

    private void CalculateCameraSize()
    {
        int max = boardWidth;
        if (boardHeigth > max)
            max = boardHeigth;

        playerCam.orthographicSize = max;
    }
}
