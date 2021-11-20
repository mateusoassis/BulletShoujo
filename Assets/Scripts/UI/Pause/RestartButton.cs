using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RestartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public PauseScript heartPauseTransformScript;

    public void OnPointerEnter(PointerEventData eventData)
	{
		if(!heartPauseTransformScript.overrideMouse)
		{
			heartPauseTransformScript.indexButtons = 1;
			heartPauseTransformScript.UpdatePositions();
		}
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		heartPauseTransformScript.overrideMouse = false;
	}
}