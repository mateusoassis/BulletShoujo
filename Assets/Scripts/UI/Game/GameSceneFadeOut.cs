using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneFadeOut : MonoBehaviour
{
    public GameManagerScript gameManagerScript;
	
	public float backgroundFadeOutDuration;
	public Image backgroundFadeOutImage;
	public GameObject backgroundFadeOutObject;
	
	public GameObject pauseMenu;
	
	public void FadeGameOut()
	{
		
		backgroundFadeOutObject.SetActive(true);
		gameManagerScript.fadingToMenu = true;
		gameManagerScript.TimeScaleNormal();
		StartCoroutine(gameManagerScript.FadeImgIn(backgroundFadeOutImage, backgroundFadeOutDuration, 0, 0, 0));
		Debug.Log("começou troca pra menu");
		StartCoroutine("WaitToChangeScene");
		Debug.Log("acabou");
	}
	
	/*void Update()
	{
		if(backgroundFadeOutImage.color.a >= 0.99f)
		{
			gameManagerScript.MenuScene();
		}
	}*/
	
	public IEnumerator WaitToChangeScene()
	{
		Debug.Log("esperando");
		yield return new WaitForSeconds(backgroundFadeOutDuration * 1.7f);
		gameManagerScript.MenuScene();
	}
}
