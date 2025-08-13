using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsController : MonoBehaviour
{
    public BarsController Instance;
    public GameObject barPrefab;
    public float defaultMaxElements = 30f;
    public List<BarController> bars;

    private void Start()
    {
        Instance = this;
    }

    public void InitializeBars(List<ElementType> elementTypes)
    {
        foreach (ElementType elementType in elementTypes)
        {
            GameObject go = Instantiate(barPrefab, transform);
            BarController barController = go.GetComponent<BarController>();
            barController.SetMaxElements(defaultMaxElements);
            barController.SetBarColor(ElementDataGenerator.GetColor(elementType));
            barController.SetElementType(elementType);
            barController.UpdateBar();
            bars.Add(barController);
        }
    }

    public void UpdateBar(CollectedElementsInformation cei)
    {
        foreach (BarController barController in bars)
        {
            if (barController.ElementType == cei.elementType)
            {
                barController.AddCollected(cei.amount);
                break;
            }
        }
    }
}
