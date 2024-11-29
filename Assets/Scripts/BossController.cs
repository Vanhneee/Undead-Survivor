using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject _hudDamageText;

    public EnemyStat stat;
    public Rigidbody2D target;
    bool isLive = true;
    public float skillcool = 8f;
    public int randStat = 50;
    bool useSkill = false;

    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private Animator animator;

    private void Awake()
    {
        Init();
    }

    protected void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isLive)
            return;

        Vector2 dirVec = target.position - rigid.position;
        if (dirVec.magnitude < 3f)
        {
            if (useSkill) return;
            float rd = UnityEngine.Random.Range(0, 100f);

            if (rd < randStat)
            {
                Skill1();
            }
            else
            {
                Skill2();
            }
            return;
        }
        Vector2 nextVec = dirVec.normalized * (stat.MoveSpeed * Time.fixedDeltaTime);
        rigid.MovePosition(rigid.position + nextVec);

    }

    private void LateUpdate()
    {
        sprite.flipX = (target.position.x - rigid.position.x < 0) ? false : true;
    }

    private void OnEnable()
    {
        isLive = true;
        //_stat.HP = _stat.MaxHP;
    }

    void Skill1()
    {
        useSkill = true;
        print("skill 1");
        animator.SetBool("skill1", true);
        animator.SetBool("skill2", false);
    }

    void Skill2()
    {
        useSkill = true;
        print("skill 2");
        animator.SetBool("skill2", true);
        animator.SetBool("skill1", false);
    }

    public void OnDamaged(int damage)
    {
        int calculateDamage = Mathf.Max(damage - stat.Defense, 1);
        stat.HP -= calculateDamage;
        rigid.AddForce((rigid.position - target.position).normalized * 500f);
        FloatDamageText(calculateDamage);

        if (stat.HP <= 0)
        {
            OnDead();
        }
    }

    void FloatDamageText(int damage)
    {
        GameObject hudText = Instantiate(_hudDamageText);
        hudText.transform.position = transform.position + Vector3.up * 1.5f;
        hudText.GetComponent<UI_DamageText>(). damage = damage;
    }

    public void OnDead()
    {
        isLive = false;
        stat.HP = 0;
        animator.Play("Mushromm_Death");
        SpawnExp();
        GameManager.instance.AddExp(10); // Thêm EXP cho người chơi khi Boss chết
    }

    void SpawnExp()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject expGo = Instantiate(GameManager.instance.expPrefab);
            expGo.transform.position = transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0);
        }
    }
}

[Serializable]
public class EnemyStat
{
    public float MoveSpeed = 3.0f;
    public int HP = 100;
    public int MaxHP = 100;
    public int Defense = 10;
}

public class UI_DamageText : MonoBehaviour
{
    public int damage;
}



