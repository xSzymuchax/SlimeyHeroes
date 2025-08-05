using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public int boardWidth = 5;
    public int boardHeigth = 6;
    public GameObject[] elementPrefabs;
    public static GameController Instance;
    public Gameboard gameboard;

    private void Start()
    {
        Instance = this;
        gameboard = new Gameboard(boardWidth, boardHeigth, elementPrefabs);
    }

    public void ElementPressed(Position2D position2D)
    {
        gameboard.ElementPressed(position2D);
    }
    
}
