using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUTORIALMirror : MonoBehaviour
{
    public float rotationSpeed;

    public Transform bossTarget;

    //public Transform bossFirePoint;

    //public GameObject mirrorsPrefab;
    public float reflectionSpeed;
	
	public GameObject mirror1;
	public GameObject mirror2;
	public GameObject mirror3;
	public GameObject mirror4;
	public GameObject mirrorInside1;
	public GameObject mirrorInside2;
	
	public float timeToReactivate;
	public float timeToReactivateTimer;
	
	public int mirrorsUp;
	public bool canUseMirror;
	
    void Start()
    {
		ActivateMirrors();
    }
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime,0);
		Vector3 newPosition = new Vector3(bossTarget.position.x, transform.position.y, bossTarget.position.z);
        //transform.position = newPosition;
		
		if (mirrorsUp < 4)
		{
			timeToReactivateTimer -= Time.deltaTime;
			if(timeToReactivateTimer <= 0)
			{
				ActivateMirrors();
				timeToReactivateTimer = timeToReactivate;
			}
		}
    }
	
	public void ActivateMirrors()
	{
		mirror1.gameObject.SetActive(true);
		mirror2.gameObject.SetActive(true);
		mirror3.gameObject.SetActive(true);
		mirror4.gameObject.SetActive(true);
		mirrorsUp = 4;		
	}
	
	public void Mirror1Break()
	{
		if(mirrorsUp >= 1)
		{
			mirrorsUp--;
		}	
		mirror1.gameObject.SetActive(false);
	}
	public void Mirror2Break()
	{
		if(mirrorsUp >= 1)
		{
			mirrorsUp--;
		}
		mirror2.gameObject.SetActive(false);
	}
	public void Mirror3Break()
	{
		if (mirrorsUp >= 1)
		{
			mirrorsUp--;
		}
		mirror3.gameObject.SetActive(false);
	}
	public void Mirror4Break()
	{
		if (mirrorsUp >= 1)
		{
			mirrorsUp--;
		}
		mirror4.gameObject.SetActive(false);
	}
}