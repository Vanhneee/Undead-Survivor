using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    UnityEngine.UI.Image icon;
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDesc;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;
        textName.text = data.itemName;
    }

    private void OnEnable()
    {  
        UpdateUI();
    }

    private void UpdateUI()
    {
        textLevel.text = "Lv." + (level + 1);

        switch (data.itemType)
        {   case ItemType.Poison:
            case ItemType.Rake:
            case ItemType.FireBall:
            case ItemType.Melee:
            case ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[Mathf.Min(level, data.damages.Length - 1)] * 100, data.counts[Mathf.Min(level, data.counts.Length - 1)]);
                break;

            case ItemType.Glove:
            case ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[Mathf.Min(level, data.damages.Length - 1)] * 100);
                break;

            default:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            // dame , số lượng
            case ItemType.Poison:
            case ItemType.Rake:
            case ItemType.FireBall:
            case ItemType.Melee:
            case ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();

                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage + data.damages[level];
                    int nextCount = data.counts[level];

                    weapon.skill.levelUp(nextDamage, nextCount);
                }
                level = weapon.skill.count;
                break;

            // Tốc độ di chuyển, Tốc độ bắn
            case ItemType.Glove:
            case ItemType.Shoe:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                level += 1;
                gear.level = level;
                break;

            // Máu 
            case ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
                level = 1;
                break;
            
            // Skill

        } 

        UpdateUI();

        if (level >= data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }

    public void ScanWP_inChildPlayer()
    {
        Weapon[] wps = GameManager.instance.player.GetComponentsInChildren<Weapon>();
        foreach (Weapon wp in wps)
        {
            if (wp.skill.type == this.data.itemType) 
            {
                weapon = wp;
                level = wp.skill.count;
                return;
            }
        }
    }
    public void ScanGear_inChildPlayer()
    {
        Gear[] gears = GameManager.instance.player.GetComponentsInChildren<Gear>();
        foreach (Gear gear in gears)
        {
            if (gear.type == this.data.itemType)
            {
                this.gear = gear;
                level = gear.level;
                return;
            }
        }
    }
}
