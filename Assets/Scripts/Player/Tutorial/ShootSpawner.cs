using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSpawner : MonoBehaviour
{
	public float shootingRate;
	public float shootingRateTimer;
	public Vector3 shootingDirection;
	public GameObject bossBulletPrefab;
	public float bulletForce;
	
	void Update()
	{
		if(shootingRateTimer <= 0)
		{
			Shoot();
			shootingRateTimer = shootingRate;
		} else
		{
			shootingRateTimer -= Time.deltaTime;
		}
			
	}
	
    void Shoot()
    {
        //Instanciamento de prefab do tiro de personagem.
        GameObject bullet = Instantiate(bossBulletPrefab, transform.position, transform.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        //Adicao de forca no tiro para impulsionar o prefab.
        bulletRb.AddForce(shootingDirection * bulletForce, ForceMode.Impulse);
    }
}
