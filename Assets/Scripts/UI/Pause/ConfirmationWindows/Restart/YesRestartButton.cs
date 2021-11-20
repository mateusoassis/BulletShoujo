using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class YesRestartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public RestartScript restartTransformScript;

    public void OnPointerEnter(PointerEventData eventData)
	{
		if(!restartTransformScript.overrideMouse)
		{
			restartTransformScript.indexRestartButtons = 0;
			restartTransformScript.UpdateRestartPositions();
		}
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		restartTransformScript.overrideMouse = false;
	}
}