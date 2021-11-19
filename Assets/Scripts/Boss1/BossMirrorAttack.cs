using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMirrorAttack : MonoBehaviour
{
    public float rotationSpeed;

    public Transform bossTarget;

    //public Transform bossFirePoint;

    //public GameObject mirrorsPrefab;
    public float reflectionSpeed;
	
	public GameObject mirror1;
	public GameObject mirror2;
	public GameObject mirrorInside1;
	public GameObject mirrorInside2;
	
	public int mirrorsUp;
	public bool canUseMirror;
	
    void Start()
    {
        bossTarget = GameObject.Find("Boss").GetComponent<Transform>();
		mirrorsUp = 0;
    }
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime,0);
		Vector3 newPosition = new Vector3(bossTarget.position.x, transform.position.y, bossTarget.position.z);
        transform.position = newPosition;
		
		if(mirrorsUp == 0)
		{
			canUseMirror = true;
		} else {
			canUseMirror = false;
		}

        /*
		if (bossTarget.position.z > transform.position.z||bossTarget.position.x > transform.position.x||bossTarget.position.x < transform.position.x||bossTarget.position.z<transform.position.z)
        {
            Vector3 newPosition = new Vector3(bossTarget.position.x, transform.position.y, bossTarget.position.z);
            transform.position = newPosition;
        }
		*/
    }
	
	public void ActivateMirrors()
	{
		mirror1.gameObject.SetActive(true);
		//mirror1.transform.position = new Vector3(1f, 0.2f, 0f);
		mirror2.gameObject.SetActive(true);
		//mirror2.transform.position = new Vector3(-1f, 0.2f, 0f);
		mirrorsUp = 2;
		FindObjectOfType<AudioManager>().PlayOneShot("AmayaMirrorShield");
	}
	
	public void Mirror1Break()
	{
		//mirror1.transform.position = new Vector3(1f, 0.2f, 0f);
		if(mirrorsUp >= 1)
		{
			mirrorsUp--;
		}	
		mirror1.gameObject.SetActive(false);
	}
	public void Mirror2Break()
	{
		//mirror2.transform.position = new Vector3(-1f, 0.2f, 0f);
		if(mirrorsUp >= 1)
		{
			mirrorsUp--;
		}
		mirror2.gameObject.SetActive(false);
	}

    /*
	public void CastMirrors()
    {
        GameObject mirrors = Instantiate(mirrorsPrefab, bossFirePoint.position, Quaternion.identity) as GameObject;
        mirrors.transform.SetParent (GameObject.FindGameObjectWithTag("Boss").transform, false);
		mirrors.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
    }
	*/
}
