using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SaveData
{
    public PlayerSaveData playerSaveData;
    public List<SkillSaveData> skillSaveData;
    public List<EnemySaveData> enemySaveData;
}

[System.Serializable]
public struct PlayerSaveData
{
    public int Id;
    public Vector3 Position;
    public float Health;
    public int Level;
    public int Exp;
    public int Kill;
    public float Time;
}

[System.Serializable]
public struct SkillSaveData
{
    public int prefabId;
    public int level;
    public float damage;
}

[System.Serializable]
public struct EnemySaveData
{

}



