using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Audio; 
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    

    public Sound[] sounds;

    [Range(0f, 1f)]
    public float generalMultiplier;
    [Range(0f, 1f)]
    public float fxMultiplier;
    [Range(0f, 1f)]
    public float songMultiplier; 

    //public AudioMixerGroup audioMixer;
    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        //Ajusta os valores dos sons baseado no prefs, se não tiver prefs, seta em 1 (máx)
        generalMultiplier = PlayerPrefs.GetFloat("genVol", 1f);
        songMultiplier = PlayerPrefs.GetFloat("sgVol", 1f);
        fxMultiplier = PlayerPrefs.GetFloat("fxVol", 1f);        

        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip; 

            if(s.tag == 0){
                s.source.volume = s.volume*generalMultiplier*fxMultiplier;
            } else if (s.tag == 1){
                s.source.volume = s.volume*generalMultiplier*songMultiplier;
            }
            
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }

    }

    private void LateUpdate() {
        foreach(Sound s in sounds){
        if(s.tag == 0){
                s.source.volume = s.volume*generalMultiplier*fxMultiplier;
            } else if (s.tag == 1){
                s.source.volume = s.volume*generalMultiplier*songMultiplier;
            }
        }
    }

    // Update is called once per frame
    public void Play(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s == null){
            Debug.LogWarning("Som "+ name + " escrito errado");
            return;
        }
        s.source.Play();
    }

    public void PlayOneShot(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);

         if(s == null){
            Debug.Log("Som "+ name + " escrito errado");
            return;
        }
        s.source.PlayOneShot(s.clip);
    }
  
}
