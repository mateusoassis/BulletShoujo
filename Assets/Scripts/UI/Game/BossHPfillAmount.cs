using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPfillAmount : MonoBehaviour
{
    public Slider bossHealthSlider;
	public Image imageFilling;
	
    void Update()
    {
        imageFilling.fillAmount = bossHealthSlider.value;
    }
}
