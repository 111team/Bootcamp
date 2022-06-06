using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Canvas : MonoBehaviour
{
   
    public GameObject mainMenu;
    public GameObject options;
    public GameObject quit;
    public GameObject pause;

    public void ChangeMenu(int menuType)
    {
        switch (menuType)
        {
            case 1:
                mainMenu.SetActive(true);
                options.SetActive(false);
                break;

            case 2:
                mainMenu.SetActive(false);
                options.SetActive(true);
                break;

            default:
                break;
        }

    }

    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void PauseMenu()
    {
        Time.timeScale = 0f;
        pause.SetActive(true);
    }

    public void QuitGame()
    {
        //Application.Quit();
        QuitGame();
    }
}
