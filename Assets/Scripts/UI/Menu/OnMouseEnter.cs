using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Transform heartTransform;
	public Transform targetTransform;
	public Transform resetTransform;
	
	public void OnPointerEnter (PointerEventData eventData)
	{
		heartTransform.position = targetTransform.position;
	}
	
	public void OnPointerExit (PointerEventData eventData)
	{
		heartTransform.position = resetTransform.position;
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		// dale
	}
}
