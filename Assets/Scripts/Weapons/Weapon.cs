using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Skill skill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive || skill == null)
            return;

        skill.Excute();
    }
    public void Init(ItemData data)
    {
        // Basic Set

        name = "Weapon " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;
        Skill newSkill = data.itemType switch
        {
            ItemType.Range => new Range(data),
            ItemType.FireBall or
            ItemType.Melee => new SpinningSkill(data),
            ItemType.Rake => new Rake(data),
        };
        skill = newSkill;
        skill.skillObj = this.transform;

        GameManager.instance.player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
}
