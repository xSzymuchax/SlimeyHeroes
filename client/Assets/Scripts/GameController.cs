using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public List<ElementType> currentElements;
    private int _defaultTypesAmount = 4;

    public Gameboard gameboard;
    public int boardWidth = 5;
    public int boardHeigth = 6;
    public float maxComboTime = 0.3f;
    public float fallingAnimationDuration = 0.5f;
    
    public static GameController Instance;
    public Camera playerCam;

    public BarsController barsController;
    public GameObject turnBarsUIContainer;

    private void Start()
    {
        Instance = this;
        StartGame();
    }

    public void ElementPressed(Position2D position2D)
    {
        // tell gameboard that element was pressed
        CollectedElementsInformation cei = gameboard.ElementPressed(position2D);

        // update bars
        barsController.UpdateBar(cei);
    }
    
    public void StartGame()
    {
        // draw 4 element types
        currentElements = ElementDataGenerator.DrawElementTypes(_defaultTypesAmount);

        // create gameboard with 4 types
        gameboard = this.AddComponent<Gameboard>();
        gameboard.Init(boardWidth, boardHeigth, maxComboTime, currentElements);

        // create bars corresponding to types
        barsController.InitializeBars(currentElements);

        // fix camera position
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
