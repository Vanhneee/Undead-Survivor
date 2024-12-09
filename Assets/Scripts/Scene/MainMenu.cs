using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ContinueGame()
    {

        SaveSystemm.Load();

        SceneManager.LoadSceneAsync(1);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
