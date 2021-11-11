using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELETABLE : MonoBehaviour
{
    public float rotspedd;
    public float shootinterval;
    public int shootQuantity;

    public Transform firePoint;
    public float bulletForce = 20f;
    public GameObject bulletPrefab;


    void Start() {
        firePoint = this.transform;
        StartCoroutine("FireSwirl");
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotspedd * Time.deltaTime, 0);
        
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
