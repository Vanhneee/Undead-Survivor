using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject _hudDamageText;

    public EnemyStat _stat;
    public Rigidbody2D _target;
    bool _isLive = true;
    public float _skillcool = 8f;
    public int _randStat = 50;
    bool _useSkill = false;

    private Rigidbody2D _rigid;
    private SpriteRenderer _sprite;
    private Animator _anime;

    private void Awake()
    {
        Init();
    }

    protected void Init()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _anime = GetComponent<Animator>();
        _target = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!_isLive)
            return;

        Vector2 dirVec = _target.position - _rigid.position;
        if (dirVec.magnitude < 3f)
        {
            if (_useSkill) return;
            float rd = UnityEngine.Random.Range(0, 100f);

            if (rd < _randStat)
            {
                Skill1();
            }
            else
            {
                Skill2();
            }
            return;
        }
        Vector2 nextVec = dirVec.normalized * (_stat.MoveSpeed * Time.fixedDeltaTime);
        _rigid.MovePosition(_rigid.position + nextVec);
        
    }

    private void LateUpdate()
    {
        _sprite.flipX = (_target.position.x - _rigid.position.x < 0) ? false : true;
    }

    private void OnEnable()
    {
        _isLive = true;
        //_stat.HP = _stat.MaxHP;
    }

    void Skill1()
    {
        _useSkill = true;
        print("skill 1");
        _anime.SetBool("skill1" , true);
        _anime.SetBool("skill2" , false);
    }

    void Skill2()
    {
        _useSkill = true;
        print("skill 2");
        _anime.SetBool("skill2", true);
        _anime.SetBool("skill1", false);
    }

    public void OnDamaged(int damage)
    {
        int calculateDamage = Mathf.Max(damage - _stat.Defense, 1);
        _stat.HP -= calculateDamage;
        _rigid.AddForce((_rigid.position - _target.position).normalized * 500f);
        FloatDamageText(calculateDamage);

        if (_stat.HP <= 0)
        {
            OnDead();
        }
    }

    void FloatDamageText(int damage)
    {
        GameObject hudText = Instantiate(_hudDamageText);
        hudText.transform.position = transform.position + Vector3.up * 1.5f;
        hudText.GetComponent<UI_DamageText>()._damage = damage;
    }

    public void OnDead()
    {
        _isLive = false;
        _stat.HP = 0;
        _anime.Play("Mushromm_Death");
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
    public int _damage;
}
