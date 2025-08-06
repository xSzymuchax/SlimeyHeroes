using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class Gameboard : MonoBehaviour
    {
        private GameObject[,] gameboard;
        public int boardWidth = 5;
        public int boardHeigth = 6;
        public Transform boardCenter;
        public GameObject[] elementPrefabs;
        private float _maxComboTime;
        private Coroutine _fixCooutine;

        public void Init(int width, int heigth, GameObject[] prefabs, float maxComboTime)
        {
            boardWidth = width;
            boardHeigth = heigth;
            elementPrefabs = prefabs;
            _maxComboTime = maxComboTime;
            boardCenter = GameObject.Find("BoardCenter").transform;
            FillGameboard();
        }


        private GameObject GenerateElement(int x_position, int y_position, bool generateAbove = false)
        {
            float xOffset = -boardWidth / 2;
            float yOffset = -boardHeigth / 2;

            int bonusYOffset = 0;
            if (generateAbove)
                bonusYOffset = boardHeigth;

            GameObject selectedPrefab;
            int randomChoice = Random.Range(0, elementPrefabs.Length);
            selectedPrefab = elementPrefabs[randomChoice];

            Vector3 elementPosition = new Vector3(
                boardCenter.transform.position.x + x_position + xOffset,
                boardCenter.transform.position.y + y_position + yOffset + bonusYOffset,
                boardCenter.transform.position.z);

            GameObject go = Instantiate(selectedPrefab, elementPosition, Quaternion.identity);
            Element e = go.GetComponent<Element>();
            Vector3 targetPosition = new Vector3(
                    e.gameObject.transform.position.x,
                    e.gameObject.transform.position.y - bonusYOffset,
                    e.gameObject.transform.position.z
            );

            e.SetPosition(new Position2D(x_position, y_position));
            e.FallToPosition(targetPosition);

            return go;
        }

        private void FillGameboard()
        {
            gameboard = new GameObject[boardWidth, boardHeigth];

            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeigth; j++)
                {
                    GameObject go = GenerateElement(i, j);
                    gameboard[i, j] = go;
                }
            }
            DebugShowGameboard();
        }

        private IEnumerator FixGameboardAfter(float maxComboTime)
        {
            yield return new WaitForSeconds(maxComboTime);
            FixGameboard();
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

            if (_fixCooutine != null)
                StopCoroutine(_fixCooutine);
            _fixCooutine = StartCoroutine(FixGameboardAfter(_maxComboTime));
            //Destroy(go);
        }

        private void FixColumn(int columnIndex)
        {
            for (int i = 0; i < boardHeigth; i++)
            {
                if (gameboard[columnIndex, i] != null)
                    continue;

                int firstElement = -1;
                for (int j=i+1; j < boardHeigth; j++)
                {
                    if (gameboard[columnIndex, j] != null)
                    {
                        firstElement = j;
                        break;
                    }
                }

                if (firstElement == -1)
                    return;

                gameboard[columnIndex, i] = gameboard[columnIndex, firstElement];
                gameboard[columnIndex, firstElement] = null;


                Element e = gameboard[columnIndex, i].GetComponent<Element>();
                e.FallToPosition(new Vector3(
                    e.gameObject.transform.position.x,
                    e.gameObject.transform.position.y - (firstElement - i),
                    e.gameObject.transform.position.z
                    ));

                e.SetPosition(new Position2D(columnIndex, i));
            }
        }

        private List<ColumnMissing> FindMissing()
        {
            List<ColumnMissing> missing = new List<ColumnMissing>();
            int counter;
            for (int i = 0; i < boardWidth; i++)
            {
                if (gameboard[i, boardHeigth-1] == null)
                {
                    counter = 0;
                    int j = boardHeigth - 1;
                    while (j>=0)
                    {
                        if (gameboard[i, j] == null)
                        {
                            counter++;
                        }
                        j--;
                    }
                    missing.Add(new ColumnMissing(i, counter));
                }
            }
            //Debug.Log("columns with missing: " + missing.Count);
            //foreach (ColumnMissing cm in missing)
            //{
            //    Debug.Log($"Column: {cm.ColumnIndex}, Missing: {cm.MissingElements}");
            //}
            return missing;
        }

        private void GenerateMissing(List<ColumnMissing> missings)
        {
            foreach (ColumnMissing cm in missings)
            {
                int index = cm.ColumnIndex;
                int amount = cm.MissingElements;
                int firstMissing = boardHeigth - amount;

                for (int j = firstMissing; j < boardHeigth; j++)
                {
                    gameboard[index, j] = GenerateElement(index, j, true);
                }
            }
        }

        private void FixGameboard()
        {
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeigth; j++)
                {
                    if (gameboard[i, j] == null)
                    {
                        FixColumn(i);
                    }
                }
            }
            List<ColumnMissing> missings = FindMissing();
            GenerateMissing(missings);
            DebugShowGameboard();
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

        private void DebugShowGameboard()
        {
            string debugString = "";
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeigth; j++)
                {
                    if (gameboard[i, j] != null)
                    {
                        Element e = gameboard[i, j].GetComponent<Element>();
                        debugString += $"\t({e.X}, {e.Y}, {e.elementType}) ";
                    }
                    else
                        debugString += "\tNULL ";
                }
                debugString += "\n";
            }

            Debug.Log(debugString);
        }
    }
}
