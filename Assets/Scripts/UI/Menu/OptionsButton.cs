using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsButton : MonoBehaviour, IPointerEnterHandler
{
	public HeartTransform heartTransformScript;

    public void OnPointerEnter(PointerEventData eventData)
	{
		heartTransformScript.indexButtons = 1;
		heartTransformScript.UpdatePositions();
	}
}
