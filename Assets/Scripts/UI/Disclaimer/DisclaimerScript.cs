using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DisclaimerScript : MonoBehaviour
{
	public float disclaimerTextDuration;
	public float disclaimerTextFadeIn;
	public float disclaimerBackgroundDuration;
	public GameObject disclaimerBackGroundGameObject;
	public TextMeshProUGUI disclaimerText;
	public GameObject disclaimerTextGameObject;
	public Image disclaimerBackground;
	
    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine("FadeInFadeOutText");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public IEnumerator FadeImgOut(Image img, float i, int r, int g, int b)
	{
		for(float n = i; n >= 0; n -= Time.deltaTime/2)
		{
			img.color = new Color(r, g, b, n);
			if(img.color.a <= 0.1f)
			{
				img.color = new Color(r,g,b,0);
				yield break;
			}
			yield return null;
		}
	}
	public IEnumerator FadeImgIn(Image img, float j, int r, int g, int b)
	{
		for(float m = 0; m <= j; m += Time.deltaTime/2)
		{
			img.color = new Color(r, g, b, m);
			yield return null;
		}
	}
	
	public IEnumerator FadeTextOut(TextMeshProUGUI text, float i, int r, int g, int b)
	{
		for(float n = i; n >= 0; n -= Time.deltaTime/2)
		{
			text.color = new Color(r, g, b, n);
			if(text.color.a <= 0.05f)
			{
				text.color = new Color(r,g,b,0);
				MenuScene();
				yield break;
			}
			yield return null;
		}	
	}
	
	public void MenuScene()
	{
		 SceneManager.LoadScene("MenuScene");
	}
	
	public IEnumerator FadeTextIn(TextMeshProUGUI text, float j, int r, int g, int b)
	{
		for(float m = 0; m <= j; m += Time.deltaTime/2)
		{
			text.color = new Color(r, g, b, m);	
			yield return null;			
		}	
	}
	
	public IEnumerator FadeInFadeOutText()
	{
		yield return new WaitForSeconds(2f);
		StartCoroutine(FadeTextIn(disclaimerText, disclaimerTextFadeIn, 1, 1, 1));
		yield return new WaitForSeconds(2f);
		StartCoroutine(FadeTextOut(disclaimerText, disclaimerTextDuration, 1, 1, 1));
		yield return new WaitForSeconds(6f);
		yield return null;
	}
}
