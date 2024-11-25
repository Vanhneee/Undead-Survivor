using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public bool isLive;
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
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;

    [Header("Prefab Settings")]
    public GameObject expPrefab;

    [Header("# Character Settings")]
    public RuntimeAnimatorController[] animControllers;


    private void Awake()
    {
        isLive = true;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
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
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);  

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (!isLive)
        {
            return;
        }

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

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

    private void LevelUp()
    {
        level++;
        Debug.Log("Level Up! Current Level: " + level);
        uiLevelUp.Show();
        // Cập nhật các thuộc tính khác nếu cần (như tăng máu, tăng sức mạnh, v.v.)
    }

    public void GetExp()
    {
    
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
