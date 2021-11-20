using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NoRestartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public RestartScript restartTransformScript;

    public void OnPointerEnter(PointerEventData eventData)
	{
		if(!restartTransformScript.overrideMouse)
		{
			restartTransformScript.indexRestartButtons = 1;
			restartTransformScript.UpdateRestartPositions();
		}
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		restartTransformScript.overrideMouse = false;
	}
}