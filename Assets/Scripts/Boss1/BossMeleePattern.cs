using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleePattern : MonoBehaviour
{
	[Header("Objetos mutáveis")]
	public Transform player;
	public Player playerScript;
	public PlayerAttributes playerAttributesScript;
	public Transform boss;
	public Transform areaDamage;
	public Transform areaDamageParent;
	public GameObject areaDamageObject;
	public MeshRenderer areaDamageRenderer;
	
	[Header("Controles de tamanho da área")]
	public Vector3 horizontalScale;
	public float horizontalMultiplierX;
	public float horizontalMultiplierZ;
	
	public Vector3 verticalScale;
	public float verticalMultiplierX;
	public float verticalMultiplierZ;
	
	[Header("Controle de duração de buff")]
	public bool isBuffUp;
	public float buffDuration;
	public float extraRangeMultiplier;
	public float extraRange = 1f;
	public float buffTimer;
	
	[Header("Movespeed e localização do player")]
	public Vector3 targetPosition;
	public bool playerPositionSaved;
	[SerializeField]private float bossMoveSpeed;
	public float moveSpeed;
	public bool isPlayerOnArea;
	public bool canBossMove;
	public bool isPlayerPosSaved;
	public bool bossLookingAtPlayer;
	public float timeMoving;
	public float timeAwayFromPlayer;
	
	[Header("Stats do boss")]
	public BossDamage bossDamage;
	
	[Header("Booleano de animação")]
	public bool isBossMoving;
	
    void Awake()
    {
		//referenciar as coisas
        player = GameObject.Find("Player").GetComponent<Transform>();
		playerScript = GameObject.Find("Player").GetComponent<Player>();
		playerAttributesScript = GameObject.Find("Player").GetComponent<PlayerAttributes>();
		boss = GameObject.Find("Boss").GetComponent<Transform>();
		areaDamage = GameObject.Find("AreaDamage").GetComponent<Transform>();
		areaDamageParent = GameObject.Find("ScalingAreaDamage").GetComponent<Transform>();
		areaDamageObject = GameObject.Find("AreaDamage");
		areaDamageRenderer = GameObject.Find("AreaDamage").GetComponent<MeshRenderer>();
		bossDamage = GameObject.Find("Boss").GetComponent<BossDamage>();
    }

	void Start()
	{
		bossLookingAtPlayer = true;
		canBossMove = true;
		UpdateAxis();
	}
	
	public void DoMeleeMoves()
	{
		//checar se buff do boss tá ativo
		CheckIfBuffIsUp();
		
		//checar se ele pode se mover (movespeed diferente de 0)
		CheckIfBossCanMove();
		
		if(bossLookingAtPlayer)
		{
			targetPosition.x = player.position.x;
			targetPosition.z = player.position.z;
		}
		boss.LookAt(targetPosition, Vector3.up);
		
		//checar se o player está na área para continuar se aproximando dele antes de ataca-lo
		if(!isPlayerOnArea)
		{
			boss.position = Vector3.MoveTowards(boss.position, targetPosition, bossMoveSpeed * Time.deltaTime);
		} else
		{
			boss.position = Vector3.MoveTowards(boss.position, targetPosition, bossMoveSpeed * Time.deltaTime);
			timeMoving += Time.deltaTime;
			if(timeMoving >= 1.5f)
			{
				StartCoroutine("WaitForAtk");
				timeMoving = 0f;
			}
		}
		timeAwayFromPlayer += Time.deltaTime;
	}
	
	public void UpdateAxis()
	{
		
		// 0 = horizontal
		// 1 = vertical
		// 2 = buff + random.range entre os dois de novo
		
		//mudar o tamanho da área da espadada
		if(!isBuffUp)
		{
			int u = Random.Range(0,3);
			if(u == 0)
			{
				areaDamageParent.transform.localScale = new Vector3((extraRangeMultiplier * horizontalMultiplierX), 1f, ((extraRangeMultiplier/1.5f) * horizontalMultiplierZ));
			} else if(u == 1)
			{
				areaDamageParent.transform.localScale = new Vector3(((extraRangeMultiplier/1.5f) * verticalMultiplierX), 1f, (extraRangeMultiplier * verticalMultiplierZ));
			} else if(u == 2)
			{
				TurnOnBuff();
				
				int v = Random.Range(0,2);
				if(v == 0)
				{
					areaDamageParent.transform.localScale = new Vector3((extraRangeMultiplier * horizontalMultiplierX), 1f, ((extraRangeMultiplier/1.5f) * horizontalMultiplierZ));
				} else if(v == 1)
				{
					areaDamageParent.transform.localScale = new Vector3(((extraRangeMultiplier/1.5f) * verticalMultiplierX), 1f, (extraRangeMultiplier * verticalMultiplierZ));
				}
			}
		} else if(isBuffUp)
		{
			int w = Random.Range(0,2);
				if(w == 0)
				{
					areaDamageParent.transform.localScale = new Vector3((extraRangeMultiplier * horizontalMultiplierX), 1f, ((extraRangeMultiplier/1.5f) * horizontalMultiplierZ));
				} else if(w == 1)
				{
					areaDamageParent.transform.localScale = new Vector3(((extraRangeMultiplier/1.5f) * verticalMultiplierX), 1f, (extraRangeMultiplier * verticalMultiplierZ));
				}
		}
        
	}
	
	//liga buff de range e dano
	public void TurnOnBuff()
	{
		isBuffUp = true;
		buffTimer = buffDuration;
	}
	
	//salvar posição do jogador e setar booleano como salvo
	public void SavePlayerPosition()
	{
		targetPosition.x = player.position.x;
		targetPosition.z = player.position.z;
		isPlayerPosSaved = true;
	}
	
	//mudar a velocidade de movimentação do boss
	public void CheckIfBossCanMove()
	{
		if(canBossMove)
		{
			bossMoveSpeed = moveSpeed;
			isBossMoving = true;
		} else
		{
			bossMoveSpeed = 0;
			isBossMoving = false;
		}
	}
	
	//realizar mudança de range extra do boss
	public void CheckIfBuffIsUp()
	{
		if(isBuffUp)
		{
			extraRangeMultiplier = extraRange;
			buffTimer -= Time.deltaTime;
			if(buffTimer <= 0f)
			{
				isBuffUp = false;
			}
		} else
		{
			extraRangeMultiplier = 1f;
		}
	}	
	
	//enumerator referente ao ATK que faz o seguinte:
	//- para de OLHAR para o player
	//- ativa o renderer da área de dano
	//- seta movespeed do boss em 0
	//- salva posição do jogador
	//- espera 1 segundo para realizar redução do HP
	//- somente se o player está dentro da área
	//- se buffado, reduz o dobro de vida
	//- desativa o renderer após o golpe
	//- permite o boss se mover
	//- troca o axis do golpe
	//- volta a OLHAR para o player
	//- reseta o tempo se movendo
	public IEnumerator WaitForAtk()
	{
		bossLookingAtPlayer = false;
		areaDamageRenderer.enabled = true;
		canBossMove = false;
		SavePlayerPosition();	
		
		yield return new WaitForSeconds(1f);
		
		if(isPlayerOnArea && !playerScript.isDashing && !isBuffUp)
		{
			playerAttributesScript.currentLife--;		
		} else if(isPlayerOnArea && !playerScript.isDashing && isBuffUp)
		{
			playerAttributesScript.currentLife -= 2;
		}
		
		areaDamageRenderer.enabled = false;
		canBossMove = true;
		isPlayerPosSaved = false;
		UpdateAxis();
		bossLookingAtPlayer = true;
		timeMoving = 0f;
		//timeAwayFromPlayer = 0f;
	}
}
