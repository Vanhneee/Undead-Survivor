using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject chua data 
[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Object/SaveData")]
public class GameData : ScriptableObject
{
    public Option option;
    public PlayerSaveData playerSaveData;
    public List<SkillSaveData> skillSaveData;
    public List<EnemySaveData> enemySaveData;
}

