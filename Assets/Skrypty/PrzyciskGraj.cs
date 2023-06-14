using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrzyciskGraj : MonoBehaviour
{
    public GameObject Menu;
    public GameObject chooseMenu;
    public void onClick()
    {
        Menu.SetActive(false);
        chooseMenu.SetActive(true);
    }
    
    public void onClickExit()
    {
        Application.Quit();
    }

    public void onClickReturnToMenu()
    {
        Menu.SetActive(false);
        SceneManager.LoadScene("Menu");
    }
}
