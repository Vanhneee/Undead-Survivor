using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InfiniteMap : MonoBehaviour
{
    public GameObject chunkPrefab; // Prefab cho chunk
    public int chunksVisibleInViewDistance; // Số lượng chunk hiển thị

    private Transform playerTransform;
    private Vector3 lastPlayerPosition;
    private float chunkSize;

    private Queue<GameObject> chunks;

    void Start()
    {
        playerTransform = Camera.main.transform;
        lastPlayerPosition = playerTransform.position;
        chunkSize = 10 * 32f / 32f; // Nếu chunk là 10x10 tiles
        chunks = new Queue<GameObject>();

        // Tạo các chunk ban đầu
        for (int x = -chunksVisibleInViewDistance; x <= chunksVisibleInViewDistance; x++)
        {
            for (int y = -chunksVisibleInViewDistance; y <= chunksVisibleInViewDistance; y++)
            {
                CreateChunk(new Vector3(x * chunkSize, y * chunkSize, 0));
            }
        }
    }

    void Update()
    {
        if (Vector3.Distance(playerTransform.position, lastPlayerPosition) >= chunkSize)
        {
            lastPlayerPosition = playerTransform.position;
            UpdateChunks();
        }
    }

    void CreateChunk(Vector3 position)
    {
        GameObject chunk = Instantiate(chunkPrefab, position, Quaternion.identity);
        chunks.Enqueue(chunk);
    }

    void UpdateChunks()
    {
        // Xóa bỏ các chunk không còn cần thiết
        while (chunks.Count > 0)
        {
            Vector3 chunkPosition = chunks.Peek().transform.position;

            // Kiểm tra khoảng cách giữa người chơi và chunk
            if (Vector3.Distance(playerTransform.position, chunkPosition) > chunksVisibleInViewDistance * chunkSize)
            {
                Destroy(chunks.Dequeue()); // Xóa chunk nếu xa
            }
            else
            {
                break; // Nếu còn chunk gần, dừng kiểm tra
            }
        }

        // Tạo thêm chunk mới nếu cần
        for (int x = -chunksVisibleInViewDistance; x <= chunksVisibleInViewDistance; x++)
        {
            for (int y = -chunksVisibleInViewDistance; y <= chunksVisibleInViewDistance; y++)
            {
                Vector3 newChunkPosition = new Vector3(x * chunkSize, y * chunkSize, 0);
                // Kiểm tra xem có chunk nào tại vị trí này chưa
                bool chunkExists = false;
                foreach (GameObject chunk in chunks)
                {
                    if (chunk.transform.position == newChunkPosition)
                    {
                        chunkExists = true;
                        break;
                    }
                }
                if (!chunkExists)
                {
                    CreateChunk(newChunkPosition); // Tạo chunk mới
                }
            }
        }
    }
}