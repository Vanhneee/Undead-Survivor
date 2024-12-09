using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float Speed
    {
        get { return GameManager.instance.playerId == 0 ? 1.1f :
                     GameManager.instance.playerId == 2 ? 1.5f :
                   1f; }
    }

    public static float Damage
    {
        get
        {
            return GameManager.instance.playerId == 2 ? 1.5f :
                   1f;
        }
    }
}
