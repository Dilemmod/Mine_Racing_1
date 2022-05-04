using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class UIAudioManager : MonoBehaviour
{
    [SerializeField] private UiSound[] sounds;
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    public AudioMixerGroup GetAudioMixerGroup()
    {
        return audioMixerGroup;
    }

    public static UIAudioManager Instance;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        foreach(UiSound s in sounds)
        {
            if (s != null)
            {
                s.AudioSource = gameObject.AddComponent<AudioSource>();
                s.AudioSource.clip = s.AudioClip;
                s.AudioSource.volume = s.Volume;
                s.AudioSource.pitch = s.Pitch;
                s.AudioSource.loop = s.Loop;
               // s.AudioSource.playOnAwake = s.PlayOnAwake
                s.AudioSource.outputAudioMixerGroup = audioMixerGroup;
            }
        }
        Play(UIClipName.BackgroundMusic);
    }
    public UiSound GetClip(UIClipName name)
    {
        UiSound sound =  Array.Find(sounds, s => s.ClipName == name);
        if (sound != null)
            return sound;
        else
        {
            Debug.LogError("Wrong name of clip = " + name);
            return sounds[0];
        }
    }
    public void Play(UIClipName name)
    {
        GetClip(name).AudioSource.Play();
    }
}
public enum UIClipName
{
    Play,
    LvlMenu,
    Reset,
    Settings,
    Quit,
    BackgroundMusic,
    Сongratulations,
    Engine,
    Drowned,
    Accident,
    Coin_5,
    Coin_25,
    Coin_100,
    Fuel,
    Block_25,
    Block_100,
    Block_500

}
