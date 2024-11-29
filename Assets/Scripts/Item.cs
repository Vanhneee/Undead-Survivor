using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    UnityEngine.UI.Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
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
        {   case ItemType.Rake:
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
                break;

            // Máu 
            case ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
            
            // Skill

        }

        level++;
        UpdateUI();

        if (level >= data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
