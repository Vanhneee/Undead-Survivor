using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public PoolManager pool;

    public Transform[] spawnPoint; 
    public SpawnData[] spawnData;

    bool canSpawnBoss = true;

    public  int level;  
    public float timer = 0f; 

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>(); 
        
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime; 
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 120f), spawnData.Length - 1); // 120s spawn new enemy
        
        if (timer > spawnData[level].spawnTime) 
        {
            timer = 0;
            // Spawn quái vật từ level 0 đến level hiện tại
            for (int i = 0; i <= level; i++)
            {
                Spawn(i);
            }
        }

        if(GameManager.instance.gameTime >= GameManager.instance.maxGameTime * 0.8f && canSpawnBoss)
        {
            SpawnBoss(4);
        }
    }

    void Spawn(int level)
    {
        
        GameObject enemy = GameManager.instance.pool.Get(0); // Lấy enemy từ pool
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // Đặt vị trí
        enemy.GetComponent<Enemy>().Init(spawnData[level]); // Khởi tạo enemy
        enemy.SetActive(true);
    }

    void SpawnBoss(int idboss)
    {
        GameObject enemy = GameManager.instance.pool.Get(idboss); // Lấy boss từ pool
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // Đặt vị trí
        enemy.SetActive(true);
        canSpawnBoss = false;
    }

}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;  
    public int spriteType;   
    public int health;       
    public float speed;     
}