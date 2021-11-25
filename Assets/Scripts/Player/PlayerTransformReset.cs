using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformReset : MonoBehaviour
{
	public Transform playerTransform;
    public Transform transformReset;
	public Player playerScript;
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			playerTransform.position = transformReset.position;
			playerScript.StartCoroutine("DamagedReset");
		}
	}
}
