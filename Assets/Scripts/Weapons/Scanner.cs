using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange; // ban kinh
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer); // vi tri, ban kinh, huong , do dai, muc tieu
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D taget in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 tagetPos = taget.transform.position;
            float curDiff = Vector3.Distance(myPos, tagetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = taget.transform;
            }
        }
        return result;
    }
}
