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
	
	// iniciar com booleano falso para não pausar o jogo
	void Start()
	{
		pausedGame = false;
	}
	
	// apertou esc = pausa o jogo, ele pausa e despausa no mesmo botão também
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)){
			PauseUnpauseGame();
		}
	}
	
	public void MenuScene()
	{
		 SceneManager.LoadScene("MenuScene");
	}
	
	public void GameScene()
	{
		SceneManager.LoadScene("GameScene");
		TimeScaleNormal();
	}
	public void Retry()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public void TimeScaleNormal()
	{
		Time.timeScale = 1f;
	}
	
	public void Boss2()
	{
		SceneManager.LoadScene("Boss2");
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}
	
	// lógica para pausar e despausar no mesmo botão
    public void PauseUnpauseGame()
	{
		if(!pausedGame){
			Time.timeScale = 0f;
			PauseButtonText.text = "Unpause";
			PausePanel.SetActive(true);
			pausedGame = true;
		} else if(pausedGame){
			Time.timeScale = 1f;
			PauseButtonText.text = "Pause";
			PausePanel.SetActive(false);
			pausedGame = false;
		}		
	}	
}
