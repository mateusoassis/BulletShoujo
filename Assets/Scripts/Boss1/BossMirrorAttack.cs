using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMirrorAttack : MonoBehaviour
{
    public float rotationSpeed;

    public Transform bossTarget;

    public Transform bossFirePoint;

    public GameObject mirrorsPrefab;
    public float reflectionSpeed;
    void Start()
    {
        bossTarget = GameObject.Find("Boss").GetComponent<Transform>();
    }
    void Update()
    {
        transform.Rotate(0,rotationSpeed*Time.deltaTime,0);

        if (bossTarget.position.z > transform.position.z||bossTarget.position.x > transform.position.x||bossTarget.position.x < transform.position.x||bossTarget.position.z<transform.position.z)
        {
            Vector3 newPosition = new Vector3(bossTarget.position.x, transform.position.y, bossTarget.position.z);
            transform.position = newPosition;
        }
    }

    public void CastMirrors()
    {
        GameObject mirrors = Instantiate(mirrorsPrefab, bossFirePoint.position, Quaternion.identity) as GameObject;
        mirrorsPrefab.transform.SetParent (GameObject.FindGameObjectWithTag("Boss").transform, false);
    }
}
