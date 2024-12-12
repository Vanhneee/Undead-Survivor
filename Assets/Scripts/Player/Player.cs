using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    public float speed; 
    public Vector2 inputVec;    // Vector lưu trữ hướng di chuyển                              
    public Scanner scanner;     
    public Hand[] hands;        // Hiển thị vũ khí
    public RuntimeAnimatorController[] animCon;

    private Rigidbody2D rigid;      
    private SpriteRenderer spriter; 
    private Animator animator;

    public void Awake()

    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable()
    {
        speed *= Character.Speed;
        // Cập nhật AnimatorController của nhân vật
        if (GameManager.instance != null && animator != null &&
            GameManager.instance.playerId < animCon.Length)
        {
            animator.runtimeAnimatorController = animCon[GameManager.instance.playerId];
        }
        else
        {
            Debug.LogError("Invalid GameManager instance, animator, or playerId.");
        }
    }

    public void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    public void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        // di chuyen Player
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        // set animator chuyển động
        animator.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0) 
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!GameManager.instance.isLive || collision == null)
            return;
        // nhận damage
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.health -= Time.deltaTime * 10;
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            BossController boss = collision.gameObject.GetComponent<BossController>();
            GameManager.instance.health -= boss.stat.Damage;
        }

        if(GameManager.instance.health < 0) 
        {
            animator.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.instance.isLive || (!collision.gameObject.CompareTag("Bullet")) || collision == null)
            return;
        Bullet b = collision.GetComponent<Bullet>();
        if (!b.parent.CompareTag("Enemy") && !b.parent.CompareTag("Boss")) return;
        GameManager.instance.health -= b.damage;

        if (GameManager.instance.health < 0)
        {
            animator.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

    
    public void ChangeCharacter(int characterId)
    {
        if (characterId < 0 || characterId >= animCon.Length)
        {
            Debug.LogError("Character ID is out of bounds for animCon array.");
            return;
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // khởi tạo character
        animator.runtimeAnimatorController = animCon[characterId];
    }

    // Phuong thuc Save,Load
    // Lấy dữ liệu từ người chơi (Player), chuyển sang JSON và lưu vào file save.save.
    public void Save(ref PlayerSaveData data)
    {
        data.Id = GameManager.instance.playerId;
        data.Position = transform.position;
        data.Health = GameManager.instance.health; 
        data.Level = GameManager.instance.level; 
        data.Exp = GameManager.instance.exp;
        data.Kill = GameManager.instance.kill;
        data.Time = GameManager.instance.gameTime;
    }

    //Đọc dữ liệu từ file, chuyển JSON -> đối tượng (Player) và Load game
    public void Load(PlayerSaveData data)
    {
        transform.position = data.Position;
        GameManager.instance.health = data.Health; 
        GameManager.instance.level = data.Level; 
        GameManager.instance.exp = data.Exp; 
        GameManager.instance.kill = data.Kill;
        GameManager.instance.gameTime = data.Time;
    }
}



