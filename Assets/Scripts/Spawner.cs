using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; // Điểm spawn
    public SpawnData[] spawnData;  // Dữ liệu spawn

    private int level;   // Level hiện tại
    private float timer; // Bộ đếm thời gian

    void Start()
    {
        spawnPoint = GetComponentsInChildren<Transform>(); // Lấy điểm spawn
    }

    void Update()
    {
        timer += Time.deltaTime; // Tăng thời gian
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1); // Cập nhật level

        if (timer > spawnData[level].spawnTime) // Kiểm tra thời gian spawn
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0); // Lấy enemy từ pool
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // Đặt vị trí
        enemy.GetComponent<Enemy>().Init(spawnData[level]); // Khởi tạo enemy
    }
}

// Hiển thị các thuộc tính lên Inspector
[System.Serializable]
public class SpawnData
{
    public float spawnTime;  
    public int spriteType;   
    public int health;       
    public float speed;     
}