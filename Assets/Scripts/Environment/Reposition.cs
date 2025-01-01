using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Map
public class Reposition : MonoBehaviour
{
    private Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        // Xác định hướng di chuyển của nhân vật theo trục dirX và diry
        Vector3 playerDir = GameManager.instance.player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            // di chuyển Ground
            case "Ground":
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 80);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 80);
                }
                break;
            case "Enemy":
                if (coll.enabled)
                    // Di chuyển đối tượng theo hướng của người chơi với tốc độ 20 và thêm một độ dịch ngẫu nhiên
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                break;
        }
    }
}















































































