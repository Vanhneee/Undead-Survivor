﻿using System.Collections;
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
        Time.timeScale = 0;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp); 
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        Time.timeScale = 1;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
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
            if (item.weapon != null) continue;
            item.ScanWP_inChildPlayer();
            item.ScanGear_inChildPlayer();
          
        }

        // Xác định loại vũ khí hiện tại mà nhân vật đang sử dụng
        Weapon currentWeapon = GameManager.instance.player.GetComponentInChildren<Weapon>();

        // 2. Kích hoạt ngẫu nhiên 3 item
        HashSet<int> selectedIndices = new HashSet<int>();
        while (selectedIndices.Count < 3)
        {
            int randIndex = Random.Range(0, items.Length);
            Item randItem = items[randIndex];

            // Chỉ thêm item phù hợp vũ khí hiện tại
            if (currentWeapon != null)
            {
                
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
