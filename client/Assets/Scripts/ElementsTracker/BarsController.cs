using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsController : MonoBehaviour
{
    public static BarsController Instance;
    public GameObject barPrefab;
    public float defaultMaxElements = 30f;
    public List<BarController> bars;

    private void Start()
    {
        Instance = this;
    }

    public void InitializeBars(List<ElementsTrackerData> elementDatas)
    {
        foreach (var data in elementDatas)
        {
            GameObject go = Instantiate(barPrefab, transform);
            BarController barController = go.GetComponent<BarController>();
            barController.Initialize(data);
            barController.UpdateBar();
            bars.Add(barController);
        }
    }

    public void UpdateBars()
    {
        foreach (BarController barController in bars)
        {
            barController.UpdateBar();
        }
    }
}
