using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
	public GameObject cursorLayerMap;
	
	public BoxCollider thisCollider;
	public Player playerScript;
	public Transform playerTransform;
	public Transform restartWaypoint;
	public GameManagerScript gameManagerScript;
	public PlayerAttributes playerAtt;
	
	public int manaRegenPerSecond;
	public float manaRegenDelay;
	public float manaRegenDelayTimer;
	
	public bool resetPlayerPos;
	
	void Awake()
	{
		gameManagerScript = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
	}
	
    void Start()
    {
        gameManagerScript.tutorialStarted = true;
		gameManagerScript.gameStarted = false;		
    }

    void Update()
    {
        if(playerScript.isDashing == true)
		{
			thisCollider.isTrigger = true;
			resetPlayerPos = false;
		} else
		{
			thisCollider.isTrigger = false;
			resetPlayerPos = true;
		}		
		RegenMana();		
    }
	
	public void RegenMana()
	{
		if(playerAtt.currentMana < playerAtt.maxMana)
		{
			
			if(manaRegenDelayTimer <= 0)
			{
				playerAtt.currentMana += manaRegenPerSecond;
				manaRegenDelayTimer = manaRegenDelay;
			} else
			{
				manaRegenDelayTimer -= Time.deltaTime;
			}
		} else
		{
			playerAtt.currentMana = playerAtt.maxMana;
		}
		
	}
	
	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Player" && resetPlayerPos)
		{
			SendPlayerToStart();
		}
	}
	
	public void SendPlayerToStart()
	{
		playerTransform.position = restartWaypoint.position;
	}
}
