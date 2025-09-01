using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class used for displayed character image in the menus.
/// To work, it needs Gameobject called "CharacterImage" in childrens with Image component attached.
/// </summary>
public class CharacterDisplay : MonoBehaviour
{
    private Image _characterImage;

    public void ChangeImage(Sprite sprite)
    {
        _characterImage = transform.Find("CharacterImage").GetComponent<Image>();

        if (sprite == null)
        {
            Debug.LogError("Sprite is null!");
            return;
        }

        if (_characterImage == null)
        {
            Debug.LogError("No Image component assigned!");
            return;
        }

        _characterImage.sprite = sprite;
    }
}
