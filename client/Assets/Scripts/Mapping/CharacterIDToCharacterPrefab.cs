using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class CharacterIDToCharacterPrefab : MonoBehaviour
{
    public GameObject FireDebugCharacter;
    public GameObject WaterDebugCharacter;
    public GameObject NatureDebugCharacter;
    public GameObject LightningDebugCharacter;
    private static Dictionary<int, GameObject> _map;

    private void Start()
    {
        _map = new Dictionary<int, GameObject>()
        {
            { 1, FireDebugCharacter },
            { 2, WaterDebugCharacter },
            { 3, NatureDebugCharacter },
            { 4, LightningDebugCharacter }
        };
    }

    

    public static GameObject GetCharacterOfId(int id)
    {
        return _map[id];
    }
}
