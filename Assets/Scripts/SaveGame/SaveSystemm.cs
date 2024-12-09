using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystemm
{
    private static SaveData saveData = new SaveData();

    [System.Serializable]
    public struct SaveData
    {
        public PlayerSaveData PlayerData;
    }

    // tạo file save
    public static string SaveFileName()
    {
        string saveDirectory = "D:\\Đồ Án Tốt Nghiệp\\project Unity\\Undead-Survivor\\MySaves";
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
        string saveFile = saveDirectory + "/save.save";
        return saveFile;
    }

    public static void Save()
    {
        HandleSaveData();

        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(saveData, true)); // PlayerSaveData -> JSON
    }

    private static void HandleSaveData()
    {
        GameManager.instance.player.Save(ref saveData.PlayerData);
    }

    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());

        GameManager.instance.gameData.playerSaveData = JsonUtility.FromJson<SaveData>(saveContent).PlayerData;
        //HandleLoadData();
    }

    private static void HandleLoadData()
    {
        //GameManager.instance.player.Load(saveData.PlayerData);
        Debug.Log(GameManager.instance.player == null ? "Player bị null" : "Player tồn tại");

        if (GameManager.instance.player != null)
        {
            GameManager.instance.player.Load(saveData.PlayerData);
        }
        else
        {
            Debug.LogError("Player không tồn tại! Không thể load dữ liệu.");
        }
    }
}






