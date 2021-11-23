using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour
{
    public AudioManager volManager;

    public Slider genVolSlider;
    public Slider fxVolSlider;

    public Slider sgVolSlider;
    void Start() {
        volManager = GameObject.Find("SoundManager").GetComponent<AudioManager>();

        //Ajusta os Sliders para ficarem no lugar certo
        genVolSlider.value = PlayerPrefs.GetFloat("genVol", 1f);
        sgVolSlider.value = PlayerPrefs.GetFloat("sgVol", 1f);
        fxVolSlider.value = PlayerPrefs.GetFloat("fxVol", 1f);
    }

    public void SetGeneralVol(float genVol){
        volManager.generalMultiplier = genVol;
        PlayerPrefs.SetFloat("genVol", genVol);
    }
    public void SetFXVol(float fxVol){
        volManager.fxMultiplier = fxVol;
        PlayerPrefs.SetFloat("fxVol", fxVol);
    }

    public void SetSongVol(float sgVol){
        volManager.songMultiplier = sgVol;
        PlayerPrefs.SetFloat("sgVol", sgVol);
    }
}
