using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }

    private void Next()
    {
        // 1. Tắt tất cả các item
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // Xác định loại vũ khí hiện tại mà nhân vật đang sử dụng
        Weapon currentWeapon = GameManager.instance.player.GetComponentInChildren<Weapon>();

        // 2. Kích hoạt ngẫu nhiên 3 item trong số các item có sẵn
        HashSet<int> selectedIndices = new HashSet<int>();
        while (selectedIndices.Count < 3)
        {
            int randIndex = Random.Range(0, items.Length);
            Item randItem = items[randIndex];

            // Chỉ thêm những item phù hợp với loại vũ khí hiện tại
            if (currentWeapon != null)
            {
                // Nếu nhân vật cầm vũ khí cận chiến (Melee) thì không thêm vũ khí tầm xa (Range) và ngược lại
                if ((currentWeapon.id == (int)ItemData.ItemType.Melee && randItem.data.itemType == ItemData.ItemType.Range) ||
                    (currentWeapon.id == (int)ItemData.ItemType.Range && randItem.data.itemType == ItemData.ItemType.Melee))
                {
                    continue;
                }
            }

            selectedIndices.Add(randIndex);
        }

        // Vòng lặp qua các chỉ số đã chọn và kích hoạt các item tương ứng
        foreach (int index in selectedIndices)
        {
            Item randItem = items[index];

            // 3. Nếu item đã đạt cấp độ tối đa, thay thế bằng một item tiêu thụ (ví dụ: Heal)
            if (randItem.level >= randItem.data.damages.Length)
            {
                // Kích hoạt item tiêu thụ (ví dụ: Heal)
                foreach (Item item in items)
                {
                    if (item.data.itemType == ItemData.ItemType.Heal)
                    {
                        item.gameObject.SetActive(true);
                        break;
                    }
                }
            }
            else
            {
                // Kích hoạt item đã chọn ngẫu nhiên
                randItem.gameObject.SetActive(true);
            }
        }
    }
}
