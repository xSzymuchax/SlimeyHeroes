using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Main controlling game class. It connects everything that is used in game.
/// </summary>
public class GameController : MonoBehaviour
{

    public List<ElementType> currentElements;
    private int _defaultTypesAmount = 4; 

    // gameboard init
    public Gameboard gameboard;
    public int boardWidth = 5;
    public int boardHeigth = 6;
    public float maxComboTime = 0.3f;
    public float fallingAnimationDuration = 0.5f;
    
    public static GameController Instance;
    
    // main cam
    public Camera playerCam;

    // progress tracking
    public BarsController barsController;
    public GameObject turnBarsUIContainer;
    public ElementsTracker elementsTracker;

    private void Start()
    {
        Instance = this;
        StartGame();
    }

    /// <summary>
    /// Function used to send click information to gameboard. 
    /// Called by Elements. 
    /// </summary>
    /// <param name="position2D">Position of Element in Gameboard</param>
    public void ElementPressed(Position2D position2D)
    {
        // tell gameboard that element was pressed
        List<CollectedElementsInformation> ceis = gameboard.ElementPressed(position2D);

        // update game state
        foreach (CollectedElementsInformation cei in ceis)
            elementsTracker.UpdateTracker(cei);
    }
    
    /// <summary>
    /// Function updating the game state if there are multiple types of elements destroyed in one move.
    /// </summary>
    /// <param name="ceis">List with informations about collected elements.</param>
    public void CollectMixedElements(List<CollectedElementsInformation> ceis)
    {
        foreach (var item in ceis)
        {
            elementsTracker.UpdateTracker(item);
        }
    }

    /// <summary>
    /// Main Function starting the game. TODO - should take input, such as elements used in the particular game, inventory, buffs...
    /// </summary>
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

    /// <summary>
    /// Sets camera size to the size of gameboard.
    /// </summary>
    private void CalculateCameraSize()
    {
        int max = boardWidth;
        if (boardHeigth > max)
            max = boardHeigth;

        playerCam.orthographicSize = max;
    }
}
