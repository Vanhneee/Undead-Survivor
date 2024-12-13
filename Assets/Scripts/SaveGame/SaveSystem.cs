using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.CompilerServices;
using static Cinemachine.DocumentationSortingAttribute;

public class SaveSystem
{
    public static SaveData saveData = new SaveData();
    public static bool isSaving = false;
    public static bool isSaved = false;

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
    }

    public static void writeToFile()
    {
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(saveData, true)); // PlayerSaveData -> JSON
        isSaved = true;
    }

    private static void HandleSaveData()
    {
        saveData.Clear();

        GameManager.instance.player.Save(ref saveData.playerSaveData);
        Weapon[] weapons =  GameManager.instance.player.GetComponentsInChildren<Weapon>();
        Gear[] gears = GameManager.instance.player.GetComponentsInChildren<Gear>();
        foreach (Weapon weapon in weapons) 
        {
            weapon.skill.Save(saveData.skillSaveData);
        }
        foreach (Gear gear in gears)
        {
            gear.Save(saveData.gearSaveData);
        }

        isSaving = true;
    }
    
    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());
 
        GameManager.instance.gameData.playerSaveData = JsonUtility.FromJson<SaveData>(saveContent).playerSaveData;
        GameManager.instance.gameData.gearSaveData = JsonUtility.FromJson<SaveData>(saveContent).gearSaveData;
        GameManager.instance.gameData.skillSaveData = JsonUtility.FromJson<SaveData>(saveContent).skillSaveData;
        GameManager.instance.gameData.enemySaveData = JsonUtility.FromJson<SaveData>(saveContent).enemySaveData;

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
        }

        foreach(GearSaveData gear in GameManager.instance.gameData.gearSaveData) 
        {
            GameObject obj =  new GameObject();
            Gear g = obj.AddComponent<Gear>();
            g.Init(GameManager.instance.GetItemData(gear.type));
            g.Load(gear);
        }

        foreach (EnemySaveData enemy in GameManager.instance.gameData.enemySaveData)
        { 
            GameObject e = GameManager.instance.pool.Get(enemy.prefabId); // Lấy enemy từ pool
            Enemy eCtrl = e.GetComponent<Enemy>();
            eCtrl.Load(enemy);  
        }
    }
}






