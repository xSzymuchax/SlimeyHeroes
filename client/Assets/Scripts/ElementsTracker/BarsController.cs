using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Groups Bars.
/// </summary>
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

    /// <summary>
    /// Initializes the TurnBars system. 
    /// </summary>
    /// <param name="elementDatas">Data of each element that will be used to display bars</param>
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

    /// <summary>
    /// Updates bars if the data changed.
    /// </summary>
    public void UpdateBars()
    {
        foreach (BarController barController in bars)
        {
            barController.UpdateBar();
        }
    }
}
