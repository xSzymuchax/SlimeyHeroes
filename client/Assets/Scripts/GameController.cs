using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

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

    // figthing 
    public BarsController barsController;
    public GameObject turnBarsUIContainer;
    public ElementsTracker elementsTracker;

    public FightingController fightingController;
    public GameObject characterPrefab;
    public Transform myTeamSpawn;
    public Transform enemyTeamSpawn;

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

        if (ceis == null)
            return;

        // update game state
        foreach (CollectedElementsInformation cei in ceis)
        {
            bool grantedMove = elementsTracker.UpdateTracker(cei);
            if (grantedMove)
                fightingController.PerformAction(cei.elementType);
        }   
    }
    
    /// <summary>
    /// Function updating the game state if there are multiple types of elements destroyed in one move.
    /// </summary>
    /// <param name="ceis">List with informations about collected elements.</param>
    public void CollectMixedElements(List<CollectedElementsInformation> ceis)
    {
        foreach (var item in ceis)
        {
            bool grantedMove = elementsTracker.UpdateTracker(item);
            if (grantedMove)
                fightingController.PerformAction(item.elementType);
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

        // initialize teams - TODO - should be loaded from server data
        fightingController = gameObject.AddComponent<FightingController>();
        List<GameObject> team = new List<GameObject>() { characterPrefab };
        fightingController.Initialize(team, team, myTeamSpawn, enemyTeamSpawn);

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
