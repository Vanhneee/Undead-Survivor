using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Object/SaveData")]
public class GameData : ScriptableObject
{
    public Option option;
    public PlayerSaveData playerSaveData;
}

