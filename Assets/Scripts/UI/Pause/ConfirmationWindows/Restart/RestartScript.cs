using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartScript : MonoBehaviour
{
	public Transform yesButtonTransform;
	public Button yesButton;
	public Image yesButtonImage;
	
	public Transform noButtonTransform;
	public Button noButton;
	public Image noButtonImage;
	
	[SerializeField] private GameObject restartConfirmationPanel;
	
	public bool overrideMouse;
	
	[SerializeField] private GameManagerScript gameManagerScript;
	
	public int indexRestartButtons;
	
	public PauseScript pauseScript;
	
	void Start()
    {
		indexRestartButtons = 0;
		UpdateRestartPositions();
    }
	
	void Update()
    {
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			indexRestartButtons--;
			UpdateRestartPositions();
		} else if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			indexRestartButtons++;
			UpdateRestartPositions();
		}		
		
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			Debug.Log("enter sai");
			if(indexRestartButtons == 0)
			{
				gameManagerScript.GameScene();
			} else if(indexRestartButtons == 1)
			{
				//restartConfirmationPanel.SetActive(false);
				//this.transform.parent.gameObject.SetActive(false);
				pauseScript.restartConfirmationPanelIsUp = false;
				this.transform.parent.gameObject.SetActive(false);
				//pauseScript.CloseRestartConfirmation();
				Debug.Log("fecha restart");
			}		
		}
	}
		
	public void UpdateRestartPositions()
	{
		overrideMouse = true;
		if(indexRestartButtons == 0)
		{
			transform.position = yesButtonTransform.position;
			yesButton.Select();
			yesButtonImage.color = new Color(1,1,1, 0f);
			noButtonImage.color = new Color(1,1,1, 1f);
			
		} else if(indexRestartButtons == 1)
		{
			transform.position = noButtonTransform.position;			
			noButton.Select();
			yesButtonImage.color = new Color(1,1,1, 1f);
			noButtonImage.color = new Color(1,1,1, 0f);
		}
	}
}