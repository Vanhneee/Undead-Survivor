using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    public int id;
    public int prefabId;
    public int count = 1;
    public float damage;
    public float speed;
    public Hand rightHand;
    public float radius;
    public float timer;
    public Player player;
    public Transform skillObj;
    public ItemType type;

    // tăng damage, số lượng vũ khí
    public virtual void levelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        player = GameManager.instance.player;
        // Property Set
        type = data.itemType;
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
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

        // tốc độ xoay , bắn
        switch (id)
        {
            case 0:
                speed = -150;
                break;
            case 1:
                speed = 0.8f;
                break;
            case 5:
                speed = 100;
                break;
            case 6:
                speed = 2f;
                break;
            case 8:
                radius = 2;
                break;
        }

        if (rightHand == null)
        {
            rightHand = GameObject.FindWithTag("HandRight")?.GetComponent<Hand>();
        }

        // Đưa vũ khí cho Player
        if (rightHand != null && data.hand != null)
        {
            rightHand.spriteR.sprite = data.hand;
            rightHand.gameObject.SetActive(true);
        }
    }
    public abstract void Excute();


    public virtual void Save(List<SkillSaveData> data) 
    {
        SkillSaveData skill = new SkillSaveData();
        skill.prefabId = prefabId;
        skill.level = count;
        skill.damage = damage;
        skill.type = this.type;

        data.Add(skill);
    }
    public virtual void Load(SkillSaveData data) 
    {
        prefabId = data.prefabId;
        count = data.level;
        damage = data.damage;
    }
  
}

// 4 skill

public class Range : Skill
{
    float timelimit = 1f;

    public Range(ItemData data = null)
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

        if (count <= 3)
        {
            //1 dir
            FireRange(dir);
        }
        else
        {
            //tăng dir lv 4 :  3 dir , lv5 : 5 dir : lv6 : 7 dir
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
            bulletComponent.Init(damage, dirrec,player.transform, (15f, 20f), true); // Khởi tạo viên đạn

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
        }
    }
}

public class Rake : Skill
{
    float timelimit = 1f;

    public Rake(ItemData data = null)
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
        Vector3 dir = (targetPos - skillObj.position).normalized;

        for (int i = 0; i <= count; i++) // Tăng số lượng rake dựa trên count
        {
            // Tính toán góc lệch để phân bổ rake đều xung quanh
            float angle = (360f / count) * i;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 adjustedDir = rotation * dir;

            // Lấy một đối tượng từ Object Pool
            Transform rake = GameManager.instance.pool.Get(prefabId).transform;

            // Thiết lập vị trí và khởi tạo skill
            rake.position = skillObj.position;
            rake.rotation = Quaternion.LookRotation(Vector3.forward,adjustedDir); // xoay đúng hướng
            rake.GetComponent<Bullet>().Init(damage, adjustedDir, player.transform, (15f, 20f), true);
        }
    }

}

public class SpinningSkill : Skill
{
    int curLv = 0;
    public SpinningSkill(ItemData data = null)
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
                weapon.parent = skillObj; 
            }

            // Đặt lại vị trí, góc quay và xoay vũ khí
            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;
            float rotationAngle = 360f * index / count;
            weapon.rotation = Quaternion.Euler(0, 0, rotationAngle);

            // Di chuyển vũ khí ra khỏi vị trí gốc
            weapon.Translate(Vector3.up * radius, Space.Self);

            // Khởi tạo thông số vũ khí
            weapon.GetComponent<Bullet>().Init(damage, Vector3.zero, player.transform,(15f,20f));
        }
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Melle);
    }
}

public class Poison : Skill
{
    private Transform poisonArea;

    public Poison(ItemData data = null)
    {
        Init(data);
    }

    public override void Excute()
    {
        poison();
    }

    void poison()
    {
        for (int index = 0; index < count; index++)
        {
            if (index < skillObj.childCount)
            {
                poisonArea = skillObj.GetChild(index);
            }
            else
            {
                poisonArea = GameManager.instance.pool.Get(prefabId).transform;
                poisonArea.parent = skillObj;
            }

            poisonArea.localPosition = Vector3.zero;

            float scaleMultiplier = 1f + 0.2f * count;
            poisonArea.localScale = Vector3.one * scaleMultiplier;

            poisonArea.GetComponent<Bullet>().Init(damage, Vector3.zero, player.transform, (0, 0));
        }
    }
}



