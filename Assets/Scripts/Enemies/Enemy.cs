﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static Cinemachine.DocumentationSortingAttribute;

public class Enemy : MonoBehaviour
{
    public int prefabID;
    public float health;
    public float maxHealth;
    public float speed;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;
    public GameObject expPrefab; 

    bool isLive;

    private Rigidbody2D rigid;
    private Collider2D coll;
    private Animator anim;
    private SpriteRenderer spriter;
    private WaitForFixedUpdate wait;

    public static event System.Action OnHitPlayer;
    public static event System.Action OnDeath;

    public int level;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    private void Update()
    {
        if(SaveSystem.isSaving && GameManager.instance.enemies.Contains(this)) 
        {
            GameManager.instance.enemies.Remove(this);
            Save(SaveSystem.saveData.enemySaveData);
      
        }
    }

     private void FixedUpdate()
     {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) // hit enemy
            return;

        Vector2 dirVec = target.position - rigid.position; // vector hướng tới mục tiêu
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; // vector di chuyển

        rigid.MovePosition(rigid.position + nextVec); 
        rigid.velocity = Vector2.zero; 
        spriter.flipX = target.position.x < rigid.position.x; 
    }
    private void LateUpdate()
    {
        if (GameManager.instance.gameWin) 
        {
            StartCoroutine(hitAndDead());
        }
    }
    IEnumerator hitAndDead()
    {
        anim.SetBool("Dead", true);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        Death();
    }
    // Alive
    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    // Dead
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet")  || !isLive || collision == null)
            return;
        Bullet b = collision.GetComponent<Bullet>();
        if (!b.parent.CompareTag("Player")) return;
            health -= collision.GetComponent<Bullet>().damage; // máu bị trừ nếu va phải weapon

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            StartCoroutine(KnockBack());
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            Death();
        }
    }

    private void Death()
    {
        isLive = false;
        coll.enabled = false;
        rigid.simulated = false;
        spriter.sortingOrder = 1;
        anim.SetBool("Dead", true);
        OnDeath?.Invoke();
        GameManager.instance.kill++;

        // Thêm xác suất rơi EXP (80%)
        if (expPrefab != null)
        {
            int dropChance = Random.Range(0, 100);
            if (dropChance < 80)
            {
                Instantiate(expPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    // hiệu ứng đòn đánh Player vào Enemy
    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.collider.CompareTag("Player"))
            return;

        OnHitPlayer?.Invoke();
    }


    public virtual void Save(List<EnemySaveData> data)
    {
        EnemySaveData enemy = new EnemySaveData();
        enemy.prefabId = prefabID;
        enemy.position = transform.position;
        enemy.health = health;
        enemy.speed = speed;
        enemy.level = level;

        data.Add(enemy);
    }
    public virtual void Load(EnemySaveData data)
    {
        transform.position = data.position;
        health = data.health;
        speed = data.speed;
        level = data.level;
        anim.runtimeAnimatorController = animCon[data.level];
        this.gameObject.SetActive(true);
    }
}