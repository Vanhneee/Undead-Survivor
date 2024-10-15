using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    public float speed; // Tốc độ di chuyển
    public Vector2 inputVec;    // Vector lưu trữ hướng di chuyển                                                           


    private Rigidbody2D rigid;      // Rigidbody2D để di chuyển đối tượng
    private SpriteRenderer spriter; // Để thay đổi hình ảnh nhân vật
    private Animator animator;      // Để điều khiển các animation

    public void Start()
    {
        // Lấy cac thành phần trên đối tượng cụ thể
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
     
    }

    public void Update()
    {
        // Lấy input từ bàn phím (WASD hoặc các phím mũi tên)
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    public void FixedUpdate()
    {
        // Di chuyển đối tượng dựa trên input và tốc độ di chuyển
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;

        // Di chuyen player den vi tri tiep theo
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        // set animator chuyển động
        animator.SetFloat("Speed", inputVec.magnitude);

        // lật trái phải
        if (inputVec.x != 0) 
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
}
