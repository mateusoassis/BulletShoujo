using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
	[SerializeField]TextMeshProUGUI PauseButtonText;
	[SerializeField]GameObject PausePanel;
	public bool pausedGame;
	
	public float disclaimerTextDuration;
	public float disclaimerTextFadeIn;
	public float disclaimerBackgroundDuration;
	public GameObject disclaimerBackGroundGameObject;
	public TextMeshProUGUI disclaimerText;
	public GameObject disclaimerTextGameObject;
	public Image disclaimerBackground;
	public bool noTutorial;
	public Toggle tutorialToggle;
	public GameObject tutorialPopup;
	
	public Scene currentScene;
	public int sceneIndex;
	
	void Awake()
	{
		currentScene = SceneManager.GetActiveScene();
		sceneIndex = currentScene.buildIndex;
        //DontDestroyOnLoad(gameObject);
		if(PlayerPrefs.HasKey("noTutorial"))
		{
			noTutorial = PlayerPrefs.GetInt("noTutorial") == 1? true: false;
		} else
		{
			noTutorial = false;
		}
	}
	
	// iniciar com booleano falso para não pausar o jogo
	void Start()
	{
		/*
		var foo = true;
		// Save boolean using PlayerPrefs
		PlayerPrefs.SetInt("foo", foo?1:0);
		// Get boolean using PlayerPrefs
		foo = PlayerPrefs.GetInt("foo")==1?true:false;
		
		if(gameStarted)
		{
			StartCoroutine("FadeOut", 15f);
		}
		*/
		pausedGame = false;
		
		if(currentScene.buildIndex == 1)
		{
			StartCoroutine(FadeImgOut(disclaimerBackground, disclaimerBackgroundDuration, 0, 0, 0));
		}	
	}
	
	// apertou esc = pausa o jogo, ele pausa e despausa no mesmo botão também
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)){
			PauseUnpauseGame();
		}
	}
	
	public void ToggleTutorial()
	{
		if(tutorialToggle.isOn)
		{
			noTutorial = true;
			PlayerPrefs.SetInt("noTutorial", noTutorial?1:0);
		} else
		{
			noTutorial = false;
			PlayerPrefs.SetInt("noTutorial", noTutorial?1:0);
		}
		return;
	}
	
	public void MenuScene()
	{
		SceneManager.LoadScene("MenuScene");
		TimeScaleNormal();
	}
	
	public void GameScene()
	{
		SceneManager.LoadScene("GameScene");
		TimeScaleNormal();
	}
	public void OpenTutorialPopup()
	{
		if(!noTutorial)
		{
			tutorialPopup.SetActive(true);
		} else
		{
			GameScene();
		}
	}
	
	public void TutorialScene()
	{
		if(!noTutorial && !tutorialToggle.isOn)
		{
			SceneManager.LoadScene("TutorialScene");
			TimeScaleNormal();
		} else
		{
			SceneManager.LoadScene("GameScene");
			TimeScaleNormal();
		}
		
	}
	public void Retry()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		TimeScaleNormal();
	}
	public void TimeScaleNormal()
	{
		Time.timeScale = 1f;
	}
	
	public void Boss2()
	{
		SceneManager.LoadScene("Boss2");
		TimeScaleNormal();
	}
	
	public void QuitGame()
	{
		Application.Quit();
		TimeScaleNormal();
	}
	
	// lógica para pausar e despausar no mesmo botão
    public void PauseUnpauseGame()
	{
		if(!pausedGame){
			Time.timeScale = 0f;
			//PauseButtonText.text = "Unpause";
			PausePanel.SetActive(true);
			pausedGame = true;
		} else if(pausedGame){
			Time.timeScale = 1f;
			//PauseButtonText.text = "Pause";
			PausePanel.SetActive(false);
			pausedGame = false;
		}		
	}
	
	public IEnumerator FadeImgOut(Image img, float i, int r, int g, int b)
	{
		Debug.Log("funciona");
		for(float n = i; n >= 0; n -= Time.deltaTime/2)
		{
			img.color = new Color(r, g, b, n);
			if(img.color.a <= 0.1f)
			{
				img.color = new Color(r,g,b,0);
				disclaimerBackGroundGameObject.gameObject.SetActive(false);
				img.color = new Color(r,g,b,1);
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
			if(text.color.a <= 0.1f)
			{
				text.color = new Color(r,g,b,0);
				yield break;
			}
			yield return null;
		}
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
