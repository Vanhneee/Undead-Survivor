using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("# Game Control")]
    public bool isLive;
    public bool gameWin;
    public float gameTime;
    public float maxGameTime;

    [Header("# Player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

    [Header("# Game Object")]
    public GameData gameData;
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    public Result uiResult;

    [Header("Prefab Settings")]
    public GameObject expPrefab;

    [Header("# Character Settings")]
    public RuntimeAnimatorController[] animControllers;


    private void Awake()
    {
        isLive = false;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        if (!isLive)
        {
            return;
        }

        gameTime += Time.deltaTime;

        if (gameTime >= maxGameTime)
        {
            gameTime = maxGameTime;
            gameWin = true;
            GameVictory();
        }

        // save load game
        if (Keyboard.current.numpad0Key.wasReleasedThisFrame)
        {
            SaveSystemm.Save();
        }

        if (Keyboard.current.numpad1Key.wasReleasedThisFrame)
        {
            SaveSystemm.Load();
        }
    }


    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;

        // Gọi hàm ChangeCharacter của Player để thay đổi nhân vật
        player.ChangeCharacter(playerId);

        // Bật nhân vật và bắt đầu trò chơi
        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2);
        isLive = true;
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver() 
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    // kinh nghiệm
    public void AddExp(int amount)
    {
        if (!isLive) 
            return;

        exp += amount;

        while (level < nextExp.Length && exp >= nextExp[level])
        {
            exp -= nextExp[level];
            LevelUp();
        }  
    }

    // Hiển thị giao diện Level Up
    private void LevelUp()
    {
        level++;
        uiLevelUp.Show();
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
