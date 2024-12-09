using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game/SaveData")]
public class GameData : ScriptableObject
{
    public PlayerSaveData playerSaveData;
}
