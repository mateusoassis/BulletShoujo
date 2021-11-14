using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBoss : MonoBehaviour
{
	[Header("Comportamento do boss")]
	private Vector3 targetPosition;
	[SerializeField] private bool playerPositionSaved;
	private float bossMoveSpeed; // fórmula
	public float moveSpeed;	// valor editável
	[SerializeField] private bool isBossMoving;	
	[SerializeField] public bool canBossMove;
	[SerializeField] private bool bossLookingAtPlayer;
	[SerializeField] private bool isBossDashing;
	[SerializeField] private bool isBossAttacking;
	public bool isPlayerOnArea;
	public float distanceFromPlayer;
	public float attackAntecipation;
	public float timeInsideAreaDamage;
	private float timeMoving;
	public float dashDuration;
	
	
	[Header("Range do boss")]
	public float verticalMultiplierX;
	public float verticalMultiplierZ;
	public float horizontalMultiplierX;
	public float horizontalMultiplierZ;
	
	[Header("Buff")]
	public bool isBuffUp;
	public float extraRange = 1.5f;
	public float extraRangeMultiplier = 1f;
	
	//bom dia
	[Header("Boss")]
	private Rigidbody bossRb;
	private Transform bossTransform;
	private Transform areaDamage;
	private Transform areaDamageParent;
	private GameObject areaDamageObject;
	private MeshRenderer areaDamageRenderer;
	[SerializeField] private Vector3 verticalAttackScale;
	[SerializeField] private Vector3 horizontalAttackScale;
	private BossDamage bossDamage;
	private BossState bossState;
	
	[Header("Player")]
	private Rigidbody playerRb;
	private Transform playerTransform;
	private Player playerScript;
	private PlayerAttributes playerAttributesScript;
	
	// 1 = mover
	// 2 = ataque melee
	// 3 = dash
	
	
	void Awake()
	{
		// referenciar as coisa
		bossRb = GameObject.Find("Boss").GetComponent<Rigidbody>();
		playerTransform = GameObject.Find("Player").GetComponent<Transform>();
		playerScript = GameObject.Find("Player").GetComponent<Player>();
		playerAttributesScript = GameObject.Find("Player").GetComponent<PlayerAttributes>();
		bossTransform = GameObject.Find("Boss").GetComponent<Transform>();
		areaDamage = GameObject.Find("AreaDamage").GetComponent<Transform>();
		areaDamageParent = GameObject.Find("ScalingAreaDamage").GetComponent<Transform>();
		areaDamageObject = GameObject.Find("AreaDamage");
		areaDamageRenderer = GameObject.Find("AreaDamage").GetComponent<MeshRenderer>();
		bossDamage = GameObject.Find("Boss").GetComponent<BossDamage>();
		bossState = GameObject.Find("Boss").GetComponent<BossState>();
	}
	
    void Start()
    {
        canBossMove = true;
		bossLookingAtPlayer = true;
		areaDamageRenderer.enabled = false;
    }
	
	public void LookAndMove()
	{
		// checar se ele pode se mover (movespeed diferente de 0)
		CheckIfBossCanMove();
		
		// duh
		LookAtPlayer();
		
		// ciclar todas as skills
		CyclingSkills();
	}
	
	
	public void CheckIfBossCanMove()
	{
		if(canBossMove && !isBossDashing && bossState.isCasting) // && !isDashing
		{
			if(distanceFromPlayer > 10)
			{
				bossMoveSpeed = 2 * moveSpeed;
			} else
			{
				bossMoveSpeed = moveSpeed;
			}
			
		} else
		{
			bossMoveSpeed = 0;
		}
	}
	public void LookAtPlayer()
	{
		// cálculo de distância até o jogador
		distanceFromPlayer = Vector3.Distance(bossTransform.position, playerTransform.position);
		
		//if !isDashing
		if(!isBossDashing && !bossState.isCasting)
		{
			if(!isBossAttacking)
			{
				targetPosition.x = playerTransform.position.x;
				targetPosition.z = playerTransform.position.z;
			}
			bossTransform.LookAt(targetPosition, Vector3.up);
			//bossTransform.rotation = Quaternion.LookRotation(playerTransform.position - bossTransform.position, transform.up);
		}	
			
	}
	
	public void CyclingSkills()
	{		
		// cálculo de distância até player
		distanceFromPlayer = Vector3.Distance(bossTransform.position, playerTransform.position);
		
		if(distanceFromPlayer > 10f && !isBossDashing && !bossState.isCasting)
		// checar se o player está no ALCANCE para continuar se aproximando dele antes de ataca-lo
		{
			bossState.ChangeState(bossState.DASH_STATE);
		} else if(canBossMove)
		{
	/*{
			// dashcd true pra resta tempo até re-uso, false pra já pode ser usado
			if(!isDashOnCooldown && !isBossDashing)
				
			{
				DashToPlayer();	
			} else if(!isBossDashing && isDashOnCooldown)
			{
				boss.position = Vector3.MoveTowards(boss.position, targetPosition, bossMoveSpeed * Time.deltaTime);
			}
			//boss.position = Vector3.MoveTowards(boss.position, targetPosition, 2 * bossMoveSpeed * Time.deltaTime);
		} else if(!isBossDashing && isDashOnCooldown)
		{*/
			bossTransform.position = Vector3.MoveTowards(bossTransform.position, targetPosition, bossMoveSpeed * Time.deltaTime);
			
			timeMoving += Time.deltaTime;
			if(timeMoving >= timeInsideAreaDamage) // EM SEGUNDOS
			{
				bossState.ChangeState(bossState.MELEE_STATE);
				timeMoving = 0f;
			}
		}
	}
	
	public IEnumerator MeleeAttack()
	{
		canBossMove = false;
		isBossAttacking = true;
		bossLookingAtPlayer = false;
		areaDamageRenderer.enabled = true;
		bossRb.velocity = Vector3.zero;
		bossRb.angularVelocity = Vector3.zero;
				
		yield return new WaitForSeconds(attackAntecipation);
		//bossMeleeCollider.enabled = true; //após um tempinho
		
		if(isPlayerOnArea) //&& !playerScript.isDashing && !isBuffUp
		{
			playerAttributesScript.currentLife--;		
		} else if(isPlayerOnArea && isBuffUp)
		{
			playerAttributesScript.currentLife -= 2;
		}
		bossRb.velocity = Vector3.zero;
		bossRb.angularVelocity = Vector3.zero;
		
		areaDamageRenderer.enabled = false;
		SwitchMeleeAttack();
		canBossMove = true;
		timeMoving = 0f;
		//bossMeleeCollider.enabled = false;
		//timeAwayFromPlayer = 0f;
		
		bossLookingAtPlayer = true;
		isBossAttacking = false;
		bossState.ChangeState(bossState.IDLE_STATE);
	}
	
	public void SwitchMeleeAttack()
	{
		if(isBuffUp)
		{
			extraRangeMultiplier = extraRange;
		} else {
			extraRangeMultiplier = 1f;
		}
		// 0 = horizontal
		// 1 = vertical
		
		//mudar o tamanho da área da espadada
		int u = Random.Range(0,2);
		if(u == 0)
		{
			areaDamageParent.transform.localScale = new Vector3((extraRangeMultiplier * horizontalMultiplierX) * horizontalAttackScale.x, 1f * horizontalAttackScale.y, ((extraRangeMultiplier) * horizontalMultiplierZ) * horizontalAttackScale.z);
		} else if(u == 1)
		{
			areaDamageParent.transform.localScale = new Vector3(((extraRangeMultiplier) * verticalMultiplierX) * verticalAttackScale.y, 1f * verticalAttackScale.y, (extraRangeMultiplier * verticalMultiplierZ) * verticalAttackScale.z);
        
		}
	
	}
	
	public IEnumerator Dash(Transform transform, Vector3 position)
	//StartCoroutine("Dash", boss, player.position, 2f)
	{
		isBossDashing = true;
		bossLookingAtPlayer = false;
		canBossMove = false;
		bossTransform.LookAt(targetPosition, Vector3.up);
		//bossDashCollider.enabled = true;
		Vector3 playerPos = position;
		float duration = dashDuration;
		var counter = 0f; // counter
		while(counter < duration) // duration
		{
			counter += Time.deltaTime;
			float time = (Vector3.Distance(bossTransform.position, playerPos)) / ((duration-counter));
			//if(isDashOnCollider)
			//{
			//	isBossDashing = false;
			//	bossDashCollider.enabled = false;
			//	bossLookingAtPlayer = true;
			//	Debug.Log("dash termina");
			//	yield break;
			//}
			//else{
			//t += Time.deltaTime / duration; // EM SEGUNDOS PRA CHEGAR NO ALVO
			//bossTransform.position = Vector3.Lerp(bossTransform.position, position, t);
			bossTransform.position = Vector3.MoveTowards(bossTransform.position, playerPos, time * Time.deltaTime);
			yield return null;
			//} FRONTAL_ORBS_STATE
		}
		isBossDashing = false;
		//bossDashCollider.enabled = false;
		bossLookingAtPlayer = true;
		canBossMove = true;
		//Debug.Log("dash termina");
		bossState.ChangeState(bossState.MELEE_STATE);
	}
}
	

