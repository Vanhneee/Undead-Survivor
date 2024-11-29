﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill  
{
    public int id ;
    public int prefabId;
    public int count=1;
    public float damage;
    public float speed;
    public Hand rightHand;
    public float radius;
    public float timer;
    public Player player;
    public Transform skillObj;

    public virtual void levelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data )
    {
        player = GameManager.instance.player;
        // Property Set
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;
        radius = data.radius;

        //Tìm kiếm prefab ID
        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        // Thiết lập tốc độ xoay và các thuộc tính khác
        switch (id)
        {
            case 6:
                speed = 2f;
                break;
            case 5:
                speed = 100;
                //SpinningSkill();
                break;
            case 0:
                speed = -150;
               // SpinningSkill();
                break;
            default:
                speed = 0.8f;
                break;
        }

        // Gán Right Hand tự động tìm theo tag
        if (rightHand == null)
        {
            rightHand = GameObject.FindWithTag("HandRight")?.GetComponent<Hand>();
        }

        if (rightHand != null)
        {
            rightHand.spriteR.sprite = data.hand;
            rightHand.gameObject.SetActive(true);
        }
    }
    public abstract void Excute() ;
    
    


    

}

public class Range : Skill
{
    float timelimit = 1f;

    public Range(ItemData data)
    {
        Init(data);
    }

    public override void Excute()
    {
        timer += Time.deltaTime;
        if (timer < timelimit) return;
        timer = 0f;
        Fire();
    }

    void Fire()
    {
        if (rightHand == null || rightHand.muzzle == null)
        {
            return;
        }

        Vector3 dir = (rightHand.muzzle.position - player.transform.position).normalized;
        dir.z = 0; // Đảm bảo chỉ hoạt động trên mặt phẳng 2D

        //print(dir);
        if (count <= 3)
        {
            //1 dir
            FireRange(dir);
        }
        else
        {
            //tăng dir lv 4 :  3 dir , lv5 : 5 dir : lv6 : 7 dir
            //print((count - 3) * 2);
            FireRange(dir, (count - 3) * 2);
        }


    }

    void FireRange(Vector3 dir, float loop = 0)
    {
        Vector3 dirrec;
        Quaternion euler;
        int e = 1;
        for (int i = 0; i <= loop; i++)
        {
            // Lấy viên đạn từ Object Pool
            GameObject bulletObject = GameManager.instance.pool.Get(prefabId);

            if (bulletObject == null) return;

            e = (i - e == 2) ? i : e;

            euler = Quaternion.Euler(0, 0,
                (i % 2 == 0) ? e * 10f : -e * 10f);

            dirrec = (i > 0) ? euler * dir : dir;

            bulletObject.SetActive(true); // Kích hoạt đối tượng từ Pool
            Transform bullet = bulletObject.transform;
            bullet.position = rightHand.muzzle.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dirrec);
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            bulletComponent.Init(damage, dirrec, true); // Khởi tạo viên đạn
        }
    }
}
public class Rake : Skill
{
    float timelimit = 1f;

    public Rake(ItemData data)
    {
        Init(data);
    }

    public override void Excute()
    {
        timer += Time.deltaTime;
        if (timer < timelimit) return;
        timer = 0f;
        rake();
    }
    void rake()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - skillObj.position;
        dir = dir.normalized;

        // Lấy một viên đạn từ Object Pool
        Transform rake = GameManager.instance.pool.Get(prefabId).transform;

        // khởi tạo góc quay
        rake.position = skillObj.position;

        // tạo skill
        rake.GetComponent<Bullet>().Init(damage, dir, true);
    }
}
public class SpinningSkill : Skill
{
    int curLv = 0;
    public SpinningSkill(ItemData data)
    {
        Init(data);
    }
    public override void Excute()
    {
        Vector3 euler = Vector3.forward * speed * Time.deltaTime;
        euler.z = euler.z % 360f;
        skillObj.Rotate(euler);

        if (curLv == count) return;
        curLv = count;
        spinningSkill();
    }
    // Vũ khí xoay
    void spinningSkill()
    {
        for (int index = 0; index < count; index++)
        {
            Transform weapon;

            // Kiểm tra vũ khí có sẵn hay lấy từ pool
            if (index < skillObj.childCount)
            {
                weapon = skillObj.GetChild(index);
            }
            else
            {
                weapon = GameManager.instance.pool.Get(prefabId).transform;
                weapon.parent = skillObj; // Gán làm con của đối tượng
            }

            // Đặt lại vị trí, góc quay và xoay vũ khí
            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;
            float rotationAngle = 360f * index / count;
            weapon.rotation = Quaternion.Euler(0, 0, rotationAngle);

            // Di chuyển vũ khí ra khỏi vị trí gốc
            weapon.Translate(Vector3.up * radius, Space.Self);

            // Khởi tạo thông số vũ khí
            weapon.GetComponent<Bullet>().Init(damage, Vector3.zero); // -1 là Infinity Per
        }
    }
}