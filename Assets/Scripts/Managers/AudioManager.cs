using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#Sound")]
    public AudioClip soundClip;
    public float soundVolume;
    AudioSource soundPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Dead, Lose, Win, Select, LevelUp, Melle, Range, Bonus, Hit, BMG}

    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        GameObject soundObject = new GameObject("BgmPlayer");
        soundObject.transform.parent = transform;
        soundPlayer = soundObject.AddComponent<AudioSource>();
        soundPlayer.playOnAwake = false;
        soundPlayer.loop = true;
        soundPlayer.volume = soundVolume;
        soundPlayer.clip = soundClip;

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isplay)
    {
        if (isplay)
        {
            soundPlayer.Play();
        }
        else
        {
            soundPlayer.Stop();
        }
    }

    public void PlaySfx (Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++) {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;


            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
    
    // tăng giảm âm lượng
    public void SoundVolume(float volume)
    {
        soundPlayer.volume = Mathf.Clamp(volume, 0f, 1f); 
        soundVolume = volume; 
    }

    public void SfxVolume(float volume)
    {
        sfxVolume = Mathf.Clamp(volume, 0f, 1f); 
        foreach (var player in sfxPlayers)
        {
            player.volume = sfxVolume; 
        }
    }

}





