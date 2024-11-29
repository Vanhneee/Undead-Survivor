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

    //private void Next()
    //{
    //    // 1. Tắt tất cả các item
    //    foreach (Item item in items)
    //    {
    //        item.gameObject.SetActive(false);
    //    }

    //    // Xác định loại vũ khí hiện tại mà nhân vật đang sử dụng
    //    Weapon currentWeapon = GameManager.instance.player.GetComponentInChildren<Weapon>();

    //    // 2. Kích hoạt ngẫu nhiên 3 item trong số các item có sẵn
    //    HashSet<int> selectedIndices = new HashSet<int>();
    //    while (selectedIndices.Count < 3)
    //    {
    //        int randIndex = Random.Range(0, items.Length);
    //        Item randItem = items[randIndex];

    //        // Chỉ thêm những item phù hợp với loại vũ khí hiện tại
    //        if (currentWeapon != null)
    //        {
    //            // Nếu nhân vật cầm vũ khí cận chiến (Melee) thì không thêm vũ khí tầm xa (Range) và ngược lại
    //            if ((currentWeapon.id == (int)ItemType.Melee && randItem.data.itemType == ItemType.Range) ||
    //                (currentWeapon.id == (int)ItemType.Range && randItem.data.itemType == ItemType.Melee))
    //            {
    //                continue;
    //            }
    //        }

    //        selectedIndices.Add(randIndex);
    //    }

    //    // 2. Activate 3 distinct random items among them
    //    int[] rand = new int[3];
    //    while (true)
    //    {
    //        rand[0] = Random.Range(0, items.Length);
    //        rand[1] = Random.Range(0, items.Length);
    //        rand[2] = Random.Range(0, items.Length);

    //        // Ensure that the selected items are distinct
    //        if (rand[0] != rand[1] && rand[1] != rand[2] && rand[0] != rand[2])
    //            break;
    //    }


    //    // Vòng lặp qua các chỉ số đã chọn và kích hoạt các item tương ứng
    //    for (int index = 0; index < rand.Length; index++)
    //    {
    //        Item randItem = items[rand[index]];

    //        // 3. In case of max level item, replace it with a consumable item
    //        if (randItem.level == randItem.data.damages.Length)
    //        {
    //            // Activate the consumable item
    //            items[4].gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            // Activate the randomly selected item
    //            randItem.gameObject.SetActive(true);
    //        }
    //    }
    //}


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
                if ((currentWeapon.skill.id == (int)ItemType.Melee && randItem.data.itemType == ItemType.Range) ||
                    (currentWeapon.skill.id == (int)ItemType.Range && randItem.data.itemType == ItemType.Melee))
                {
                    continue;
                }
            }

            selectedIndices.Add(randIndex);
        }

        // 2. Lấy các chỉ số ngẫu nhiên đã chọn
        int[] rand = new int[3];
        int index = 0;
        foreach (int selectedIndex in selectedIndices)
        {
            rand[index++] = selectedIndex;
        }

        // Vòng lặp qua các chỉ số đã chọn và kích hoạt các item tương ứng
        for (int i = 0; i < rand.Length; i++)
        {
            Item randItem = items[rand[i]];

            // 3. Trong trường hợp item đạt cấp độ tối đa, thay thế bằng item tiêu thụ
            if (randItem.level == randItem.data.damages.Length)
            {
                // Kích hoạt item tiêu thụ
                items[4].gameObject.SetActive(true);
            }
            else
            {
                // Kích hoạt item được chọn ngẫu nhiên
                randItem.gameObject.SetActive(true);
            }
        }
    }

}
