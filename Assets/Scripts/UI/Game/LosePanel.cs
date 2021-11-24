using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LosePanel : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI loseText;
	public WinOrLoseStrings stringsNow;
	
	void OnEnable()
	{
		Debug.Log("perdeu man");
		int u = Random.Range(0, (stringsNow.loseStrings.Length)+1);
		loseText.text = stringsNow.loseStrings[u];
	}
}