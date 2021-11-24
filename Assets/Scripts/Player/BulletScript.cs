using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Rigidbody rb;
    private Transform mirror1;
    private Transform mirror2;
    private Player playerScript;
	
	[SerializeField] private BossDamage bossDamage;
	[SerializeField] private PlayerAttributes playerAtt;

    public GameObject explosionPrefab;
    
    private BossMirrorAttack bossMirror;
	
	void Awake()
	{
		
	}
    void Start()
    {
        Destroy(this.gameObject, 3f);
        playerScript = GameObject.Find("Player").GetComponent<Player>();
		playerAtt = GameObject.Find("PlayerAttributes").GetComponent<PlayerAttributes>();
		bossDamage = GameObject.Find("Boss").GetComponent<BossDamage>();
        bossMirror = GameObject.Find("MirrorPoint").GetComponent<BossMirrorAttack>();
        rb = GetComponent<Rigidbody>();
		mirror1 = GameObject.Find("MirrorInside1").GetComponent<Transform>();
        mirror2 = GameObject.Find("MirrorInside2").GetComponent<Transform>();
        //StartCoroutine("TimeToDestroy");
    }

    /*public IEnumerator TimeToDestroy()
    {
        yield return new WaitForSeconds(1.8f);
        Destroy(this.gameObject);
    }*/
	
	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.tag == "Wall")
        {   
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
			Destroy(this.gameObject);
		}

        if(col.gameObject.tag == "Mirror1")
        {
            Debug.Log("TiroBateu1");
            rb.AddForce(mirror1.right * playerScript.bulletForce * -1 * bossMirror.reflectionSpeed, ForceMode.Impulse);
        }

        if(col.gameObject.tag == "Mirror2")
        {
            Debug.Log("TiroBateu2");
            rb.AddForce(mirror2.right * playerScript.bulletForce * bossMirror.reflectionSpeed, ForceMode.Impulse);
        }
		/*if(col.gameObject.tag == "Boss")
        {
            bossDamage.bossHPCurrent--;
			if(playerAtt.currentMana < playerAtt.maxMana){
				playerAtt.currentMana++;
			}			
            Destroy(this.gameObject);
        }*/
	}
}
