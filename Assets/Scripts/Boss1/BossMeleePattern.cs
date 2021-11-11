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
	public GameObject areaDamageObject;
	public MeshRenderer areaDamageRenderer;
	
	[Header("Controles de tamanho da área")]
	public bool horizontal;
	public bool axisCheck;
	public Vector3 horizontalScale;
	public int horizontalMultiplierX;
	public int horizontalMultiplierZ;
	
	public Vector3 verticalScale;
	public int verticalMultiplierX;
	public int verticalMultiplierZ;
	
	[Header("Controle de duração de buff")]
	public bool isBuffUp;
	public float buffDuration;
	public float extraRangeMultiplier;
	public float extraRange = 1f;
	
	[Header("Movespeed e localização do player")]
	public Vector3 targetPosition;
	public bool playerPositionSaved;
	private float bossMoveSpeed;
	public float moveSpeed;
	public bool isPlayerOnArea;
	public bool canBossMove;
	public bool isPlayerPosSaved;
	public bool bossLookingAtPlayer;
	public float timeMoving;
	
    void Awake()
    {
		//referenciar as coisas
        player = GameObject.Find("Player").GetComponent<Transform>();
		playerScript = GameObject.Find("Player").GetComponent<Player>();
		playerAttributesScript = GameObject.Find("Player").GetComponent<PlayerAttributes>();
		boss = GameObject.Find("Boss").GetComponent<Transform>();
		areaDamage = GameObject.Find("AreaDamage").GetComponent<Transform>();
		areaDamageObject = GameObject.Find("AreaDamage");
		areaDamageRenderer = GameObject.Find("AreaDamage").GetComponent<MeshRenderer>();
    }

	void DoMeleeMoves()
	{
		//checar se buff do boss tá ativo
		CheckIfBuffIsUp();
		
		//checar se ele pode se mover (movespeed diferente de 0)
		CheckIfBossCanMove();
		
		//mudar o tamanho da área da espadada
        if(horizontal)
		{
			areaDamage.transform.localScale = new Vector3((extraRangeMultiplier * horizontalMultiplierX * horizontalScale.x), horizontalScale.y, (horizontalMultiplierZ * horizontalScale.z));
			areaDamage.transform.position = new Vector3(transform.position.x, transform.position.y, (verticalScale.z/2));
		} else 
		{
			areaDamage.transform.localScale = new Vector3((verticalMultiplierX * verticalScale.x), verticalScale.y, (extraRangeMultiplier * verticalMultiplierZ * verticalScale.z));
			areaDamage.transform.position = new Vector3(transform.position.x, transform.position.y, (verticalScale.z/2));
		}
		
		//checar se o player está na área para continuar se aproximando dele antes de ataca-lo
		if(!isPlayerOnArea)
		{
			boss.position = Vector3.MoveTowards(boss.position, player.position, bossMoveSpeed * Time.deltaTime);
		} else
		{
			timeMoving += Time.deltaTime;
			boss.position = Vector3.MoveTowards(boss.position, player.position, bossMoveSpeed * Time.deltaTime);
			if(timeMoving >= 1.5f)
			{
				Invoke("WaitForAtk", 0.1f);
			}
		}
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
		} else
		{
			bossMoveSpeed = 0;
		}
	}
	
	//realizar mudança de range extra do boss
	public void CheckIfBuffIsUp()
	{
		if(isBuffUp)
		{
			extraRangeMultiplier = extraRange;
		} else
		{
			extraRangeMultiplier = 1f;
		}
	}
	
	//mudar de horizontal para vertical
	public void SwitchAxis()
	{
		if(horizontal)
		{
			horizontal = false;
		} else 
		{
			horizontal = true;
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
		SwitchAxis();
		bossLookingAtPlayer = true;
		timeMoving = 0f;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			isPlayerOnArea = true;
		}
	}
	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			isPlayerOnArea = false;
		}
	}
}
