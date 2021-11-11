using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
	//Duração e delay entre os ataques do primeiro padrão de tiros
	[Header("Tiro 1")]
	public BossFirePattern golpe1;
	public float golpe1Duration;
	public float golpe1Delay;
	

	[Header("Tiro 2")]
	public BossFirePattern2 golpe2;
	public float golpe2Duration;
	public float golpe2Delay;
	
	[Header("Body Slam")]
	public BossBodySlam golpe3;
	public float golpe3Duration;
	public float golpe3Delay;
	public bool isBodySlamming;

	/*[Header("Swirl Attack")]
	public BossFireSwirl golpe4;
	public float rotSpeed;
	public float golpe4Duration;
	public float golpe4Delay;
	public bool isSpinning;
	public Transform firePoint;*/
	
	
	[Header("Ignora, André")]
	public Transform player;
	public Rigidbody playerRigidbody;
	public Player playerScript;
	public Vector3 knockbackDirection;
	private BoxCollider boxCol;
	public float knockbackForce;
	
	public int state;
	public int currentState;
	// 1 = tiro pra todo lado
	// 2 = tiro em 180º
	// 3 = tiro mirado no jogador
	
	
    // Start is called before the first frame update
    void Start()
    {
		player = GameObject.Find("Player").GetComponent<Transform>();
		playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
		playerScript = GameObject.Find("Player").GetComponent<Player>();
		golpe1 = GameObject.Find("BossManager").GetComponent<BossFirePattern>();
		golpe2 = GameObject.Find("BossManager").GetComponent<BossFirePattern2>();
		golpe3 = GameObject.Find("BossManager").GetComponent<BossBodySlam>();
		/*golpe4 = GameObject.Find("BossManager").GetComponent<BossFireSwirl>();
		firePoint = GameObject.Find("BossFirePoint").GetComponent<Transform>();*/
		boxCol = GetComponent<BoxCollider>();
		currentState = 1;	
		StartCoroutine("StartBoss");
    }
	
	void Update()
	{
		// boss olha para o player enquanto tá no state 1 e 2
		if(currentState == 1 || currentState == 2){
			transform.LookAt(player.position, Vector3.up);
		}
		/*if(currentState == 4){
			transform.Rotate(0, rotSpeed*Time.deltaTime, 0);
		}*/

		if(playerScript.isDashing)
		{
			boxCol.isTrigger = true;
			StartCoroutine("WaitForCollision");
		}
	}
	
	public void ChangeState(int state){
		switch (state){
			case 1:
				golpe1.InvokeRepeating("Fire", 2f, golpe1Delay);
				StartCoroutine("CancelInvoke1");
				break;
			case 2:
				golpe2.InvokeRepeating("Fire", 1f, golpe2Delay);
				StartCoroutine("CancelInvoke2");
				break;
			// boss não olha pro player no state 3, só grava a posição e olha pra lá
			case 3:
				isBodySlamming = true;
				transform.LookAt(player.position, Vector3.up);
				golpe3.ResetPlayerPosition();
				golpe3.InvokeRepeating("MoveToSlam", 2f, golpe3Delay);
				StartCoroutine("CancelInvoke3");
				break;
			/*case 4:
				isSpinning = true;
				golpe4.Swirl();
				StartCoroutine("CancelInvoke4");
				break;*/

		}
		currentState = state;
	}
	
	// tempo pro boss iniciar qualquer ação
	private IEnumerator StartBoss()
	{
		yield return new WaitForSeconds(3f);
		ChangeState(1);
	}
	
	private IEnumerator CancelInvoke1(){
		yield return new WaitForSeconds(golpe1Duration);
		golpe1.CancelInvoke("Fire");
		ChangeState(2);
	}
	
	private IEnumerator CancelInvoke2(){
		yield return new WaitForSeconds(golpe2Duration);
		golpe2.CancelInvoke("Fire");
		ChangeState(3);
	}
	private IEnumerator CancelInvoke3(){
		yield return new WaitForSeconds(golpe3Duration);
		if(!isBodySlamming){
			golpe3.CancelInvoke("MoveToSlam");
			ChangeState(1);
		}
	}
	/*private IEnumerator CancelInvoke4(){
		yield return new WaitForSeconds(golpe4Duration);
		if(!isSpinning){
			golpe4.CancelInvoke("FireSwirl");
			ChangeState(1);
		}
	}*/
	
	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Player"){
			playerScript.recentlyDamaged = true;
			Debug.Log("Favor não encostar no boss");
			knockbackDirection = transform.position - player.position;
			playerRigidbody.velocity = Vector3.zero;
			StartCoroutine("ImmuneTime");
		}
	}


	
	public IEnumerator ImmuneTime()
    {
        yield return new WaitForSeconds(0.3f);
        playerScript.recentlyDamaged = false;
		//playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.ImpulseMode);
    }

	public IEnumerator WaitForCollision()
	{
		yield return new WaitForSeconds(0.3f);
		boxCol.isTrigger = false;
	}
}
