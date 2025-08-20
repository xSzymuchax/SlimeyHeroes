using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Every Existing type of element.
/// </summary>
public enum ElementType
{
    FIRE, 
    WATER, 
    LIGHTNING, 
    NATURE
}

/// <summary>
/// Helper class. Helps with managing elements data.
/// </summary>
public static class ElementDataGenerator
{
    private static List<ElementType> _allTypes;

    static ElementDataGenerator()
    {
        _allTypes = GenerateListOfAllTypes();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>List of all element types.</returns>
    private static List<ElementType> GenerateListOfAllTypes()
    {
        return Enum.GetValues(typeof(ElementType)).Cast<ElementType>().ToList();
    }

    /// <summary>
    /// Draws element types form existing types. If drawing more than existing, it returns all existing types.
    /// </summary>
    /// <param name="amount">Amount of types, we wanna draw.</param>
    /// <returns>list of elemnt types</returns>
    public static List<ElementType> DrawElementTypes(int amount)
    {
        if (amount > Enum.GetValues(typeof(ElementType)).Length)
            return _allTypes;

        List<ElementType> allTypes = GenerateListOfAllTypes();
        List<ElementType> selectedTypes = new();

        System.Random rand = new System.Random();
        for (int i = 0; i < amount; i++)
        {
            int pick = rand.Next(0, allTypes.Count);
            selectedTypes.Add(allTypes[pick]);
            allTypes.Remove(allTypes[pick]);
        }

        return selectedTypes;
    }

    /// <summary>
    /// Holds information, about elmenets colors. 
    /// </summary>
    /// <param name="elementType">Type of which we want color</param>
    /// <returns>color of element</returns>
    public static Color GetColor(ElementType elementType)
    {
        Color c = new Color(0,0,0);
        switch (elementType)
        {
            case ElementType.FIRE:
                c = new Color(255, 0, 0);
                break;
            case ElementType.WATER:
                c = new Color(0, 0, 255);
                break;
            case ElementType.NATURE:
                c = new Color(0, 255, 0);
                break;
            case ElementType.LIGHTNING:
                c = new Color(255, 255, 0);
                break;
        }
        return c;
    }

    /// <summary>
    /// Returns array of prefabs, corresponding to list of given element types.
    /// </summary>
    /// <param name="elementTypes"> list of element types.</param>
    /// <returns>Array of prefabs.</returns>
    public static GameObject[] GetElementsPrefabs(List<ElementType> elementTypes)
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefab/Elements");
        GameObject[] result = new GameObject[elementTypes.Count];
        int i = 0;
        
        foreach (ElementType elementType in elementTypes)
        {
            foreach (GameObject go in prefabs)
            {
                ElementType prefabType = go.GetComponent<Element>().elementType;

                if (elementType == prefabType)
                {
                    result[i] = go;
                    break;
                }
            }
            i++;
        }

        Debug.Log(result);
        return result;
    }
}
