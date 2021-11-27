using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
	public Animator amayaAnimator;
	
	[Header("Variáveis da Primeira Fase")]
	public float a1;
	
	[Header("Variáveis da Segunda Fase")]
	public float a2;
	
	[Header("States")]
	public int IDLE_STATE = 0;
	public int MOVE_STATE = 1;
	public int MELEE_STATE = 2;
	public int DASH_STATE = 3;
	public int ORBS_STATE = 4;
	public int FIRE_SWIRL_STATE = 5;
	public int FULL_FIRE_STATE = 6;
	public int MIRROR_CAST = 7;
	public int PHASE_CHANGE = 8;

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
	public int idleDuration;
	public bool isBossIdle;
	public bool secondPhase;
	public GameObject secondPhaseShader;
	
	[Header("Boss")]
	private Rigidbody bossRb;
	private Transform bossTransform;
	private Transform areaDamage;
	private Transform areaDamageParent;
	
	[SerializeField] private GameObject mirrorsObject;
	
	private Transform bossFirePoint;
	private GameObject areaDamageObject;
	private MeshRenderer areaDamageRenderer;
	private Vector3 verticalAttackScale;
	private Vector3 horizontalAttackScale;
	private BossDamage bossDamage;
	private BossFirePattern bossFrontalShotsScript;
	private BossShotBigOrbs bossOrbsScript;
	private BossFireSwirl bossFireSwirlScript;
	private BossFullFire fullFireScript;
	private BossMirrorAttack mirrorCastScript;

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
		bossOrbsScript = GameObject.Find("BossManager").GetComponent<BossShotBigOrbs>();
		bossFrontalShotsScript = GameObject.Find("BossManager").GetComponent<BossFirePattern>();
		bossFirePoint = GameObject.Find("BossFirePoint").GetComponent<Transform>();
		bossFireSwirlScript = GameObject.Find("BossManager").GetComponent<BossFireSwirl>();
		fullFireScript = GameObject.Find("BossManager").GetComponent<BossFullFire>();
		mirrorCastScript = GameObject.Find("MirrorPoint").GetComponent<BossMirrorAttack>();
		
		isCasting = false;
		
		//salvando a template do dash
		//StartCoroutine(bossMeleePattern.Dash(this.transform, player.position, 2f));
		StartCoroutine("StartBoss");
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
		if(secondPhase)
		{
			secondPhaseShader.SetActive(true);
		}
		if(BossDamage.bossIsDead)
		{
			this.GetComponent<BossState>().enabled = false;
			mirrorsObject.SetActive(false);
			/*
			StopCoroutine("StartIdleState");
			StopCoroutine("CastBigOrbs");
			StopCoroutine("CastFirePattern");
			StopCoroutine("CastFireSwirl");
			StopCoroutine("CastFullFire");
			StopCoroutine("CastMirrors");
			*/
			ZaWarudo();
		}
	}
	
	public void ZaWarudo() // wtf
	{
		StopAllCoroutines();
		bossFrontalShotsScript.StopAllCoroutines();
		bossOrbsScript.StopAllCoroutines();
		bossFireSwirlScript.StopAllCoroutines();
		fullFireScript.StopAllCoroutines();
		mirrorCastScript.StopAllCoroutines();
	}
	
	
	//tempo pro boss iniciar o combate
	public IEnumerator StartBoss()
	{
		yield return new WaitForSeconds(3f);
		ChangeState(ORBS_STATE);
	}
	
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
	//public int MIRROR_CAST = 7;
	//public int PHASE_CHANGE = 8;
	
	public void ChangeState(int state)
	{
		switch (state){
			
			case 0:
				StartCoroutine("StartIdleState");
				break;
			case 1:			
				
				break;
			case 2:
				meleeBoss.StartCoroutine("MeleeAttack");			
				break;
			case 3:
				StartCoroutine(meleeBoss.Dash(bossTransform, playerTransform.position));
				break;
			case 4:
			    StartCoroutine("CastBigOrbs");
				break;
			case 5:
				StartCoroutine("CastFireSwirl");
				break;
			case 6:
				StartCoroutine("CastFullFire");
				break;
			case 7:
				StartCoroutine("CastMirrors");
				break;
			/*case 8: //PHASE_CHANGE = 8
				StartCoroutine("ChangePhase");
				break;*/
		}
		currentState = state;
	}

	
	public IEnumerator StartIdleState()
	{
		Debug.Log("idle");
		isBossIdle = true;
		amayaAnimator.SetBool("isIdle", true);		
		yield return new WaitForSeconds(idleDuration);
		
		int randomNextAttack = Random.Range(0,5);
		if(randomNextAttack == 0)
		{
			isBossIdle = false;
			amayaAnimator.SetBool("isIdle", false);
			ChangeState(ORBS_STATE);
			Debug.Log("troca pra orb");			// state 4
		}else if(randomNextAttack == 1)
		{
			isBossIdle = false;
			amayaAnimator.SetBool("isIdle", false);
			ChangeState(FIRE_SWIRL_STATE);
			Debug.Log("troca pra fire swirl");// state 5
		}else if(randomNextAttack == 2)
		{
			isBossIdle = false;
			amayaAnimator.SetBool("isIdle", false);
			ChangeState(MOVE_STATE);
			Debug.Log("troca pra move");// state 1		
		}else if(randomNextAttack == 3){
			isBossIdle = false;
			amayaAnimator.SetBool("isIdle", false);
			ChangeState(FULL_FIRE_STATE);
			Debug.Log("troca pra full fire");// state 6
		}else if(randomNextAttack == 4 && mirrorCastScript.canUseMirror)
		{
			isBossIdle = false;
			amayaAnimator.SetBool("isIdle", false);
			ChangeState(MIRROR_CAST);
			Debug.Log("troca pra espelho");// state 7
		} else if(randomNextAttack == 4 && !mirrorCastScript.canUseMirror)
		{
			isBossIdle = false;
			amayaAnimator.SetBool("isIdle", false);
			ChangeState(MOVE_STATE);
			Debug.Log("troca pra move state");			// state 1
		}
	}

	public IEnumerator CastBigOrbs()
	{	
		Debug.Log("cast big orbs");
		isCasting = true;
		amayaAnimator.SetBool("isShooting", true);
		meleeBoss.canBossMove = false;
		yield return new WaitForSeconds(1.0f);
		bossOrbsScript.BigOrbs(bossOrbsScript.numberOfProjectiles);
		yield return new WaitForSeconds(1.0f);
		meleeBoss.canBossMove = true;
		isCasting = false;
		amayaAnimator.SetBool("isShooting", false);
		ChangeState(MOVE_STATE);	
	}

	public IEnumerator CastFirePattern()
	{
		Debug.Log("cast fire pattern");
		isCasting = true;
		amayaAnimator.SetBool("isShooting", true);
		meleeBoss.canBossMove = false;
		yield return new WaitForSeconds(2.0f);
		bossOrbsScript.BigOrbs(8);
		yield return new WaitForSeconds(2.0f);
		meleeBoss.canBossMove = true;
		isCasting = false;
		amayaAnimator.SetBool("isShooting", false);
		ChangeState(MOVE_STATE);
	}

	public IEnumerator CastFireSwirl()
	{
		Debug.Log("fire swirl");
		isCasting = true;
		amayaAnimator.SetBool("isShooting", true);
		meleeBoss.canBossMove = false;
		bossFireSwirlScript.Swirl();
		yield return new WaitForSeconds(6.0f);
		meleeBoss.canBossMove = true;
		isCasting = false;
		amayaAnimator.SetBool("isShooting", false);
		ChangeState(MOVE_STATE);
	}
	public IEnumerator CastFullFire()
	{
		Debug.Log("full fire");
		isCasting = true;
		amayaAnimator.SetBool("isShooting", true);
		meleeBoss.canBossMove = false;
		fullFireScript.StartCoroutine("Shoot");
		yield return new WaitForSeconds(6.0f);
		meleeBoss.canBossMove = true;
		isCasting = false;
		amayaAnimator.SetBool("isShooting", false);
		ChangeState(MOVE_STATE);
	}

	public IEnumerator CastMirrors()
	{
		Debug.Log("espelhos");
		isCasting = true;
		amayaAnimator.SetBool("isMirror", true);
		meleeBoss.canBossMove = false;
		FindObjectOfType<AudioManager>().PlayOneShot("AmayaMirrorShield");
		yield return new WaitForSeconds(0.8f);
		mirrorCastScript.ActivateMirrors();
		yield return new WaitForSeconds(6.0f);
		meleeBoss.canBossMove = true;
		isCasting = false;
		amayaAnimator.SetBool("isMirror", false);
		ChangeState(MOVE_STATE);
	}
	/*public IEnumerator ChangePhase()
	{
		Debug.Log("fase 8");
		transform.position = bossDamage.secondPhaseTransform.position;
		//meleeBoss.isBuffUp = true;
		// aplicar shaders aqui já já
		yield return new WaitForSeconds(5f);
		Debug.Log("AE PUTA");
		bossDamage.canTakeDamage = true;
		//ChangeState(DASH_STATE);
	}*/
}


