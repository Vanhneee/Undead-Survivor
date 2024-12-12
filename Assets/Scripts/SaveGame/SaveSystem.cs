using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.CompilerServices;

public class SaveSystem
{
    public static SaveData saveData = new SaveData();

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
        saveData.Clear();

        GameManager.instance.player.Save(ref saveData.playerSaveData);
        Weapon[] weapons =  GameManager.instance.player.GetComponentsInChildren<Weapon>();
        if (weapons == null || weapons.Length <= 0) return;
        foreach (Weapon weapon in weapons) 
        {
            weapon.skill.Save(saveData.skillSaveData);
        }
    }
    
    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());
 
        GameManager.instance.gameData.playerSaveData = JsonUtility.FromJson<SaveData>(saveContent).playerSaveData;
        GameManager.instance.gameData.skillSaveData = JsonUtility.FromJson<SaveData>(saveContent).skillSaveData;

        HandleLoadData();
    }

    private static void HandleLoadData()
    {
        if(GameManager.instance.player != null)
        {
            GameManager.instance.player.Load(GameManager.instance.gameData.playerSaveData);
        } 
        
        foreach(SkillSaveData skill in GameManager.instance.gameData.skillSaveData) 
        {
            GameObject obj =  new GameObject();
            Weapon wp = obj.AddComponent<Weapon>();
            wp.Init(GameManager.instance.GetItemData(skill.type));
            wp.skill.Load(skill);
            Debug.Log(skill.type);
        }
    }
}






