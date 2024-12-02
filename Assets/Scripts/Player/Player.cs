using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    public float speed; 
    public Vector2 inputVec;    // Vector lưu trữ hướng di chuyển                              
    public Scanner scanner;     
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;
    private Rigidbody2D rigid;      // Rigidbody2D để di chuyển đối tượng
    private SpriteRenderer spriter; // Để thay đổi hình ảnh nhân vật
    private Animator animator;      // Để điều khiển các animation

    public void Awake()

    {
        // Lấy các thành phần trên đối tượng cụ thể
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable()
    {
        // Đảm bảo rằng GameManager và Animator đã được khởi tạo
        if (GameManager.instance != null && animator != null)
        {
            // Cập nhật AnimatorController của nhân vật dựa trên playerId
            if (GameManager.instance.playerId < animCon.Length)
            {
                animator.runtimeAnimatorController = animCon[GameManager.instance.playerId];
            }
            else
            {
                Debug.LogError("Player ID is out of bounds for animCon array.");
            }
        }
        else
        {
            Debug.LogError("GameManager or Animator is not properly initialized.");
        }
    }


    public void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        
        // Lấy input từ bàn phím (WASD hoặc các phím mũi tên)
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    public void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        // Di chuyển đối tượng dựa trên input và tốc độ di chuyển
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;

        // Di chuyen player den vi tri tiep theo
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        // set animator chuyển động
        animator.SetFloat("Speed", inputVec.magnitude);

        // lật trái phải
        if (inputVec.x != 0) 
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!GameManager.instance.isLive ||  !collision.gameObject.CompareTag("Enemy"))
            return;
        // nhan damge
        GameManager.instance.health -= Time.deltaTime * 10 ;

        if(GameManager.instance.health < 0) 
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

    
}
