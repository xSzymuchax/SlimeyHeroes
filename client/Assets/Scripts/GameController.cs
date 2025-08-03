using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int boardSize = 5;
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
        gameboard = new GameObject[10, 10];
        float xOffset = -boardSize / 2;
        float yOffset = -boardSize / 2;

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                GameObject selectedPrefab;
                int randomChoice = Random.Range(0, elementPrefabs.Length);
                selectedPrefab = elementPrefabs[randomChoice];

                Vector3 elementPosition = new Vector3(
                    boardCenter.transform.position.x + xOffset + i,
                    boardCenter.transform.position.y + yOffset + j,
                    boardCenter.transform.position.z);
                
                GameObject go = Instantiate(selectedPrefab, elementPosition, Quaternion.identity);
                go.GetComponent<Element>().SetPosition(i, j);
                gameboard[i, j] = go;
            }
        }
    }

    public void ElementPressed(int x, int y)
    {
        GameObject go = gameboard[x, y];
        gameboard[x,y] = null;
        Destroy(go);
    }
}
