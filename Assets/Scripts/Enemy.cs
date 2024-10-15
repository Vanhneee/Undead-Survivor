using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float speed;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D taget;

    bool isLive;

    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer spriter;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {

        if (!isLive)
            return;

        Vector2 dirVec = taget.position - rigid.position; // Tính toán vector hướng tới mục tiêu
        Vector2 nextVec = dirVec.normalized * speed * Time.deltaTime; // Tính toán vector di chuyển

        rigid.MovePosition(rigid.position + nextVec); // Di chuyển Rigidbody2D tới vị trí mới
        rigid.velocity = Vector2.zero; // Đặt vận tốc của Rigidbody2D về 0

        spriter.flipX = taget.position.x < rigid.position.x; // lật trai phai enemy
    }

    void OnEnable()
    {
        taget = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        health -= collision.GetComponent<Bullet>().damage; // máu bị trừ nếu va phải 

        if (health > 0)
        {
            // .. live, Hit Action
        }
        else
        {
            // .. Die
            Dead();
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }




}
