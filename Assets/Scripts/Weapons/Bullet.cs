using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public Vector3 dir;
    public float speed ;
    bool isLife;
    public float timeLife;

    public void Init(float damage, Vector3 dir , bool isLife = false )
    {
        this.damage = damage;
        this.dir = dir;
        this.isLife = isLife;
        timeLife = Random.Range(5f, 10f);
        speed = Random.Range(15f, 20f);
    }

    private void Update()
    {
        this.transform.position += dir * speed * Time.deltaTime;

        if (!isLife) return;

        timeLife -= Time.deltaTime;
        if(timeLife <= 0  )
        {
            gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || !isLife)
            return;

        gameObject.SetActive(false);
        
    }
}