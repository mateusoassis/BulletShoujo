using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillReadyScript : MonoBehaviour
{
    void OnDisable() {
        FindObjectOfType<AudioManager>().PlayOneShot("SkillReady");
    }
}
