using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public int count;
    public float damage;
    public float speed;


    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                break;
            default:
                break;
        }

        // .. Test Code ..
        //if (Input.GetButtonDown("Jump"))
        //{
        //    levelUp(20, 1);
        //}
    }

    public void levelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Batch();
    }


    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150;
                Batch();
                break;
            default:
                break;
        }
    }

    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            Transform weapon;

            // Kiểm tra vũ khí có sẵn hay lấy từ pool
            if (index < transform.childCount)
            {
                weapon = transform.GetChild(index);
            }
            else
            {
                weapon = GameManager.instance.pool.Get(prefabId).transform;
                weapon.parent = transform; // Gán làm con của đối tượng
            }

            // Đặt lại vị trí, góc quay và xoay vũ khí
            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;
            float rotationAngle = 360f * index / count;
            weapon.rotation = Quaternion.Euler(0, 0, rotationAngle);

            // Di chuyển vũ khí ra khỏi vị trí gốc
            weapon.Translate(Vector3.up * 1.5f, Space.Self);

            // Khởi tạo thông số vũ khí
            weapon.GetComponent<Bullet>().Init(damage, -1); // -1 là Infinity Per
        }
    }


}
