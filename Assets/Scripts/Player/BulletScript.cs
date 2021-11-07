using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Rigidbody rb;
    private Transform mirror1;
    private Transform mirror2;
    private Player playerScript;
    
    private BossMirrorAttack bossMirror;
    void Start()
    {
        mirror1 = GameObject.Find("MirrorInside1").GetComponent<Transform>();
        mirror2 = GameObject.Find("MirrorInside2").GetComponent<Transform>();
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        bossMirror = GameObject.Find("MirrorAttack").GetComponent<BossMirrorAttack>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine("TimeToDestroy");
    }

    public IEnumerator TimeToDestroy()
    {
        yield return new WaitForSeconds(1.8f);
        Destroy(this.gameObject);
    }
	
	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.tag == "Wall"){
			Destroy(this.gameObject);
		}

        if(col.gameObject.tag == "Mirror1")
        {
            Debug.Log("TiroBateu");
            rb.AddForce(mirror1.right * playerScript.bulletForce * -1 * bossMirror.reflectionSpeed, ForceMode.Impulse);
        }

        if(col.gameObject.tag == "Mirror2")
        {
            Debug.Log("TiroBateu");
            rb.AddForce(mirror2.right * playerScript.bulletForce * bossMirror.reflectionSpeed, ForceMode.Impulse);
        }
	}

    void OnCollisionEnter(Collision other)
    {

    }
}
