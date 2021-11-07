using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBodySlam : MonoBehaviour
{
	public Transform player;
	public Transform boss;
	public Vector3 targetPosition;
	public float bodySlamSpeed;
	public bool playerPositionSaved;
	public BossState bossStateScript;
	public Rigidbody bossRigidbody;
	
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
		boss = GameObject.Find("Boss").GetComponent<Transform>();
		playerPositionSaved = false;
		bossStateScript = GameObject.Find("Boss").GetComponent<BossState>();
		bossRigidbody = GameObject.Find("Boss").GetComponent<Rigidbody>();
    }
	
	public void ResetPlayerPosition(){
		playerPositionSaved = false;
	}
	
	//BodySlam skill
	public void MoveToSlam()
	{
		if(!playerPositionSaved){
			//No caso de a posição do player ainda não ter sido identificada, o boss irá buscar a localização do player para preparar o ataque
			targetPosition.x = player.position.x;
			targetPosition.z = player.position.z;
			boss.LookAt(targetPosition, Vector3.up);
			//Após salvar, ele torna a variável verdadeira para o else dessa condição funcionar
			playerPositionSaved = true;
		} else {
			//Código para movimentar o boss em direção ao player e realizar o bodyslam
			boss.position = Vector3.MoveTowards(boss.position, targetPosition, bodySlamSpeed * Time.deltaTime);
			if(targetPosition == boss.position){
				bossStateScript.isBodySlamming = false;
			}
		}
	}
}
