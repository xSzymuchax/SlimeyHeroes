using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    public GameObject authMenu;
    public GameObject mainMenu;
    public GameObject gameScreen;

    private void Start()
    {
        Instance = this;
    }

    public void ShowMainMenuAfterLogin()
    {
        authMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ShowGameScreenHideMenu()
    {
        mainMenu.SetActive(false);
        gameScreen.SetActive(true);
    }
}
