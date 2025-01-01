using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using static Cinemachine.DocumentationSortingAttribute;

public enum Option {NewGame, LoadGame }

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        GameManager.instance.gameData.option = Option.NewGame;
        SceneManager.LoadSceneAsync(1);
    }

    public void ContinueGame()
    {
        GameManager.instance.gameData.option = Option.LoadGame;
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
