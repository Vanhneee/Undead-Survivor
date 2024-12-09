using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemType type;
    public float rate;

    public void  Init(ItemData data)
    {

        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (type)
        {
            case ItemType.Glove:
                RateUp();
                break;
            case ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    // Toc do weapon (levelup)
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.skill.id)
            {
                case 0:
                    weapon.skill.speed = 150 + (150  * rate);
                    break;
                case 1:
                    weapon.skill.speed = 0.8f + (0.8f * rate);
                    break;
            }
        }
    }

    // Toc do di chuyen (levelup)
    void SpeedUp() 
    {
        float speed = 3 * Character.Speed;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
