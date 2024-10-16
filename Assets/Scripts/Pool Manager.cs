using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs; // Các prefab

    private List<GameObject>[] pools; // Mảng các danh sách pool

    private void Awake()
    {
        // Khởi tạo mảng pool
        pools = new List<GameObject>[prefabs.Length];

        // Tạo danh sách rỗng cho mỗi pool
        for (int index = 0; index < pools.Length; ++index)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // Tìm đối tượng chưa sử dụng trong pool
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // Tạo mới nếu không có đối tượng khả dụng
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select; 
    }
}