using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Image barSprite;
    public Image elementSprite;

    private float _maxElements;
    private float _collectedElements = 0f;
    private Color _barColor;
    private ElementType _elementType;

    public float MaxElements { get => _maxElements; private set => _maxElements = value; }
    public float CollectedElements { get => _collectedElements; private set => _collectedElements = value; }
    public Color BarColor { get => _barColor; private set => _barColor = value; }
    public ElementType ElementType { get => _elementType; private set => _elementType = value; }

    private void Start()
    {
        barSprite.fillAmount = 0f;
    }

    public void SetMaxElements(float amount)
    {
        MaxElements = amount;
        UpdateBar();
    }

    public void SetElementType(ElementType elementType)
    {
        _elementType = elementType;
    }

    public void AddCollected(float amount)
    {
        CollectedElements += amount;
        UpdateBar();
    }

    public void SetBarColor(Color color)
    {
        BarColor = color;
        barSprite.color = BarColor;
    }

    private float CalculateFill()
    {
        if (CollectedElements >= MaxElements)
            return 1f;
        else
            return Mathf.Min(CollectedElements / MaxElements, 1f);
    }

    public void UpdateBar()
    {
        float fillPercent = CalculateFill();
        barSprite.fillAmount = fillPercent;
    }
}
