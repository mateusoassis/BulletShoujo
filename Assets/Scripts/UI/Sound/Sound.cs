using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Audio;

[System.Serializable]
public class Sound
{



    [Range(0, 1)]
    public int tag;
    public string name;
    public AudioClip clip;
    
    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;


     
    [HideInInspector]
    public AudioSource source;


}
