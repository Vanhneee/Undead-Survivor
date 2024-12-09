using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider soundSlider, sfxSlider;

    public void SoundVolume()
    {
        AudioManager.instance.SoundVolume(soundSlider.value);
    }
    public void SfxVolume()
    {
        AudioManager.instance.SfxVolume(sfxSlider.value);
    }
}
