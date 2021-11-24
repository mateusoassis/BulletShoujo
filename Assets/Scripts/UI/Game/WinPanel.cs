using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinPanel : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI winText;
	public WinOrLoseStrings stringsNow;
	
	void OnEnable()
	{
		Debug.Log("ganhou man");
		int u = Random.Range(0, (stringsNow.winStrings.Length));
		winText.text = stringsNow.winStrings[u];
	}
}
