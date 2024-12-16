using UnityEngine;

public class FollowHealth : MonoBehaviour
{
    public Transform player;          // Tham chiếu đến Transform của Player
    public Vector3 offset = new Vector3(); // Độ lệch để thanh máu hiển thị trên đầu Player

    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        if (player == null || canvas == null)
            return;

        Vector3 worldPosition = player.position + offset;

        // Chuyển đổi vị trí thế giới sang vị trí màn hình
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        // Chuyển đổi vị trí màn hình sang vị trí trong Canvas
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.renderMode == RenderMode.ScreenSpaceCamera ? Camera.main : null,
            out canvasPosition
        );

        rectTransform.anchoredPosition = canvasPosition;
    }
}
