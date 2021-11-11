using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShotBigOrbs : MonoBehaviour
{
    [Header("Projectile Settings")]
    public int numberOfProjectiles;
    public float projectileSpeed;
    public GameObject ProjectilePrefab;

    [Header("Private Variables")]
    private Vector3 startPoint;
    private const float radius = 1f;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            startPoint = transform.position;
            BigOrbs(numberOfProjectiles);
        }
    }

    public void BigOrbs(int _numberOfProjectiles)
    {
        float angleStep = 360f/_numberOfProjectiles;
        float angle = 0f;

        for(int i = 0; i <= _numberOfProjectiles - 1; i++)
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
}
