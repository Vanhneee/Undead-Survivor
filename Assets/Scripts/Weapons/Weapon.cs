using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Skill skill;

    void Update()
    {
        if (!GameManager.instance.isLive || skill == null)
            return;
        
        skill.Excute();
    }
    // setup vũ khí, kĩ năng
    public void Init(ItemData data)
    {
        
        name = "Weapon " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;
        Skill newSkill = data.itemType switch
        {
            ItemType.Range => new Range(data),
            ItemType.FireBall or
            ItemType.Melee => new SpinningSkill(data),
            ItemType.Rake => new Rake(data),
            _=> null,
        };
        skill = newSkill;
        skill.skillObj = this.transform;
        GameManager.instance.player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    //public void SwitchWeapon(ItemData data)
    //{
    //    Skill newSkill = data.itemType switch
    //    {
    //        ItemType.Range => new Range(data),
    //        ItemType.FireBall or
    //        ItemType.Melee => new SpinningSkill(data),
    //        ItemType.Rake => new Rake(data),
    //        _ => null,
    //    };
    //    skill = newSkill;
    //    skill.skillObj = this.transform;
    //}
}
