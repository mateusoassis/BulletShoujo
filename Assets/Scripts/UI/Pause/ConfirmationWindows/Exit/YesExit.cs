using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class YesExit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public ExitGameScript exitTransformScript;

    public void OnPointerEnter(PointerEventData eventData)
	{
		if(!exitTransformScript.overrideMouse)
		{
			exitTransformScript.indexExitButtons = 0;
			exitTransformScript.UpdateExitPositions();
		}
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		exitTransformScript.overrideMouse = false;
	}
}