//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class CharacterSelectionUI : MonoBehaviour
//{
//    public List<CanvasGroup> characterIcons; // Danh sách CanvasGroup của các nhân vật
//    public Text characterInfoText; // Text thường để hiển thị thông tin nhân vật

//    private int selectedCharacterIndex = -1;

//    // Hàm được gọi khi click vào nhân vật
//    public void OnCharacterIconClick(int characterIndex)
//    {
//        Debug.Log("Character Icon Clicked: " + characterIndex); // Kiểm tra xem hàm có được gọi không

//        // Kiểm tra characterIndex có hợp lệ không
//        if (characterIndex < 0 || characterIndex >= characterIcons.Count)
//        {
//            Debug.LogWarning("Character index out of range: " + characterIndex);
//            return;
//        }

//        // Làm mờ tất cả các icon trước
//        foreach (var icon in characterIcons)
//        {
//            icon.alpha = 0.5f; // Làm mờ icon để biểu thị không được chọn
//            icon.interactable = false; // Đảm bảo không thể click vào các icon đã mờ
//        }

//        // Làm sáng icon đã chọn
//        characterIcons[characterIndex].alpha = 1.0f;
//        characterIcons[characterIndex].interactable = true; // Đảm bảo icon đã chọn có thể tương tác

//        // Lưu chỉ số nhân vật đã chọn
//        selectedCharacterIndex = characterIndex;

//        // Cập nhật thông tin nhân vật đã chọn
//        UpdateCharacterInfo(characterIndex);
//    }

//    // Hàm cập nhật thông tin của nhân vật đã chọn
//    void UpdateCharacterInfo(int characterIndex)
//    {
//        Debug.Log("Updating character info for index: " + characterIndex); // Kiểm tra xem hàm có được gọi không

//        switch (characterIndex)
//        {
//            case 0:
//                characterInfoText.text = "Thể lực: ★★★★★\nBổ trợ vũ khí công lực: ★★★★☆\nBổ trợ vũ khí tốc độ: ★★☆☆☆\nDi chuyển tốc độ: ★☆☆☆☆";
//                break;
//            case 1:
//                characterInfoText.text = "Thể lực: ★★★☆☆\nBổ trợ vũ khí công lực: ★★★★☆\nBổ trợ vũ khí tốc độ: ★★★☆☆\nDi chuyển tốc độ: ★★☆☆☆";
//                break;
//            // Thêm thông tin cho các nhân vật tiếp theo nếu có
//            default:
//                characterInfoText.text = "Thông tin nhân vật chưa có.";
//                break;
//        }
//    }

//    // Xác nhận lựa chọn
//    public void ConfirmSelection()
//    {
//        if (selectedCharacterIndex != -1)
//        {
//            Debug.Log("Character selected: " + selectedCharacterIndex); // Kiểm tra xem hàm có được gọi không
//            // Tiếp tục sang màn hình chọn vũ khí
//            GameManager.instance.selectedCharacterId = selectedCharacterIndex;
//            GameManager.instance.StartWeaponSelection();
//        }
//        else
//        {
//            Debug.LogWarning("Vui lòng chọn một nhân vật trước khi tiếp tục.");
//        }
//    }
//}
