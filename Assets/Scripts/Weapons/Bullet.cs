using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public Vector3 dir;
    public float speed ;
    bool canDead;
    public float timeLife;
    public Transform parent;

    public void Init(float damage, Vector3 dir, Transform parent, (float,float) speed , bool canDead = false)
    {
        this.damage = damage;
        this.dir = dir;
        this.parent = parent;
        this.canDead = canDead;
        timeLife = Random.Range(5f, 10f);
        this.speed = Random.Range(speed.Item1, speed.Item2);
    }

    private void Update()
    {
        this.transform.position += dir * speed * Time.deltaTime;

        if (!canDead) return;

        timeLife -= Time.deltaTime;
        if(timeLife <= 0  )
        {
            gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canDead)
            return;
        if((collision.CompareTag("Enemy") && parent.CompareTag("Player")) || 
            (collision.CompareTag("Player") && (parent.CompareTag("Enemy") || parent.CompareTag("Boss")))
            )
            gameObject.SetActive(false);
        
    }
}