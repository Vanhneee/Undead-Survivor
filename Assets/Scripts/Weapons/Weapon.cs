using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public int count=1;
    public float damage;
    public float speed;
    public Hand rightHand;
    public float radius;
    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        //switch (id)
        //{
        //    case 5:
        //    case 0:
        //        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        //        break;
        //    default:
        //        timer += Time.deltaTime;
        //        if (timer > speed)
        //        {
        //            timer = 0f;
        //            Fire();
        //        }
        //        break;
        //}

        if (id == 0 || id == 5) 
        {
            Vector3 euler = Vector3.forward * speed * Time.deltaTime;
            euler.z = euler.z % 360f;
            transform.Rotate(euler);

            if (id == 5) print(euler);
            return;
        }
        timer += Time.deltaTime;
        if (timer > speed)
        {
            timer = 0f;
            Fire();
        }

    }

    public void levelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0 || id == 5)
            SpinningSkill();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;
        radius = data.radius;

        // Tìm kiếm prefab ID
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
            case 5:
                speed = 100;
                SpinningSkill();
                break;
            case 0:
                speed = -150;
                SpinningSkill();
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
        else
        {
            Debug.LogError("Right Hand is not assigned or could not be found!");
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // Vũ khí xoay
    void SpinningSkill()
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
            weapon.Translate(Vector3.up * radius, Space.Self);

            // Khởi tạo thông số vũ khí
            weapon.GetComponent<Bullet>().Init(damage, Vector3.zero); // -1 là Infinity Per
        }
    }
    void Fire()
    {
        if (rightHand == null || rightHand.muzzle == null)
        {
            return;
        }

        Vector3 dir = (rightHand.muzzle.position - player.transform.position).normalized;
        dir.z = 0; // Đảm bảo chỉ hoạt động trên mặt phẳng 2D

        print(dir);
        if (count <= 3)
        {
            //1 dir
            FireRange(dir);
        }
        else
        {
            //tăng dir lv 4 :  3 dir , lv5 : 5 dir : lv6 : 7 dir
            FireRange(dir,(count - 3)*2 );
        }
        
        
        
        
    }

    void FireRange(Vector3 dir, float loop = 0)
    {
        Vector3 dirrec ;
        int e = 1;
        for (int i = 0; i <= loop ; i++ ) 
        {
            // Lấy viên đạn từ Object Pool
            GameObject bulletObject = GameManager.instance.pool.Get(prefabId);
            
            if (bulletObject == null) return;

            e = (i - e == 2) ? e = i : e;
            Quaternion euler = Quaternion.Euler(0, 0, 10*e * -1f) ;
            dirrec = (i!=0)? euler * dir:dir  ;
            
            bulletObject.SetActive(true); // Kích hoạt đối tượng từ Pool
            Transform bullet = bulletObject.transform;
            bullet.position = rightHand.muzzle.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dirrec);
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            bulletComponent.Init(damage, dirrec , true); // Khởi tạo viên đạn với sát thương, số lần xuyên qua, hướng bắn
        }
    }

    void Knife()
    {
        // If no nearest target is found, do nothing
        if (!player.scanner.nearestTarget)
            return;

        // Calculate direction towards the nearest target
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        // Get a bullet from the pool and fire it towards the target
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, dir);
    }

}
