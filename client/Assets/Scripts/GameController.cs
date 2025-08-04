using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public int boardWidth = 5;
    public int boardHeigth = 6;
    public Transform boardCenter;
    public GameObject[] elementPrefabs;
    public static GameController Instance;
    private GameObject[,] gameboard;


    private void Start()
    {
        Instance = this;
        FillGameboard();
    }

    private void FillGameboard()
    {
        gameboard = new GameObject[boardWidth, boardHeigth];
        float xOffset = -boardWidth / 2;
        float yOffset = -boardHeigth / 2;

        string debugString = "";

        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeigth; j++)
            {
                GameObject selectedPrefab;
                int randomChoice = Random.Range(0, elementPrefabs.Length);
                selectedPrefab = elementPrefabs[randomChoice];

                Vector3 elementPosition = new Vector3(
                    boardCenter.transform.position.x + xOffset + j,
                    boardCenter.transform.position.y + yOffset + i,
                    boardCenter.transform.position.z);
                
                GameObject go = Instantiate(selectedPrefab, elementPosition, Quaternion.identity);
                go.GetComponent<Element>().SetPosition(new Position2D(i,j));
                gameboard[i, j] = go;

                debugString += $"({i}, {j}, {go.GetComponent<Element>().elementType})";
            }
            debugString += "\n";
        }
        Debug.Log(debugString);
    }

    public void ElementPressed(Position2D position2D)
    {
        List<Position2D> positions = FindSurroundingSimilarElements(position2D);
        Debug.Log(positions.Count);

        string debugString = "";
        foreach (Position2D p in positions)
        {
            debugString += $"({p.X},{p.Y})";
        }

        Debug.Log(debugString);
        CollectElements(positions);
        //Destroy(go);
    }

    private void RecursiveGroupFinder(List<Position2D> positions, bool[,] fieldChecked, int x, int y, ElementType type)
    {
        if (x < 0 || x >= boardWidth)
            return;

        if (y < 0 || y >= boardHeigth)
            return;

        if (gameboard[x, y] == null)
            return;

        if (fieldChecked[x, y] == true)
            return;

        fieldChecked[x, y] = true;
        if (type != gameboard[x, y].GetComponent<Element>().elementType)
            return;

        // legal cell, fine type
        positions.Add(new Position2D(x, y));

        // check surrounding
        var offsets = new List<(int x, int y)> { (1, 0), (-1, 0), (0, 1), (0, -1) };
        foreach (var (x_off, y_off) in offsets)
            RecursiveGroupFinder(positions, fieldChecked, x + x_off, y + y_off, type);

    }

    private List<Position2D> FindSurroundingSimilarElements(Position2D position2D)
    {
        int x = position2D.X;
        int y = position2D.Y;
        ElementType type = gameboard[x, y].GetComponent<Element>().elementType;
        bool[,] fieldChecked = new bool[boardWidth, boardHeigth];
        List<Position2D> position2Ds = new List<Position2D>();
        
        for (int i = 0; i < boardWidth; i++)
            for (int j = 0; j < boardHeigth; j++)
                fieldChecked[i, j] = false;

        RecursiveGroupFinder(position2Ds, fieldChecked, x, y, type);

        return position2Ds;
    }

    // TODO - collect instead of destroy
    private void CollectElements(List<Position2D> elementsPositions)
    {
        if (elementsPositions.Count < 3)
            return;

        foreach (Position2D p2d in elementsPositions)
        {
            GameObject go = gameboard[p2d.X, p2d.Y];
            gameboard[p2d.X, p2d.Y] = null;
            Destroy(go);
        }
            
    }

}
