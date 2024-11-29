using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange; // Bán kính
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    void FixedUpdate()
    {
        // Sử dụng CircleCastAll để quét tất cả các mục tiêu trong bán kính `scanRange`
        // Các mục tiêu phải thuộc lớp `targetLayer`
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer); // vị trí, bán kính, hướng, độ dài, mục tiêu

        // tìm mục tiêu gần nhất
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = Mathf.Infinity;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }
        return result;
    }
}
