using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    public void Init(float damage, int per)
    {
        this.damage = damage;
        this.per = per;
    }
    

}

