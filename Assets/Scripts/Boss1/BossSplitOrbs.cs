using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSplitOrbs : MonoBehaviour
{
    
    [Header("Projectile Settings")]
    public int numberOfSmallOrbs;
    public float projectileSpeed;
    public float destructionTimer;
    public GameObject ProjectilePrefab;

    [Header("Private Variables")]
    private Vector3 startPoint;
    private const float radius = 1f;

    void Update()
    {
        StartCoroutine("TimeToDestroy");
    }
    
    public void SmallOrbs(int _numberOfSmallOrbs)
    {
        float angleStep = 360f/_numberOfSmallOrbs;
        float angle = 0f;

        for(int i = 0; i <= _numberOfSmallOrbs - 1; i++)
        {
            float projectileDirXPosition = startPoint.x + Mathf.Sin((angle * Mathf.PI/180)) * radius;
            float projectileDirZPosition = startPoint.z + Mathf.Cos((angle * Mathf.PI/180)) * radius;

            Vector3 projectileVector = new Vector3(projectileDirXPosition, 0, projectileDirZPosition);
            Vector3 projectileMoveDirection = (projectileVector- startPoint).normalized * projectileSpeed;

            GameObject tmpObj = Instantiate(ProjectilePrefab, startPoint, Quaternion.identity);
            tmpObj.GetComponent<Rigidbody>().velocity = new Vector3(projectileMoveDirection.x,0,projectileMoveDirection.z);

            angle += angleStep;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Wall")
        {
            BulletDestroyer();
        }
    }

    void BulletDestroyer()
    {
        Destroy(this.gameObject);
        startPoint = transform.position;
        SmallOrbs(numberOfSmallOrbs);
    }

    public IEnumerator TimeToDestroy()
    {
        yield return new WaitForSeconds(destructionTimer);
        BulletDestroyer();
    }
}
