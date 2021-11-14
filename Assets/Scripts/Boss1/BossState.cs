using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
	public int MOVE_STATE = 1;
	public int MELEE_STATE = 2;
	public int DASH_STATE = 3;
	public int ORBS_STATE = 4;
	public int FRONTAL_ORBS_STATE =5;
	
	/*
	[Header("Tiro 2")]
	public BossFirePattern2 golpe2;
	public float golpe2Duration;
	public float golpe2Delay;

	/*[Header("Swirl Attack")]
	public BossFireSwirl golpe4;
	public float rotSpeed;
	public float golpe4Duration;
	public float golpe4Delay;
	public bool isSpinning;
	public Transform firePoint;*/
	
	private MeleeBoss meleeBoss;
	
	[Header("Check da máquina de STATES")]
	public int currentState;
	public int currentShotState;
	
	[Header("Boss")]
	private Rigidbody bossRb;
	private Transform bossTransform;
	private Transform areaDamage;
	private Transform areaDamageParent;
	
	private Transform bossFirePoint;
	private GameObject areaDamageObject;
	private MeshRenderer areaDamageRenderer;
	private Vector3 verticalAttackScale;
	private Vector3 horizontalAttackScale;
	private BossDamage bossDamage;
	private BossFirePattern bossFrontalShots;
	private BossShotBigOrbs bossOrbs;
	private BossFireSwirl bossFireSwirlScript;

	public bool isCasting;
	
	[Header("Player")]
	private Rigidbody playerRb;
	private Transform playerTransform;
	private Player playerScript;
	private PlayerAttributes playerAttributesScript;
	
	
    // Start is called before the first frame update
    void Start()
    {
		
		playerTransform = GameObject.Find("Player").GetComponent<Transform>();
		playerScript = GameObject.Find("Player").GetComponent<Player>();
		playerAttributesScript = GameObject.Find("Player").GetComponent<PlayerAttributes>();
		bossTransform = GameObject.Find("Boss").GetComponent<Transform>();
		areaDamage = GameObject.Find("AreaDamage").GetComponent<Transform>();
		areaDamageParent = GameObject.Find("ScalingAreaDamage").GetComponent<Transform>();
		areaDamageObject = GameObject.Find("AreaDamage");
		areaDamageRenderer = GameObject.Find("AreaDamage").GetComponent<MeshRenderer>();
		meleeBoss = GameObject.Find("BossManager").GetComponent<MeleeBoss>();
		bossOrbs = GameObject.Find("BossManager").GetComponent<BossShotBigOrbs>();
		bossFrontalShots = GameObject.Find("BossManager").GetComponent<BossFirePattern>();
		bossFirePoint = GameObject.Find("BossFirePoint").GetComponent<Transform>();
		bossFireSwirlScript = GameObject.Find("BossManager").GetComponent<BossFireSwirl>();
		
		isCasting = false;
		//salvando a template do dash
		//StartCoroutine(bossMeleePattern.Dash(this.transform, player.position, 2f));
		ChangeState(MOVE_STATE);
    }
	
	void Update()
	{

		if(currentState == MOVE_STATE)
		{
			meleeBoss.LookAndMove();
		}
	}
			
		//meleeBoss.LookAndMove();

		
		
		/*// boss olha para o player enquanto tá no state 1 e 2
		if(currentState == 1 || currentState == 2){
			transform.LookAt(player.position, Vector3.up);
		}
		if(currentState == 4){
			transform.Rotate(0, rotSpeed*Time.deltaTime, 0);
		}*//*

		if(playerScript.isDashing)
		{
			boxCol.isTrigger = true;
			StartCoroutine("WaitForCollision");
		}
	}
	
	/*public void ChangeState(int state){
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
				break;*//*
	
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
	}*/
	/*private IEnumerator CancelInvoke4(){
		yield return new WaitForSeconds(golpe4Duration);
		if(!isSpinning){
			golpe4.CancelInvoke("FireSwirl");
			ChangeState(1);
		}
	}*//*
	
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
	
	private IEnumerator ResetIsDashOnCollider()
	{
		yield return new WaitForSeconds(0.1f);
		//bossMeleePattern.isDashOnCollider = false;
	}*/
	/*void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "DashCollider")
		{
			//bossMeleePattern.isDashOnCollider = true;
			Debug.Log("entra");
			StartCoroutine(ResetIsDashOnCollider());
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "DashCollider")
		{
			//bossMeleePattern.isDashOnCollider = false;
			Debug.Log("sai");
		}
	}*//*
	
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
	}*/
	
	// 1 = mover
	// 2 = ataque melee
	// 3 = dash
	
	public void ChangeState(int state)
	{
		switch (state){
			
			case 1:
				
				//golpe1.InvokeRepeating("Fire", 2f, golpe1Delay);
				//StartCoroutine("CancelInvoke1");
				break;
			case 2:
				meleeBoss.StartCoroutine("MeleeAttack");
				//golpe2.InvokeRepeating("Fire", 1f, golpe2Delay);
				//StartCoroutine("CancelInvoke2");
				break;
			case 3:
				StartCoroutine(meleeBoss.Dash(bossTransform, playerTransform.position));
				break;
			case 4:
			    StartCoroutine("CastBigOrbs");
				state = MOVE_STATE;
				break;
			case 5:
				StartCoroutine("CastFireSwirl");
				state = MOVE_STATE;
				break;
			/*case 5:
				isSpinning = true;
				golpe4.Swirl();
				StartCoroutine("CancelInvoke4");
				break;*/
		}
		currentState = state;
	}

	public IEnumerator CastBigOrbs()
	{
		Debug.Log("casting");
		isCasting = true;
		meleeBoss.canBossMove = false;
		yield return new WaitForSeconds(2.0f);
		bossOrbs.BigOrbs(bossOrbs.numberOfProjectiles);
		yield return new WaitForSeconds(2.0f);
		meleeBoss.canBossMove = true;
		isCasting = false;
	}

	public IEnumerator CastFirePattern()
	{
		Debug.Log("casting");
		isCasting = true;
		meleeBoss.canBossMove = false;
		yield return new WaitForSeconds(2.0f);
		bossOrbs.BigOrbs(8);
		yield return new WaitForSeconds(2.0f);
		meleeBoss.canBossMove = true;
		isCasting = false;
	}

	public IEnumerator CastFireSwirl()
	{
		Debug.Log("casting");
		isCasting = true;
		meleeBoss.canBossMove = false;
		yield return new WaitForSeconds(2.0f);
		bossFireSwirlScript.Swirl();
		yield return new WaitForSeconds(2.0f);
		meleeBoss.canBossMove = true;
		isCasting = false;
	}
}


