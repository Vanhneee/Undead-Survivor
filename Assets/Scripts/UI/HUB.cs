using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUB : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health };
    public InfoType type;

    // dung UnityEngine.UI
    Text myText;
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();  
    }

    // need fix
    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float currentExperience = GameManager.instance.exp;
                float maximumExperience = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];

                // Tính tỉ lệ và cập nhật giá trị Slider
                mySlider.value = currentExperience / maximumExperience;
                break;

            case InfoType.Level:
                // Cập nhật văn bản hiển thị cấp độ
                int level = GameManager.instance.level;
                myText.text = "Lv:" + level;
                break;

            case InfoType.Kill:
                // Cập nhật văn bản hiển thị số lượng giết
                int kills = GameManager.instance.kill;
                myText.text = kills.ToString();
                break;

            case InfoType.Time:
                // Tính toán thời gian còn lại
                float remainingTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int minutes = Mathf.FloorToInt(remainingTime / 60);
                int seconds = Mathf.FloorToInt(remainingTime % 60);

                // Cập nhật thời gian
                myText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
                break;

            case InfoType.Health:
                float currentHealth = GameManager.instance.health;
                float maximumHealth = GameManager.instance.maxHealth;

                // Tính tỉ lệ và cập nhật giá trị Slider
                mySlider.value = currentHealth / maximumHealth;
                break;
        }
    }

}
