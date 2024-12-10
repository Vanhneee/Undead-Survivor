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

    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
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

    public void Save() 
    {
        SaveSystem.Save();
    }
}
