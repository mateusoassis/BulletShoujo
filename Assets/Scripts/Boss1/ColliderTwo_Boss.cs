using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTwo_Boss : MonoBehaviour
{
    public Transform bossTransform;
	public SphereCollider thisCollider;
	
	public BoxCollider bossCollider;
	
	public Player playerScript;
	
	void Update()
	{
		transform.position = bossTransform.position;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player" && playerScript.isDashing)
		{
			bossCollider.isTrigger = true;
			StartCoroutine(WaitSomeSeconds(0.1f));
		}
	}
	
	/*void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "Player" && !playerScript.isDashing)
		{
			bossCollider.isTrigger = false;
		}
	}*/
	public IEnumerator WaitSomeSeconds(float n)
	{
		yield return new WaitForSeconds(n);
		bossCollider.isTrigger = false;
	}
}
