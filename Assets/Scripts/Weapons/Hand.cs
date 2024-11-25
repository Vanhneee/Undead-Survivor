using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Hand : MonoBehaviour
{
    public Transform muzzle;
    public bool isLeft;
    public SpriteRenderer spriteR;
    public Vector3 dirToShoot;
    private SpriteRenderer player;

    private Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    private Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    private Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    private Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    private Transform playerPos;
    private void Awake()
    {
        SpriteRenderer[] parentRenderers = GetComponentsInParent<SpriteRenderer>();
        playerPos = transform.parent;
        if (parentRenderers.Length > 1)
        {
            player = parentRenderers[1];
        }
        else
        {
            Debug.LogError("Player SpriteRenderer not found in parent objects!");
        }
    }
    private void Update()
    {
        dirToShoot = (muzzle.position - playerPos.position).normalized;
    }
    private void LateUpdate()
    {
        if (player == null)   
            return;
       

        // Lấy vị trí của trỏ chuột trong thế giới
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;

        // Tính toán hướng từ tay đến trỏ chuột
        Vector3 direction = mousePos - transform.position;
        transform.localRotation = Quaternion.FromToRotation(Vector3.right, direction);

        // Lật sprite dựa vào hướng của chuột
        bool isReverse = direction.x < 0;
        transform.localScale = (isReverse)? new Vector3(1,-1,1) : new Vector3(1, 1, 1);
        spriteR.sortingOrder = isReverse ? 4 : 6;

        // Điều chỉnh vị trí của tay dựa trên hướng chuột
        if (isLeft)
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
        }
        else
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        playerPos = transform.parent;
        Gizmos.DrawRay(new Ray(muzzle.position,(muzzle.position - playerPos.position).normalized));
    }
}
