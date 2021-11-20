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
		if(Input.GetKeyDown(KeyCode.UpArrow) && indexButtons > 0 && !restartConfirmationPanelIsUp && !exitConfirmationPanelIsUp)
		{
			indexButtons--;
			UpdatePositions();
		} else if(Input.GetKeyDown(KeyCode.UpArrow) && indexButtons == 0 && !restartConfirmationPanelIsUp && !exitConfirmationPanelIsUp)
		{
			indexButtons = 2;
			UpdatePositions();
		}
		
		if(Input.GetKeyDown(KeyCode.DownArrow) && indexButtons < 2 && !restartConfirmationPanelIsUp && !exitConfirmationPanelIsUp)
		{
			indexButtons++;
			UpdatePositions();
		} else if(Input.GetKeyDown(KeyCode.DownArrow) && indexButtons == 2 && !restartConfirmationPanelIsUp && !exitConfirmationPanelIsUp)
		{
			indexButtons = 0;
			UpdatePositions();
		}
		
		
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) && !restartConfirmationPanelIsUp && !exitConfirmationPanelIsUp)
		{
			if(indexButtons == 0)
			{
				gameManagerScript.PauseUnpauseGame();
			} else if(indexButtons == 1)
			{
				OpenRestartConfirmation();
			} else if(indexButtons == 2)
			{
				OpenExitConfirmation();
			}			
		}
	}
	
	public void OpenRestartConfirmation()
	{
		restartConfirmationPanelIsUp = true;
		restartConfirmationPanel.SetActive(true);
	}
	public void CloseRestartConfirmation()
	{
		restartConfirmationPanelIsUp = false;
		restartConfirmationPanel.SetActive(false);	
	}
	public void OpenExitConfirmation()
	{
		exitConfirmationPanelIsUp = true;
		exitConfirmationPanel.SetActive(true);
	}
	public void CloseExitConfirmation()
	{
		exitConfirmationPanelIsUp = false;
		exitConfirmationPanel.SetActive(false);
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
