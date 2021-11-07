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

    [SerializeField]
    Vector3 dir2;

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

    private void Destroy()
    {
        //Desativa as balas da pool do boss em vez de utilizar um destroy normal para n�o bugar na hora de chamar as variaveis
        gameObject.SetActive(false);
    }
	
	// ao desativar com o Destroy(), para o invoke
    private void OnDisable()
    {
        CancelInvoke();
    }
	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.tag == "Wall")
		{
			Invoke("Destroy", 0f);
		}
		if(col.gameObject.tag == "Player")
		{
			if(player.isDashing){
				bulletCollider.enabled = false;
			} else {
				playerAttributes.currentLife--;
				this.gameObject.SetActive(false);
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
