using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireSwirl : MonoBehaviour
{

    public float shootinterval;
    public int shootQuantity;

    public void Swirl() {
        StartCoroutine("FireSwirl");
    }


    public Transform firePoint;
    public float bulletForce = 20f;
    public GameObject bulletPrefab;


    void Start() {
        firePoint = GameObject.Find("BossFirePoint").GetComponent<Transform>();
    }

    public IEnumerator FireSwirl()
	{
		for(int i = 0; i <= shootQuantity; i++){
           
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            bulletRb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);

            yield return new WaitForSeconds(shootinterval);

        }
	}

}
