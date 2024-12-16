using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SaveData
{
    public PlayerSaveData playerSaveData;
    public List<SkillSaveData> skillSaveData;
    public List<GearSaveData> gearSaveData;
    public List<EnemySaveData> enemySaveData;

    public void Clear()
    {
        skillSaveData.Clear(); 
        gearSaveData.Clear();
        enemySaveData.Clear();
    }
}

[System.Serializable]
public struct PlayerSaveData
{
    public bool isLive;
    public int id;
    public Vector3 position;
    public float health;
    public int level;
    public int exp;
    public int kill;
    public float time;

}

[System.Serializable]
public struct SkillSaveData
{
    public ItemType type;
    public int prefabId;
    public int level;
    public float damage;
}

[System.Serializable]
public struct GearSaveData
{
    public ItemType type;
    public int level;
    public float rate;
}

[System.Serializable]
public struct EnemySaveData
{
    public int prefabId;
    public Vector3 position; 
    public float health;
    public float speed;
    public int level;
}




