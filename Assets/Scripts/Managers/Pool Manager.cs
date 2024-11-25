using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    public GameObject[] prefabs; // Các prefab

    private List<List<GameObject>> pools; // Danh sách chứa các danh sách pool

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        InitializePools();
    }

    // Phương thức khởi tạo pools
    private void InitializePools()
    {
        pools = new List<List<GameObject>>();

        // Tạo danh sách rỗng cho từng prefab
        for (int i = 0; i < prefabs.Length; i++)
        {
            pools.Add(new List<GameObject>());
        }
    }

    // Phương thức Spawn để tạo đối tượng từ pool
    public GameObject Spawn(int index, Vector3 position, Quaternion rotation)
    {
        GameObject obj = Get(index);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    // Phương thức Destroy để vô hiệu hóa đối tượng và trả về pool
    public void Destroy(GameObject obj)
    {
        obj.SetActive(false);
    }

    // Lấy một đối tượng từ pool hoặc tạo mới nếu cần thiết
    public GameObject Get(int index)
    {
        GameObject selectedObject = FindInactiveObject(index);

        // Nếu không tìm thấy, tạo mới đối tượng
        if (selectedObject == null)
        {
            selectedObject = CreateNewObject(index);
        }

        return selectedObject;
    }

    // Tìm đối tượng chưa sử dụng trong pool
    private GameObject FindInactiveObject(int index)
    {
        foreach (var obj in pools[index])
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }

    // Tạo đối tượng mới và thêm vào pool
    private GameObject CreateNewObject(int index)
    {
        var newObj = Instantiate(prefabs[index], transform);
        pools[index].Add(newObj);
        return newObj;
    }
}

