using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class ZigZagLayout : MonoBehaviour
{
    public float offsetY = 20f;

    void LateUpdate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i) as RectTransform;
            if (child != null)
            {
                // pobierz pozycj� ustawion� przez LayoutGroup
                Vector2 pos = child.anchoredPosition;

                // wymu� sta�y offset zamiast dodawania go w k�ko
                pos.y = (i % 2 == 0) ? 0f : offsetY;

                child.anchoredPosition = pos;
            }
        }
    }
}
