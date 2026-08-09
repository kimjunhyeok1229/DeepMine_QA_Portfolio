using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public enum SFXList
{
    Effect_Button,
    Effect_Mining,
    Effect_Mark,
    Effect_GameOver,
    Effect_Item_Score,
    Effect_item_Health,
    Effect_Hit_Player,
    Effect_Boom
}

[Serializable]
public struct BGMSound
{
    public string[] name;
    public AudioClip BGM;
}


public class AudioManager : MonoBehaviour
{
    [Header("SFX Sound")]
    public AudioClip effect_Button_Sound;
    public AudioClip effect_Mining_Sound;
    public AudioClip effect_Mark_Sound;
    public AudioClip effect_GameOver_Sound;
    public AudioClip effect_Item_Score_Sound;
    public AudioClip effect_item_Health_Sound;
    public AudioClip effect_Hit_Player_Sound;
    public AudioClip effect_Boom_Sound;
    [Header("BGM Sound")]
    public BGMSound[] bgm;

    [Header("Audio Group")]
    public AudioSource bgmAudio;
    public GameObject sfxAudioGroup;

    [Header("Other Sound")]
    public AudioClip[] intro_Sound;
    

    public static AudioManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<AudioManager>();
            return m_instance;
        }
    }
    private static AudioManager m_instance;

    private void Awake()
    {
        m_instance = this;
        if (m_instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        /*if (PlayerPrefs.HasKey("BGM"))
        {
            if (PlayerPrefs.HasKey("mute_Bgm") && PlayerPrefs.GetInt("mute_Bgm") == 1)
                bgmAudio.volume = 0f;
            else
                bgmAudio.volume = PlayerPrefs.GetInt("BGM") / 100f;
        }
        else
            bgmAudio.volume = 0.5f;
        if (PlayerPrefs.HasKey("SFX"))
        {
            foreach (AudioSource s in sfxAudioGroup.GetComponentsInChildren<AudioSource>())
            {
                if (PlayerPrefs.GetInt("mute_Sfx") == 1)
                    s.volume = 0f;
                else
                    s.volume = PlayerPrefs.GetInt("SFX") / 100f;
            }
        }
        else
        {
            foreach (AudioSource s in sfxAudioGroup.GetComponentsInChildren<AudioSource>())
            {
                s.volume = 0.5f;
            }
        }*/
    }


    private void Start()
    {
        //시작 시 이벤트를 등록해 줍니다.
        SceneManager.sceneLoaded += LoadedsceneEvent;
    }
    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        foreach (BGMSound b in bgm)
        {
            foreach (string s in b.name)
            {
                if (s == scene.name)
                {
                    if (bgmAudio.clip != b.BGM)
                        bgmAudio.clip = b.BGM;
                    if (!bgmAudio.isPlaying)
                        bgmAudio.Play();
                    return;
                }
            }
        }


    }


    public void Play_Sfx(SFXList sfx)
    {
        AudioSource audio = null;
        //플레이 중이 아닌 채널 탐색
        foreach (AudioSource s in sfxAudioGroup.GetComponentsInChildren<AudioSource>())
        {
            if (!s.isPlaying)
            {
                audio = s;
                break;
            }
        }
        if (audio == null)
            return;

        //클립 변경
        switch (sfx)
        {
            case SFXList.Effect_Button:
                audio.clip = effect_Button_Sound;
                break;
            case SFXList.Effect_Mining:
                audio.clip = effect_Mining_Sound;
                break;
            case SFXList.Effect_Mark:
                audio.clip = effect_Mark_Sound;
                break;
            case SFXList.Effect_GameOver:
                audio.clip = effect_GameOver_Sound;
                break;
            case SFXList.Effect_Item_Score:
                audio.clip = effect_Item_Score_Sound;
                break;
            case SFXList.Effect_item_Health:
                audio.clip = effect_item_Health_Sound;
                break;
            case SFXList.Effect_Hit_Player:
                audio.clip = effect_Hit_Player_Sound;
                break;
            case SFXList.Effect_Boom:
                audio.clip = effect_Boom_Sound;
                break;
        }

        //클립 재생
        audio.Play();
    }


    public void Change_SFX_Volume(int volume)
    {
        foreach (AudioSource s in sfxAudioGroup.GetComponentsInChildren<AudioSource>())
        {
            s.volume = volume / 100f;
        }
    }

    public void Change_BGM_Volume(int volume)
    {
        bgmAudio.volume = volume / 100f;
    }

    public void Play_PressBtnSound()
    {
        Play_Sfx(SFXList.Effect_Button);
    }
    public void Play_IntroSound(int i)
    {
        AudioSource audio = null;
        //플레이 중이 아닌 채널 탐색
        foreach (AudioSource s in sfxAudioGroup.GetComponentsInChildren<AudioSource>())
        {
            if (!s.isPlaying)
            {
                audio = s;
                break;
            }
        }
        if (audio == null)
            return;

        audio.clip = intro_Sound[i];
        audio.Play();
    }
}
