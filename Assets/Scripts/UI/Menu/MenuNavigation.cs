using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    public GameObject tutorialPanel;
	public GameObject optionsPanel;
	public GameObject exitPanel;
	
	public GameObject menuFocus;
	public GameObject optionsPanelFocus;
	public GameObject tutorialPanelFocus;
	public GameObject exitPanelFocus;
	
	//aqui nesse primeiro é o único que tem que entender, o unity é BURRO e não troca o focus sem antes deixar o focus atual como NULO
	public void OpenTutorialPanel()
	{
		tutorialPanel.SetActive(true);
		
		//set focus on NULL
		EventSystem.current.SetSelectedGameObject(null);
		//set focus on correct button
		EventSystem.current.SetSelectedGameObject(tutorialPanelFocus);
	}
	public void CloseTutorialPanel()
	{
		tutorialPanel.SetActive(false);
		
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(menuFocus);
	}
	
	public void OpenOptionsPanel()
	{
		optionsPanel.SetActive(true);
		
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(optionsPanelFocus);
	}
	public void CloseOptionsPanel()
	{
		optionsPanel.SetActive(false);
		
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(menuFocus);
	}
	
	public void OpenExitPanel()
	{
		exitPanel.SetActive(true);
		
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(exitPanelFocus);
	}
	public void closeExitPanel()
	{
		exitPanel.SetActive(false);
		
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(menuFocus);
	}
}
