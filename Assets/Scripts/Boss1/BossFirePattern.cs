using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirePattern : MonoBehaviour
{
    [SerializeField]
    //N�mero de balas, dire��es na qual as balas ser�o disparadas.
    private int bulletsAmount = 10;
    public float bulDirX;
    public float bulDirZ;
    public float bulDirY = 0;

    public GameObject firePoint;

	public Transform Player;
	
    [SerializeField]
    private float startAngle = 90f, endAngle = 270f;

    private Vector3 bulletMoveDirection;

    public void Fire()
    {
        //define a angula��o da abertura de onde os tiros ser�o disparados e o espa�amento entre eles.
        float angleStep = (endAngle - startAngle) / bulletsAmount;
        float angle = startAngle;
		
        for(int i = 0; i < bulletsAmount +1; i++)
        {
            //ALTAS MATEM�TICAS!!!!!!!!!!!!
            bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            bulDirZ = transform.position.z + Mathf.Cos((angle * Mathf.PI) / 180f);

            //definindo a dire��o para levar ao script das balas.
            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, bulDirZ );
            Vector3 bulDir = (bulMoveVector - transform.position).normalized;

            
            GameObject bul = BossBulletPool.bossBulletPoolInstanse.GetBullet();

            FindObjectOfType<AudioManager>().PlayOneShot("AmayaShot");
            
            //Utilizando o transform do firepoint para indicar de onde o tiro vai sair e para onde ele vai.
            bul.transform.position = firePoint.transform.position;
            bul.transform.rotation = firePoint.transform.rotation;
            bul.SetActive(true);
            bul.GetComponent<BossBulletScript>().SetMoveDirection(bulDir);

            angle += angleStep;
            
        }
    }
}
