using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterDisplay : MonoBehaviour
{
    public GameObject characterImage;

    public void ChangeImage(Sprite image)
    {
        characterImage.GetComponent<Image>().sprite = image;
    }
}
