using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sound 
{
    public AudioClip AudioClip;

    [Range(0, 1)]
    public float Volume;

    [Range(0, 3)]
    public float Pitch;

    public bool Loop;
}
[Serializable]
public class UiSound:Sound
{
    public UIClipName ClipName;
    [HideInInspector]
    public AudioSource AudioSource;
}
[Serializable]
public class InGameSounds:Sound
{
    public int Priority;
} 
