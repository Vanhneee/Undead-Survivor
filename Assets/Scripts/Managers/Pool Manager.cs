﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // Mảng các prefab
    public GameObject[] prefabs;

    // Danh sách để chứa các đối tượng đã được pool
    List<GameObject>[] pools;

    private void Awake()
    {
        // Mảng pools có cùng độ dài với mảng prefabs
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; ++index)
        {
            // Mỗi phần tử của mảng pools là một List<GameObject> mới
            pools[index] = new List<GameObject>();
        }
    }

    // Phương thức để lấy một đối tượng từ pool bằng chỉ số
    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // Gán select bằng đối tượng không hoạt động
                select = item;
                // Kích hoạt đối tượng được chọn
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            // Khởi tạo một đối tượng mới từ prefab tương ứng
            select = Instantiate(prefabs[index], transform);
            // Thêm đối tượng vừa khởi tạo vào pool
            pools[index].Add(select);
        }

        return select;
    }
}


