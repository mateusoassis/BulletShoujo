using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAreaDamageChild : MonoBehaviour
{
	private MeleeBoss meleeBoss;
	
	void Awake()
	{
		meleeBoss = GameObject.Find("BossManager").GetComponent<MeleeBoss>();
	}
  
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			meleeBoss.isPlayerOnArea = true;
		}
	}
	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			meleeBoss.isPlayerOnArea = false;
		}
	}
}
