using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
 
public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        if(Input.GetKeyDown(KeyCode.Escape))
            Pause();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void HomeAndSave()
    {
        StartCoroutine(waitToSaved());
    }

    public void HomeNoSave() 
    {
        Time.timeScale = 1;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        SceneManager.LoadScene("Main Menu");
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }


    IEnumerator waitToSaved()
    {
        if(SaveSystem.isSaving == false && SaveSystem.isSaved == false) SaveSystem.Save();

        yield return new WaitUntil(() => GameManager.instance.enemies.Count <= 0);
        yield return new WaitForEndOfFrame();

        if (SaveSystem.isSaving) SaveSystem.writeToFile();

        Time.timeScale = 1;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        SceneManager.LoadScene("Main Menu");
    }
}
