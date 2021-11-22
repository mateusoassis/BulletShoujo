using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartTransform : MonoBehaviour
{
	/*public Transform startButtonTransform;
	public Button startButton;
	
	public Transform optionsButtonTransform;
	public Button optionsButton;
	
	public Transform exitButtonTransform;
	public Button exitButton;
	
	[SerializeField] private GameManagerScript gameManagerScript;
	[SerializeField] private GameObject optionsPanel;
	[SerializeField] private GameObject confirmationPanel;
	
	public int indexButtons;	
	
    void Start()
    {
		gameManagerScript.gameStarted = false;
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
				gameManagerScript.OpenTutorialPopup();
			} else if(indexButtons == 1)
			{
				optionsPanel.SetActive(true);
			} else if(indexButtons == 2)
			{
				confirmationPanel.SetActive(true);
			}
			
		}
	}
    
	public void SelectStart()
	{
		indexButtons = 0;
		UpdatePositions();
	}
	public void SelectOptions()
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
		if(indexButtons == 0)
		{
			transform.position = startButtonTransform.position;
			startButton.Select();
		} else if(indexButtons == 1)
		{
			transform.position = optionsButtonTransform.position;			
			optionsButton.Select();						
		} else if(indexButtons == 2)
		{
			transform.position = exitButtonTransform.position;
			exitButton.Select();
		}
	}*/
}
