using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("# Game Control")]

    public List<SkillSaveData> skills;
    public ItemData[] itemDatas;
    public List<Enemy> enemies;

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
    public GameObject uiHUB;
    public GameObject uiPause;
    public GameObject uiGameStart;
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
        SaveSystem.isSaved = false;
        SaveSystem.isSaving = false;
    }

    private void Start()
    {
        SaveSystem.saveData.skillSaveData = new List<SkillSaveData>();
        SaveSystem.saveData.gearSaveData = new List<GearSaveData>();
        SaveSystem.saveData.enemySaveData = new List<EnemySaveData>();

        if (uiGameStart == null) return;

        if (gameData.option == Option.NewGame)
        {
            uiGameStart.SetActive(true);
        }
        else
        {
            uiGameStart.SetActive(false);
            uiHUB.SetActive(true);
            uiPause.SetActive(true);
     
            player.gameObject.SetActive(true);
            SaveSystem.Load();
            ContinueGame(gameData.playerSaveData.id);
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
        if (SaveSystem.isSaving == false) 
        {
            enemies = pool.gameObject.GetComponentsInChildren<Enemy>().ToList();
        }  
    }

    public void NewGame(int id)
    {
        playerId = id;
        health = maxHealth;

        player.ChangeCharacter(playerId); // tao nhan vat

        // StartGame
        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2); // lấy vũ khí ( PlayerId % 2 => ( 0 -> 1))
        isLive = true;

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }    

    public void ContinueGame(int id)
    {
        playerId = id;
        health = maxHealth;

        player.ChangeCharacter(playerId); // tao nhan vat

        isLive = true;

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public ItemData GetItemData(ItemType type) 
    {
        ItemData itdt = null;
        foreach (ItemData it in itemDatas) 
        {
            if(it.itemType == type) 
            {
                itdt = it;
            }
        }
        return itdt;
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
        Time.timeScale = 0;

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
        Time.timeScale = 0;

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


}
