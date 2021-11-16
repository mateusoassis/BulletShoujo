using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFullFire : MonoBehaviour
{
    [Header("Fire Pattern Shtuff")]
    public int numberOfProjectiles;
    public int numberOfTimesShot;
    public float projectileSpeed;
    public float timeBetweenShots;
    public GameObject ProjectilePrefab;
    

    [Header("Private Variables")]
    public Transform firePoint;
    private const float radius = 1f;



    public IEnumerator Shoot(){
        Debug.Log("Castou full fire");
        float angleStep = 360f/numberOfProjectiles;
        float angle = 0f;
        
        for(int j = 0; j < numberOfTimesShot; j++){
            for(int i = 0; i <= numberOfProjectiles - 1; i++)
            {
                if(j %2 == 0){
                    float projectileDirXPosition = firePoint.position.x + Mathf.Sin((angle * Mathf.PI/180)) * radius;
                    float projectileDirZPosition = firePoint.position.z + Mathf.Cos((angle * Mathf.PI/180)) * radius;

                    Vector3 projectileVector = new Vector3(projectileDirXPosition, 0, projectileDirZPosition);
                    Vector3 projectileMoveDirection = (projectileVector- firePoint.position).normalized * projectileSpeed;

                    GameObject tmpObj = Instantiate(ProjectilePrefab, firePoint.position, Quaternion.identity);
                    tmpObj.GetComponent<Rigidbody>().velocity = new Vector3(projectileMoveDirection.x,0,projectileMoveDirection.z);

                    angle += angleStep;
                } else{
                    float projectileDirXPosition = firePoint.position.x + Mathf.Sin(((angle+90f) * Mathf.PI/180)) * radius;
                    float projectileDirZPosition = firePoint.position.z + Mathf.Cos(((angle+90f) * Mathf.PI/180)) * radius;

                    Vector3 projectileVector = new Vector3(projectileDirXPosition, 0, projectileDirZPosition);
                    Vector3 projectileMoveDirection = (projectileVector- firePoint.position).normalized * projectileSpeed;

                    GameObject tmpObj = Instantiate(ProjectilePrefab, firePoint.position, Quaternion.identity);
                    tmpObj.GetComponent<Rigidbody>().velocity = new Vector3(projectileMoveDirection.x,0,projectileMoveDirection.z);

                    angle += angleStep;
                }
            }
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
}
