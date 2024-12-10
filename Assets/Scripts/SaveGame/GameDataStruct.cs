using UnityEngine;

[System.Serializable]
public struct SaveData
{
    public PlayerSaveData playerSaveData;
    public PlayerSkillData playerSkillData;
    public EnemySaveData enemySaveData;
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
public class PlayerSkillData
{

}

[System.Serializable]
public class EnemySaveData
{

}



