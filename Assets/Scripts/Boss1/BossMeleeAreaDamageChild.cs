using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAreaDamageChild : MonoBehaviour
{
	public BossMeleePattern bossMeleePattern;
	
	void Awake()
	{
		bossMeleePattern = GameObject.Find("BossManager").GetComponent<BossMeleePattern>();
	}
  
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			bossMeleePattern.isPlayerOnArea = true;
		}
	}
	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			bossMeleePattern.isPlayerOnArea = false;
		}
	}
}
