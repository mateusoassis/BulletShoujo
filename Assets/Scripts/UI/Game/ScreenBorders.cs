using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenBorders : MonoBehaviour
{
	public Vector3 maxScaleVector;
	public Vector3 minScaleVector;
	
	public float scaleSpeed;
	
	public bool scaleUp;
	
	public GameObject screenBorders;
	
	public PlayerAttributes playerAtt;
	
	public BossDamage bossDamage;
	
	public GameManagerScript gameManagerScript;
	
	void Start()
	{
		if(gameManagerScript.screenBordersOff)
		{
			this.gameObject.SetActive(false);
		} else
		{
			this.gameObject.SetActive(true);
		}
	}
	
	void Update()
    {
        if(playerAtt.currentLife == 1)
		{
			if(scaleUp == false)
			{
				transform.localScale = Vector3.MoveTowards(transform.localScale, minScaleVector, scaleSpeed * Time.deltaTime);
			} else
			{
				transform.localScale = Vector3.MoveTowards(transform.localScale, maxScaleVector, scaleSpeed * Time.deltaTime);
			}
		} else
		{
			transform.localScale = maxScaleVector;
		}
		
		if(transform.localScale.x >= maxScaleVector.x && transform.localScale.y >= maxScaleVector.y)
		{
			scaleUp = false;
		} else if(transform.localScale.x <= minScaleVector.x && transform.localScale.y <= minScaleVector.y)
		{
			scaleUp = true;
		}
		
		if(playerAtt.currentLife == 0 || bossDamage.bossHPCurrent == 0)
		{
			this.gameObject.SetActive(false);
		}
    }
}
