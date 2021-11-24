using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToGame : MonoBehaviour
{
	public GameManagerScript gameManagerScript;
	
	public GameObject goToGameScreen;
	
	void Awake()
	{
		gameManagerScript = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
	}
    
	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Player")
		{
			goToGameScreen.SetActive(true);
		}	
	}
}
