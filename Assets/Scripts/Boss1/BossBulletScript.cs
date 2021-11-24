using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletScript : MonoBehaviour
{
    private Vector3 moveDirection;
    private float moveSpeed;
	private SphereCollider bulletCollider;
	private Player player;
	private PlayerAttributes playerAttributes;
	[SerializeField] private Tutorial tutorialScript;
	
	public GameManagerScript gameManager;

	public GameObject amayaExplosions;
    [SerializeField]
    Vector3 dir2;

	void Awake()
	{
		gameManager = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
		if(gameManager.tutorialStarted)
		{
			tutorialScript = GameObject.Find("dashColliderForHole").GetComponent<Tutorial>();
		}
	}

    void Start()
    {
        //velocidade do tiro
        moveSpeed = 12f;
		bulletCollider = GetComponent<SphereCollider>();
		player = GameObject.Find("Player").GetComponent<Player>();
		playerAttributes = GameObject.Find("Player").GetComponent<PlayerAttributes>();
		
        //destruir o tiro ap�s 8 segundos
		Invoke("Destroy", 8f);
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    public void SetMoveDirection(Vector3 dir)
    {

        moveDirection = dir;
        dir2 = moveDirection;
    }

	//Desativa as balas da pool do boss em vez de utilizar um destroy normal para n�o bugar na hora de chamar as variaveis
    private void Destroy()
    {       
        gameObject.SetActive(false);
    }
	
	// ao desativar com o Destroy(), para o invoke
    private void OnDisable()
    {
        //CancelInvoke();
		Destroy(this.gameObject);
    }
	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.tag == "Wall" || col.gameObject.tag == "ExplosionWalls")
		{
			Debug.Log("eu");
			//Invoke("Destroy", 0f);
			GameObject explosion = Instantiate(amayaExplosions, transform.position, Quaternion.identity) as GameObject;
			Destroy(this.gameObject);
		}
		if(col.gameObject.tag == "Player")
		{
			if(!gameManager.tutorialStarted)
			{
				if(player.isDashing)
				{
					bulletCollider.enabled = false;
				}else if(!player.isDashing)
				{
					if(player.isShielded && player.canBeDamaged){
						player.isShielded = false;
						player.shieldObject.SetActive(false);
						player.canBeDamaged = false;
						//this.gameObject.SetActive(false);
						GameObject explosion = Instantiate(amayaExplosions, transform.position, Quaternion.identity) as GameObject;
						Destroy(this.gameObject);
						player.StartCoroutine("DamagedReset");
					} else if(player.canBeDamaged)
					{
						player.canBeDamaged = false;
						playerAttributes.currentLife--;
						//this.gameObject.SetActive(false);
						GameObject explosion = Instantiate(amayaExplosions, transform.position, Quaternion.identity) as GameObject;
						Destroy(this.gameObject);
						player.StartCoroutine("DamagedReset");
					}else if(!player.canBeDamaged)
					{
						Destroy(this.gameObject);
					}
				}
			
			} else if(!player.isDashing)
			{
				tutorialScript.SendPlayerToStart();
				Destroy(this.gameObject);
			}
		}
	}	
	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			StartCoroutine("WaitTenthOfSecond");
			bulletCollider.enabled = true;
		}
	}
	
	public IEnumerator WaitTenthOfSecond()
	{
		yield return new WaitForSeconds(0.1f);
	}
}
