using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSong : MonoBehaviour
{
	
    void Start() 
	{
        FindObjectOfType<AudioManager>().Play("Song");
    }
}
