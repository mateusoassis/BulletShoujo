using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
	public int MOVE_STATE = 1;
	public int MELEE_STATE = 2;
	public int DASH_STATE = 3;
	public int ORBS_STATE = 4;
	public int FIRE_SWIRL_STATE = 5;
	//public int FRONTAL_ORBS_STATE = 6;

	[Header("Swirl Attack")]
	public BossFireSwirl golpe4;
	public float rotSpeed;
	public float golpe4Duration;
	public float golpe4Delay;
	public bool isSpinning;
	public Transform firePoint;
	
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
		
		if(currentState == FIRE_SWIRL_STATE){
            transform.Rotate(0, rotSpeed*Time.deltaTime, 0);
        }
		/*if(playerScript.isDashing)
		{
			boxCol.isTrigger = true;
			StartCoroutine("WaitForCollision");
		}*/
	}
	/*
	
	// tempo pro boss iniciar qualquer ação
	private IEnumerator StartBoss()
	{
		yield return new WaitForSeconds(3f);
		ChangeState(1);
	}*/
	
	/*private IEnumerator CancelInvoke4(){
		yield return new WaitForSeconds(golpe4Duration);
		if(!isSpinning){
			golpe4.CancelInvoke("FireSwirl");
			ChangeState(1);
		}
	}*/
	
	/*private IEnumerator ResetIsDashOnCollider()
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
	}*/
	
	
	/*
	public IEnumerator WaitForCollision()
	{
		yield return new WaitForSeconds(0.3f);
		boxCol.isTrigger = false;
	}*/
	
	//public int MOVE_STATE = 1;
	//public int MELEE_STATE = 2;
	//public int DASH_STATE = 3;
	//public int ORBS_STATE = 4;
	//public int FIRE_SWIRL_STATE = 5;
	//public int FRONTAL_ORBS_STATE = 6;
	
	public void ChangeState(int state)
	{
		switch (state){
			
			case 1:			
				
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
				//state = MOVE_STATE;
				break;
			case 5:
				StartCoroutine("CastFireSwirl");
				break;
			//case 6:
				//state = MOVE_STATE;
				//break;
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
		yield return new WaitForSeconds(1.0f);
		bossOrbs.BigOrbs(bossOrbs.numberOfProjectiles);
		yield return new WaitForSeconds(1.0f);
		meleeBoss.canBossMove = true;
		isCasting = false;
		ChangeState(MOVE_STATE);
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
		ChangeState(MOVE_STATE);
	}

	public IEnumerator CastFireSwirl()
	{
		Debug.Log("casting");
		isCasting = true;
		meleeBoss.canBossMove = false;
		bossFireSwirlScript.Swirl();
		yield return new WaitForSeconds(6.0f);
		meleeBoss.canBossMove = true;
		isCasting = false;
		ChangeState(MOVE_STATE);
	}
}


