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
	
	void Start()
	{
		backgroundFadeOutObject.SetActive(true);
		gameManagerScript.fadingToMenu = true;
		StartCoroutine(gameManagerScript.FadeImgOut(backgroundFadeOutImage, (backgroundFadeOutDuration), 0, 0, 0));
		StartCoroutine("StartGame");
	}
	
	public void FadeGameOut()
	{
		
		backgroundFadeOutObject.SetActive(true);
		gameManagerScript.fadingToMenu = true;
		gameManagerScript.TimeScaleNormal();
		StartCoroutine(gameManagerScript.FadeImgIn(backgroundFadeOutImage, backgroundFadeOutDuration, 0, 0, 0));
		StartCoroutine("WaitToChangeScene");
	}
	public void FadeGameToRetry()
	{
		backgroundFadeOutObject.SetActive(true);
		gameManagerScript.fadingToMenu = true;
		gameManagerScript.TimeScaleNormal();
		StartCoroutine(gameManagerScript.FadeImgIn(backgroundFadeOutImage, backgroundFadeOutDuration, 0, 0, 0));
		StartCoroutine("WaitToRetryScene");
	}
	
	public IEnumerator WaitToChangeScene()
	{
		yield return new WaitForSeconds(backgroundFadeOutDuration * 1.7f);
		gameManagerScript.MenuScene();
	}
	public IEnumerator WaitToRetryScene()
	{
		yield return new WaitForSeconds(backgroundFadeOutDuration * 1.7f);
		gameManagerScript.Retry();
	}
	public IEnumerator StartGame()
	{
		yield return new WaitForSeconds(backgroundFadeOutDuration * 1.7f);
		gameManagerScript.TimeScaleNormal();
		backgroundFadeOutObject.SetActive(false);
		gameManagerScript.fadingToMenu = false;
	}
}
