using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CopyAlpha : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI avisoText;
	[SerializeField]TextMeshProUGUI disclaimerText;
	
	void Update()
	{
		avisoText.color = disclaimerText.color;
	}
}
