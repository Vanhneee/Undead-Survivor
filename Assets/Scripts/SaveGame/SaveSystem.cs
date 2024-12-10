using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveSystem
{
    private static SaveData saveData = new SaveData();

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
        GameManager.instance.player.Save(ref saveData.playerSaveData);
    }

    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());
 
        GameManager.instance.gameData.playerSaveData = JsonUtility.FromJson<SaveData>(saveContent).playerSaveData;
        HandleLoadData();
    }

    private static void print(string v)
    {
        throw new NotImplementedException();
    }

    private static void HandleLoadData()
    {
        if(GameManager.instance.player != null)
        {
            GameManager.instance.player.Load(saveData.playerSaveData);
        }        
    }
}






