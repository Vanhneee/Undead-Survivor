using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject hudDamageText;

    public EnemyStat stat;
    public Rigidbody2D target;
    bool isLive = true;
    public float skillcool = 8f;
    public int randStat = 50;
    bool useSkill = false;

    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private Animator animator;

    bool ishit = false;
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
        if(dirVec.magnitude >5f && !useSkill)
        {
            Vector3 nextVec = dirVec.normalized * (stat.MoveSpeed * Time.fixedDeltaTime);
            transform.position += nextVec;
            rigid.velocity = Vector2.zero;
        }
        if (dirVec.magnitude < 10f)
        {

            if (useSkill )
            {
                return;
            }
            float rd = UnityEngine.Random.Range(0, 100f);

            if (rd < randStat)
            {
                StartCoroutine(Skill1());
            }
            else
            {
                StartCoroutine(Skill2());
            }
            return;
        }

    }

    private void LateUpdate()
    {
        sprite.flipX = (target.position.x - rigid.position.x < 0) ? false : true;
        if (GameManager.instance.gameWin)
        {
            StartCoroutine(hitAndDead());
        }
    }
    IEnumerator hitAndDead()
    {
        animator.Play("boss_Hit");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        animator.Play("boss_dead");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        OnDead();
    }
    private void OnEnable()
    {
        isLive = true;
    }
    // Skills
    IEnumerator  Skill1()
    {
        useSkill = true;

        Vector3 dir = (target.position - rigid.position);

        Debug.DrawRay(transform.position, dir);
        yield return new WaitForFixedUpdate();
        //print("skill 1");
        rigid.AddForce(dir.normalized * 15f,ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(2f);
        useSkill = false;
    }

    IEnumerator Skill2()
    {   
        print("skill 2");
        useSkill = true;
        yield return new WaitForEndOfFrame();

        GameObject bulletobj = GameManager.instance.pool.Get(6);
        bulletobj.transform.position = rigid.position;
        Bullet bullet = bulletobj.GetComponent<Bullet>();
        Vector3 dir =(target.position - rigid.position).normalized;
        bullet.Init(10, dir,transform, (5f, 10f) , true);
        bulletobj.SetActive(true);

        yield return new WaitForSeconds(1f);
        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(2f);
        useSkill = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null || !collision.CompareTag("Bullet") || collision == null) 
            return;
        
        Bullet b = collision.GetComponent<Bullet>();
        if(!b.parent.CompareTag("Player")) return;
        OnDamaged(b.damage);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.Play("boss_walk");
    }
    #region sub Method
    public void OnDamaged(float damage)
    {
        float calculateDamage = Mathf.Max(damage - stat.Defense, 1);
        stat.HP -= calculateDamage;
        rigid.AddForce((rigid.position - target.position).normalized * 1f);
        animator.Play("boss_Hit");
        if (stat.HP <= 0)
        {
            OnDead();
        }
    }

    public void OnDead()
    {
        isLive = false;
        stat.HP = 0;
        animator.Play("boss_dead");
        gameObject.SetActive(false);
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
    #endregion
}

[Serializable]
public class EnemyStat
{
    public float MoveSpeed = 3.0f;
    public float HP = 100;
    public float MaxHP = 100;
    public float Defense = 10;
    public float Damage = 10f;
}

//public class UI_DamageText : MonoBehaviour
//{
//    public float damage;
//}



