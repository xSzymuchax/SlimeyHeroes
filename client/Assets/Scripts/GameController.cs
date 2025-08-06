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
    public GameObject[] elementPrefabs;
    public static GameController Instance;
    public Gameboard gameboard;

    private void Start()
    {
        Instance = this;
        gameboard = this.AddComponent<Gameboard>();
        gameboard.Init(boardWidth, boardHeigth, elementPrefabs, maxComboTime);
    }

    public void ElementPressed(Position2D position2D)
    {
        gameboard.ElementPressed(position2D);
    }
    
}
