using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
	public BoxCollider thisCollider;
	public Player playerScript;
	public Transform playerTransform;
	public Transform restartWaypoint;
	public GameManagerScript gameManagerScript;
	
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
