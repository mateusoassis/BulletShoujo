using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGameScript : MonoBehaviour
{
	public Transform yesButtonTransform;
	public Button yesButton;
	public Image yesButtonImage;
	
	public Transform noButtonTransform;
	public Button noButton;
	public Image noButtonImage;
	
	[SerializeField] private GameObject exitConfirmationPanel;
	
	public bool overrideMouse;
	
	[SerializeField] private GameManagerScript gameManagerScript;
	
	public int indexExitButtons;
	
	void Start()
    {
		indexExitButtons = 0;
		UpdateExitPositions();
    }
	
	void Update()
    {
		if(Input.GetKeyDown(KeyCode.LeftArrow) && indexExitButtons > 0)
		{
			indexExitButtons--;
			UpdateExitPositions();
		} else if(Input.GetKeyDown(KeyCode.LeftArrow) && indexExitButtons == 0)
		{
			indexExitButtons = 1;
			UpdateExitPositions();
		}
		
		if(Input.GetKeyDown(KeyCode.RightArrow) && indexExitButtons < 1)
		{
			indexExitButtons++;
			UpdateExitPositions();
		} else if(Input.GetKeyDown(KeyCode.RightArrow) && indexExitButtons == 1)
		{
			indexExitButtons = 0;
			UpdateExitPositions();
		}
		
		
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			if(indexExitButtons == 0)
			{
				gameManagerScript.MenuScene();
			} else if(indexExitButtons == 1)
			{
				exitConfirmationPanel.SetActive(false);
			}		
		}
	}
		
	public void UpdateExitPositions()
	{
		overrideMouse = true;
		if(indexExitButtons == 0)
		{
			transform.position = yesButtonTransform.position;
			yesButton.Select();
			yesButtonImage.color = new Color(1,1,1, 0f);
			noButtonImage.color = new Color(1,1,1, 1f);
			
		} else if(indexExitButtons == 1)
		{
			transform.position = noButtonTransform.position;			
			noButton.Select();
			yesButtonImage.color = new Color(1,1,1, 1f);
			noButtonImage.color = new Color(1,1,1, 0f);
		}
	}
}