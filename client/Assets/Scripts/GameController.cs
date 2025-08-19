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

    public ElementsTracker elementsTracker;

    private void Start()
    {
        Instance = this;
        StartGame();
    }

    public void ElementPressed(Position2D position2D)
    {
        // tell gameboard that element was pressed
        List<CollectedElementsInformation> ceis = gameboard.ElementPressed(position2D);

        // TODO - update game state information
        foreach (CollectedElementsInformation cei in ceis)
            elementsTracker.UpdateTracker(cei);
    }
    
    public void CollectMixedElements(List<CollectedElementsInformation> ceis)
    {
        foreach (var item in ceis)
        {
            elementsTracker.UpdateTracker(item);
        }
    }

    public void StartGame()
    {
        // draw 4 element types TODO - should get from server
        currentElements = ElementDataGenerator.DrawElementTypes(_defaultTypesAmount);

        // TODO - create some data object to controll collected elements
        elementsTracker = new ElementsTracker(currentElements);

        // create gameboard with 4 types
        gameboard = this.AddComponent<Gameboard>();
        gameboard.Init(boardWidth, boardHeigth, maxComboTime, currentElements);

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
