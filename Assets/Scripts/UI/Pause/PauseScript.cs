using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    public Transform unpauseButtonTransform;
	public Button unpauseButton;
	public Image unpauseButtonImage;
	
	public Transform restartButtonTransform;
	public Button restartButton;
	public Image restartButtonImage;
	
	public Transform exitButtonTransform;
	public Button exitButton;
	public Image exitButtonImage;
	
	public bool overrideMouse;
	
	[SerializeField] private GameManagerScript gameManagerScript;
	public GameObject restartConfirmationPanel;
	public bool restartConfirmationPanelIsUp;
	public RestartScript restartScript;
	
	public GameObject exitConfirmationPanel;
	public bool exitConfirmationPanelIsUp;
	
	public int indexButtons;	
	
    void Start()
    {
		indexButtons = 0;
		UpdatePositions();
    }

    void Update()
    {
		if(Input.GetKeyDown(KeyCode.UpArrow) && indexButtons > 0)
		{
			indexButtons--;
			UpdatePositions();
		} else if(Input.GetKeyDown(KeyCode.UpArrow) && indexButtons == 0)
		{
			indexButtons = 2;
			UpdatePositions();
		}
		
		if(Input.GetKeyDown(KeyCode.DownArrow) && indexButtons < 2)
		{
			indexButtons++;
			UpdatePositions();
		} else if(Input.GetKeyDown(KeyCode.DownArrow) && indexButtons == 2)
		{
			indexButtons = 0;
			UpdatePositions();
		}
		
		
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			if(indexButtons == 0)
			{
				gameManagerScript.PauseUnpauseGame();
			} else if(indexButtons == 1)
			{
				OpenRestartConfirmation();
				restartScript.indexRestartButtons = 0;
			} else if(indexButtons == 2)
			{
				OpenExitConfirmation();
			}			
		}
	}
	
	public void OpenRestartConfirmation()
	{
		restartConfirmationPanel.SetActive(true);
		restartConfirmationPanelIsUp = true;
	}
	public void CloseRestartConfirmation()
	{
		restartConfirmationPanel.SetActive(false);
		restartConfirmationPanelIsUp = false;
	}
	public void OpenExitConfirmation()
	{
		exitConfirmationPanel.SetActive(true);
		exitConfirmationPanelIsUp = true;
	}
	public void CloseExitConfirmation()
	{
		exitConfirmationPanel.SetActive(false);
		exitConfirmationPanelIsUp = false;
	}
    
	public void SelectUnpause()
	{
		indexButtons = 0;
		UpdatePositions();
	}
	public void SelectRestart()
	{
		indexButtons = 1;
		UpdatePositions();
	}
	public void SelectExit()
	{
		indexButtons = 2;
		UpdatePositions();
	}
	
	public void UpdatePositions()
	{
		overrideMouse = true;
		if(indexButtons == 0)
		{
			transform.position = unpauseButtonTransform.position;
			unpauseButton.Select();
			unpauseButtonImage.color = new Color(1,1,1, 0f);
			restartButtonImage.color = new Color(1,1,1, 1f);
			exitButtonImage.color = new Color(1,1,1, 1f);
			
		} else if(indexButtons == 1)
		{
			transform.position = restartButtonTransform.position;			
			restartButton.Select();
			unpauseButtonImage.color = new Color(1,1,1, 1f);
			restartButtonImage.color = new Color(1,1,1, 0f);
			exitButtonImage.color = new Color(1,1,1, 1f);
		} else if(indexButtons == 2)
		{
			transform.position = exitButtonTransform.position;
			exitButton.Select();
			unpauseButtonImage.color = new Color(1,1,1, 1f);
			restartButtonImage.color = new Color(1,1,1, 1f);
			exitButtonImage.color = new Color(1,1,1, 0f);
		}
	}
}
