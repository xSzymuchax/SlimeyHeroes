using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Image barSprite;
    public Image elementSprite;

    private ElementsTrackerData _elementsTrackerData;
    private Color _barColor;

    public Color BarColor { get => _barColor; private set => _barColor = value; }

    public void Initialize(ElementsTrackerData elementsTrackerData)
    {
        _elementsTrackerData = elementsTrackerData;

        SetBarColor(ElementDataGenerator.GetColor(_elementsTrackerData.ElementType));
    }

    private void Start()
    {
        barSprite.fillAmount = 0f;
    }

    public void SetBarColor(Color color)
    {
        BarColor = color;
        barSprite.color = BarColor;
    }

    private float CalculateFill()
    {
        if (_elementsTrackerData.CurrentElements >= _elementsTrackerData.MaxElements)
            return 1f;
        else
            return Mathf.Min(_elementsTrackerData.CurrentElements / _elementsTrackerData.MaxElements, 1f);
    }

    public void UpdateBar()
    {
        float fillPercent = CalculateFill();
        barSprite.fillAmount = fillPercent;
    }
}
