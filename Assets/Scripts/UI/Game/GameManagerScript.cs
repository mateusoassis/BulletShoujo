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
	public bool gameStarted;
	public bool tutorialStarted;
	public TUTORIALMirror tutorialMirror;
	public bool isOptionsPanelUp;
	public GameObject optionsPanelObject;
	
	// toggle da imagem nas bordas
	public GameObject screenBorders;
	public Toggle screenBordersToggle;
	public bool screenBordersOff;
	
	public MenuNavigation menuNavigation;
	public GameSceneFadeOut gameFadeOut;
	
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
	public bool isTutorialPopupUp;
	
	public GameSceneFadeOut gameSceneFadeOut;
	public bool fadingToMenu;
	
	public Scene currentScene;
	public int sceneIndex;
	
	void Awake()
	{
		currentScene = SceneManager.GetActiveScene();
		sceneIndex = currentScene.buildIndex;
		if(PlayerPrefs.HasKey("noTutorial"))
		{
			noTutorial = PlayerPrefs.GetInt("noTutorial") == 1? true: false;
			//tutorialToggle.isOn = true;
		} else
		{
			noTutorial = false;
		}
		
		if(PlayerPrefs.HasKey("screenBordersOff"))
		{
			screenBordersOff = PlayerPrefs.GetInt("screenBordersOff") == 1? true: false;
		} else
		{
			screenBordersOff = false;
		}
	}
	
	// iniciar com booleano falso para não pausar o jogo
	void Start()
	{
		pausedGame = false;
		TimeScaleNormal();
		
		if(currentScene.buildIndex == 1)
		{
			StartCoroutine(FadeImgOut(disclaimerBackground, disclaimerBackgroundDuration, 0, 0, 0));
		}
		if(tutorialStarted)
		{
			tutorialMirror = GameObject.Find("MirrorAttackTutorial").GetComponent<TUTORIALMirror>();
		}
	}
	
	// apertou esc = pausa o jogo, ele pausa e despausa no mesmo botão também
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) && gameStarted && !fadingToMenu)
		{
			PauseUnpauseGame();
		} else if(Input.GetKeyDown(KeyCode.Escape) && tutorialStarted && !fadingToMenu)
		{
			PauseUnpauseGame();
		}
		if(Input.GetKeyDown(KeyCode.Escape) && isOptionsPanelUp && !isTutorialPopupUp && !fadingToMenu)
		{
			CloseOptionsPanel();
			isOptionsPanelUp = false;
		}
		if(Input.GetKeyDown(KeyCode.Escape) && isTutorialPopupUp && !isOptionsPanelUp && !fadingToMenu)
		{
			tutorialPopup.SetActive(false);
			isTutorialPopupUp = false;
		}
	}
	
	public void FromGameToMenuScene()
	{
		gameSceneFadeOut.FadeGameOut();
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
	
	public void ToggleScreenBorder()
	{
		if(screenBordersToggle.isOn)
		{
			screenBordersOff = true;
			PlayerPrefs.SetInt("screenBordersOff", screenBordersOff?1:0);
		} else
		{
			screenBordersOff = false;
			PlayerPrefs.SetInt("screenBordersOff", screenBordersOff?1:0);
		}
	}
	
	public void MenuScene()
	{
		TimeScaleNormal();
		FindObjectOfType<AudioManager>().StopSound("Song");
		SceneManager.LoadScene("MenuScene");
		tutorialStarted = false;
		gameStarted = false;
	}
	
	public void GameScene()
	{
		TimeScaleNormal();
		SceneManager.LoadScene("GameScene");
		tutorialStarted = false;
		gameStarted = true;
	}
	
	public void OpenTutorialPopup()
	{
		if(noTutorial == true)
		{
			isTutorialPopupUp = false;
			GameScene();		
		} else
		{
			tutorialPopup.SetActive(true);
			isTutorialPopupUp = true;
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
	
	public void OpenOptionsPanel()
	{
		isOptionsPanelUp = true;
		optionsPanelObject.SetActive(true);
		tutorialPopup.SetActive(false);
	}
	
	public void CloseOptionsPanel()
	{
		isOptionsPanelUp = false;
		optionsPanelObject.SetActive(false);
	}
	
	// lógica para pausar e despausar no mesmo botão
    public void PauseUnpauseGame()
	{
		if(!pausedGame){
			Time.timeScale = 0f;
			PausePanel.SetActive(true);
			pausedGame = true;
		} else if(pausedGame){
			Time.timeScale = 1f;
			PausePanel.SetActive(false);
			pausedGame = false;
		}		
	}
	
	public IEnumerator FadeImgOut(Image img, float i, int r, int g, int b)
	{
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
