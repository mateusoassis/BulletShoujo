using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirePattern2 : MonoBehaviour
{
    [SerializeField]
    private int bulletsAmount = 10;
    public float bulDirX;
    public float bulDirZ;
    public float bulDirY = 0;

    public GameObject firePoint;

	public Transform Player;
	
    [SerializeField]
    private float startAngle = 90f, endAngle = 270f;

    private Vector3 bulletMoveDirection;

    void Start()
    {
        
    }

    private void Fire()
    {
        float angleStep = (endAngle - startAngle) / bulletsAmount;
        float angle = startAngle;
		
        for(int i = 0; i < bulletsAmount +1; i++)
        {
            bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            bulDirZ = transform.position.z + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, bulDirZ );
            Vector3 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = BossBulletPool.bossBulletPoolInstanse.GetBullet();
            bul.transform.position = firePoint.transform.position;
            bul.transform.rotation = firePoint.transform.rotation;
            bul.SetActive(true);
            bul.GetComponent<BossBulletScript>().SetMoveDirection(bulDir);

            angle += angleStep;
            FindObjectOfType<AudioManager>().PlayOneShot("AmayaShot");
        }
    }
}
