using UnityEngine;
using UnityEngine.Rendering;

public enum ItemType { Melee, Range, Glove, Shoe, Heal, FireBall, Rake, Poison}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    public float radius = 0;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;

    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;
}
