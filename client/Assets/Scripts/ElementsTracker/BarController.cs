using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the progress of collecting elements.
/// </summary>
public class BarController : MonoBehaviour
{
    public Image barSprite;
    public Image elementSprite;

    private ElementsTrackerData _elementsTrackerData;
    private Color _barColor;

    public Color BarColor { get => _barColor; private set => _barColor = value; }

    /// <summary>
    /// Initializes the bar. Sets its color and data.
    /// </summary>
    /// <param name="elementsTrackerData">Data obcject of specific element type.</param>
    public void Initialize(ElementsTrackerData elementsTrackerData)
    {
        _elementsTrackerData = elementsTrackerData;
        SetBarColor(ElementDataGenerator.GetColor(_elementsTrackerData.ElementType));
    }

    private void Start()
    {
        barSprite.fillAmount = 0f;
    }

    /// <summary>
    /// Sets the color of the bar.
    /// </summary>
    /// <param name="color"></param>
    public void SetBarColor(Color color)
    {
        BarColor = color;
        barSprite.color = BarColor;
    }

    /// <summary>
    /// Calculates how much % of the bar should be filled.
    /// </summary>
    /// <returns></returns>
    private float CalculateFill()
    {
        if (_elementsTrackerData.CurrentElements >= _elementsTrackerData.MaxElements)
            return 1f;
        else
            return Mathf.Min(_elementsTrackerData.CurrentElements / _elementsTrackerData.MaxElements, 1f);
    }

    /// <summary>
    /// Updates state of the bar.
    /// </summary>
    public void UpdateBar()
    {
        float fillPercent = CalculateFill();
        barSprite.fillAmount = fillPercent;
    }
}
