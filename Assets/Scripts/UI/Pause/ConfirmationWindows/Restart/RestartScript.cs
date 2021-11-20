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
	
	void Start()
    {
		indexRestartButtons = 0;
		UpdateRestartPositions();
    }
	
	void Update()
    {
		if(Input.GetKeyDown(KeyCode.LeftArrow) && indexRestartButtons > 0)
		{
			indexRestartButtons--;
			UpdateRestartPositions();
		} else if(Input.GetKeyDown(KeyCode.LeftArrow) && indexRestartButtons == 0)
		{
			indexRestartButtons = 1;
			UpdateRestartPositions();
		}
		
		if(Input.GetKeyDown(KeyCode.RightArrow) && indexRestartButtons < 1)
		{
			indexRestartButtons++;
			UpdateRestartPositions();
		} else if(Input.GetKeyDown(KeyCode.RightArrow) && indexRestartButtons == 1)
		{
			indexRestartButtons = 0;
			UpdateRestartPositions();
		}
		
		
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			if(indexRestartButtons == 0)
			{
				gameManagerScript.GameScene();
			} else if(indexRestartButtons == 1)
			{
				restartConfirmationPanel.SetActive(false);
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