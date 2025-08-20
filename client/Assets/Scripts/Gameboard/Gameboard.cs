using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    /// <summary>
    /// Class that holds entire gameboard.
    /// </summary>
    public class Gameboard : MonoBehaviour
    {
        private GameObject[,] gameboard;
        public int boardWidth = 5;
        public int boardHeigth = 6;
        public Transform boardCenter;
        public GameObject[] elementPrefabs;
        private float _maxComboTime;
        private Coroutine _fixCooutine;
        private bool _canClick = true;
        private Transform _elementsContainer;

        /// <summary>
        /// Initializes the gameboard.
        /// It needs 2 GameObjects with exact names: "BoardCenter" and "ElementsContainer".
        /// "BoardCenter" is a point, around which elements will be spawned, and teh other one is just container,
        /// so the hierarchy won't get messy.
        /// </summary>
        /// <param name="width">Gameboard width.</param>
        /// <param name="heigth">Gameboard heigth.</param>
        /// <param name="maxComboTime">Max time allowed, before elements start respawning.</param>
        /// <param name="choosenElements">List of element types, that will be used in this gameboard.</param>
        public void Init(int width, int heigth, float maxComboTime, List<ElementType> choosenElements)
        {
            boardWidth = width;
            boardHeigth = heigth;
            elementPrefabs = LoadChoosenElements(choosenElements);
            _maxComboTime = maxComboTime;
            boardCenter = GameObject.Find("BoardCenter").transform;
            _elementsContainer = GameObject.Find("ElementsContainer").transform;
            FillGameboard();

            SummonEffectTest();
        }

        /// <summary>
        /// Debuging function used for spawning special elements.
        /// </summary>
        private void SummonEffectTest()
        {
            gameboard[5, 5].GetComponent<Element>().SetEffect(new ColorSplashEffect(new Position2D(5, 5), 3, gameboard[5,5].GetComponent<Element>().elementType));
            //gameboard[5, 5].GetComponent<Element>().SetEffect(new ExplosionEffect(new Position2D(5,5), 5));
            //gameboard[5, 5].GetComponent<Element>().SetEffect(new ExplosionEffect(new Position2D(5,5), 3));
            //gameboard[5, 5].GetComponent<Element>().SetEffect(new ColorWipeEffect(new Position2D(5, 5), 3, gameboard[5, 5].GetComponent<Element>().elementType));

            gameboard[5, 5].gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }

        /// <summary>
        /// Returns array of Gameobjects used in board.
        /// </summary>
        /// <param name="choosenElements">List of elements types that should be generated.</param>
        /// <returns>array of Gameobjects</returns>
        private GameObject[] LoadChoosenElements(List<ElementType> choosenElements)
        {
            return ElementDataGenerator.GetElementsPrefabs(choosenElements);
        }

        /// <summary>
        /// Generates Gameboard elements, and instatiates them. Starts their falling animation.
        /// </summary>
        /// <param name="x_position">X position in gameboard</param>
        /// <param name="y_position">Y position in gameboard</param>
        /// <param name="generateAbove">tells if element should be spawned above gameboard or in place</param>
        /// <returns>returns GameObject of generated element</returns>
        private GameObject GenerateElement(int x_position, int y_position, bool generateAbove = false)
        {
            float xOffset = -boardWidth / 2f;
            float yOffset = -boardHeigth / 2f;

            if (boardWidth % 2 == 0)
                xOffset += 0.5f;
            if (boardHeigth % 2 == 0)
                yOffset += 0.5f;

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
            go.transform.SetParent(_elementsContainer);
            Element e = go.GetComponent<Element>();
            Vector3 targetPosition = new Vector3(
                    e.gameObject.transform.position.x,
                    e.gameObject.transform.position.y - bonusYOffset,
                    e.gameObject.transform.position.z
            );

            e.SetPosition(new Position2D(x_position, y_position));
            e.SetFallingAnimationTime(GameController.Instance.fallingAnimationDuration);
            e.FallToPosition(targetPosition);

            return go;
        }

        /// <summary>
        /// Generates Gameboard elements, and instatiates them. Starts their falling animation.
        /// Allows to set the exact type of element that should be spawned.
        /// </summary>
        /// <param name="x_position">X position in gameboard</param>
        /// <param name="y_position">Y position in gameboard</param>
        /// <param name="elementType">type of element</param>
        /// <param name="generateAbove">tells if element should be spawned above gameboard or in place</param>
        /// <returns>returns GameObject of generated element</returns>
        private GameObject GenerateElement(int x_position, int y_position, ElementType elementType, bool generateAbove = false)
        {
            float xOffset = -boardWidth / 2f;
            float yOffset = -boardHeigth / 2f;

            if (boardWidth % 2 == 0)
                xOffset += 0.5f;
            if (boardHeigth % 2 == 0)
                yOffset += 0.5f;

            int bonusYOffset = 0;
            if (generateAbove)
                bonusYOffset = boardHeigth;

            GameObject selectedPrefab = null;
            
            foreach (GameObject g in elementPrefabs)
            {
                Element el = g.GetComponent<Element>();
                if (el.elementType == elementType)
                {
                    selectedPrefab = g;
                    Debug.Log(selectedPrefab.name);
                    break;
                }
            }

            Vector3 elementPosition = new Vector3(
                boardCenter.transform.position.x + x_position + xOffset,
                boardCenter.transform.position.y + y_position + yOffset + bonusYOffset,
                boardCenter.transform.position.z);

            GameObject go = Instantiate(selectedPrefab, elementPosition, Quaternion.identity);
            go.transform.SetParent(_elementsContainer);
            Element e = go.GetComponent<Element>();
            Vector3 targetPosition = new Vector3(
                    e.gameObject.transform.position.x,
                    e.gameObject.transform.position.y - bonusYOffset,
                    e.gameObject.transform.position.z
            );

            e.SetPosition(new Position2D(x_position, y_position));
            e.SetFallingAnimationTime(GameController.Instance.fallingAnimationDuration);
            e.FallToPosition(targetPosition);

            return go;
        }

        /// <summary>
        /// Fills the entire Gameboard. Uses GenerateElement methods.
        /// </summary>
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

        /// <summary>
        /// Disables player's ability to collect elements.
        /// </summary>
        /// <param name="time">Duriation of clicking block</param>
        /// <returns>Enumerator for coroutine execution.</returns>
        private IEnumerator DisablePressing(float time)
        {
            _canClick = false;
            yield return new WaitForSeconds(time);
            _canClick = true;
        }

        /// <summary>
        /// Starts refilling gameboard.
        /// </summary>
        /// <param name="maxComboTime">Time before method starts refilling.</param>
        /// <returns>Enumerator for coroutine execution.</returns>
        private IEnumerator FixGameboardAfter(float maxComboTime)
        {
            yield return new WaitForSeconds(maxComboTime);
            FixGameboard();
        }


        /// <summary>
        /// Main entrance point for collecting elements.
        /// Seeks for gruops of elements, and collects them.
        /// At the end, tries to refill gameboard.
        /// </summary>
        /// <param name="position2D">Position of pressed element.</param>
        /// <returns>List of objects, informing about amount and types of collected elements.</returns>
        public List<CollectedElementsInformation> ElementPressed(Position2D position2D)
        {
            if (!_canClick)
                return null;

            List<Position2D> positions = FindSurroundingSimilarElements(position2D);
            Debug.Log(positions.Count);

            string debugString = "";
            foreach (Position2D p in positions)
            {
                debugString += $"({p.X},{p.Y})";
            }

            Debug.Log(debugString);
            List<CollectedElementsInformation> collectedElementsInformation = CollectElements(positions);

            if (_fixCooutine != null)
                StopCoroutine(_fixCooutine);
            _fixCooutine = StartCoroutine(FixGameboardAfter(_maxComboTime));
            
            return collectedElementsInformation;
            //Destroy(go);
        }

        /// <summary>
        /// Returns gameboard's element.
        /// </summary>
        /// <param name="position2D">Position of element</param>
        /// <returns>Element component of gameobject or null.</returns>
        public Element GetElementFromCell(Position2D position2D)
        {
            if (gameboard[position2D.X, position2D.Y] == null)
                return null;
            return gameboard[position2D.X, position2D.Y].GetComponent<Element>();
        }

        /// <summary>
        /// Regenerates element of gameboard to change it's type.
        /// </summary>
        /// <param name="position2D">Position of element</param>
        /// <param name="elementType">New type that should be generated</param>
        public void ChangeElementType(Position2D position2D, ElementType elementType)
        {
            if (position2D.X < 0 || position2D.X >= boardWidth)
                return;

            if (position2D.Y < 0 || position2D.Y >= boardHeigth)
                return;

            if (gameboard[position2D.X, position2D.Y] == null)
                return;

            GameObject oldElement = gameboard[position2D.X, position2D.Y];
            GameObject newElement = GenerateElement(position2D.X, position2D.Y, elementType, false);

            Effect old_effect = oldElement.GetComponent<Element>().Effect;
            newElement.GetComponent<Element>().SetEffect(old_effect);

            Destroy(gameboard[position2D.X, position2D.Y]);
            gameboard[position2D.X, position2D.Y] = newElement;
        }

        /// <summary>
        /// If there are empty spaces in column, it moves elements from above to close the gaps.
        /// </summary>
        /// <param name="columnIndex">Index of fixed column.</param>
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

        /// <summary>
        /// Finds how many elements are missing in which columns.
        /// </summary>
        /// <returns>List of inforations, which column has how many gaps.</returns>
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

        /// <summary>
        /// Refills gameboard with new elements. It starts generating in first possible space.
        /// </summary>
        /// <param name="missings">List of inforations, which column has how many gaps.</param>
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

        /// <summary>
        /// Gameboard fixing entry point. It coordinates every part of refilling.
        /// </summary>
        private void FixGameboard()
        {
            StartCoroutine(DisablePressing(GameController.Instance.fallingAnimationDuration));

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

        /// <summary>
        /// Helper for recursive scanning.
        /// </summary>
        /// <param name="positions">List of found similar elements</param>
        /// <param name="fieldChecked">Array used for checking if field was visited.</param>
        /// <param name="x">x position of gameboard</param>
        /// <param name="y">y position of gameboard</param>
        /// <param name="type">Looks for this type of element</param>
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

        /// <summary>
        /// Finds groups of similar elements in gameboard.
        /// </summary>
        /// <param name="position2D">Starting position of scanning.</param>
        /// <returns>List with positions of similar elements.</returns>
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

        /// <summary>
        /// Groups list of element positions of difrent types into diffrent lists.
        /// </summary>
        /// <param name="positions">Positions of elements</param>
        /// <returns>List of lists of same type.</returns>
        private List<List<Position2D>> SplitPositionsByElementTypes(List<Position2D> positions)
        {
            Dictionary<ElementType, List<Position2D>> dict = new();

            foreach (Position2D position in positions)
            {
                ElementType type = gameboard[position.X, position.Y].GetComponent<Element>().elementType;

                if (!dict.ContainsKey(type))
                    dict[type] = new List<Position2D>();

                dict[type].Add(position);
            }

            List<List<Position2D>> result = dict.Values.ToList();
            return result;
        }

        /// <summary>
        /// Collects elements. 
        /// </summary>
        /// <param name="elementsPositions">positions of elements that have to be collected</param>
        /// <param name="hasToBeGroup">information, if can collect single elements, not groupped with others.</param>
        /// <returns>how many of which elements have been collected</returns>
        public List<CollectedElementsInformation> CollectElements(List<Position2D> elementsPositions, bool hasToBeGroup = true)
        {
            if (hasToBeGroup)
            {
                if (elementsPositions.Count < 3)
                    return null;
            }

            List<CollectedElementsInformation> result = new();
            foreach (List<Position2D> positions in SplitPositionsByElementTypes(elementsPositions))
            {
                Position2D firstposition = positions[0];
                ElementType collectedType = gameboard[firstposition.X, firstposition.Y].GetComponent<Element>().elementType;
                CollectedElementsInformation collectedElementsInformation = new CollectedElementsInformation(collectedType, 0);

                List<Element> objectsToDestroy = new List<Element>();
                foreach (Position2D p2d in positions)
                {
                    Element go = gameboard[p2d.X, p2d.Y].GetComponent<Element>();
                    collectedElementsInformation.Increment();
                    gameboard[p2d.X, p2d.Y] = null;
                    // zamiast destroy -> zlicz klocki do listy, potem uruchom efekty i wtedy zniszcz klocki
                    objectsToDestroy.Add(go);
                }

                result.Add(collectedElementsInformation);
                ActivateEffectsAndDestroy(objectsToDestroy);
            }

            return result;
        }

        /// <summary>
        /// If elements have some effects, this method activates them, and after that, removes them from gameboard.
        /// </summary>
        /// <param name="gameObjects">elements that should be checked for effects and destroyed</param>
        private void ActivateEffectsAndDestroy(List<Element> gameObjects)
        {
            //foreach (Element go in gameObjects)
            //{
            //    gameboard[go.X, go.Y] = null;
            //}

            foreach (Element go in gameObjects)
            {
                Effect effect = go.Effect;
                if (effect != null)
                {
                    effect.ActivateEffect(this);
                }
                
                Destroy(go.gameObject);
            }
        }

        /// <summary>
        /// Debug method, prints whole gameboard in console.
        /// </summary>
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
