//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Weapon : MonoBehaviour
//{
//    public int id;
//    public int prefabId;
//    public int count;
//    public float damage;
//    public float speed;


//    void Start()
//    {
//        Init();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        switch (id)
//        {
//            case 0:
//                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
//                break;
//            default:
//                break;
//        }

//        // .. Test Code ..
//        if (Input.GetButtonDown("Jump"))
//        {
//            levelUp(5,1);
//        }
//    }

//    public void levelUp(float damage, int count)
//    {
//        this.damage = damage;
//        this.count = count;

//        if(id == 0)
//            Batch();
//    }


//    public void Init()
//    {
//        switch (id)
//        {
//            case 0:
//                speed = 150;
//                Batch();
//                break;
//            default:
//                break;
//        }
//    }

//    void Batch()
//    {
//        for (int index = 0; index < count; index++)
//        {
//            Transform bullet = GameManager.instance.pool.Get(prefabId).transform;

//            bullet.parent = null;
//            bullet.localPosition = Vector3.zero;

//            bullet.parent = transform;

//            // xoay bullet
//            Vector3 rotVec = Vector3.forward * 360 * index / count;
//            bullet.Rotate(rotVec);

//            bullet.Translate(bullet.up * 1.5f, Space.World); // khoảng cách

//            bullet.GetComponent<Bullet>().Init(damage,0); 
//        }
//    }
//}
