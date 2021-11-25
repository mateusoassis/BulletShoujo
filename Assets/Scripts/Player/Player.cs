using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public Animator yurinaAnimator;
	public GameObject characterModel;
	
    //velocidade de movimento do player e do tiro, respectivamente.
	[Header("Velocidade de Personagem e Bala")]
    public float moveSpeed = 10f;
    public float bulletForce = 20f;
    public float reflectionSpeed;
	[SerializeField] private float fireRateTimer;
	public float fireRate; // a cada x segundos pode atirar
	[SerializeField] private float meleeRateTimer;
	public float meleeRate;

	[Header("Dash")]
    public float dashForce = 10f;
    public float dashDuration;
    public float startDashTime;
	public int dashAmount;
	public int maxDashAmount = 2;
	public float resetDashCooldown;
	public float resetDashTimer;
    public float damageResetTimer;
    public float laserDPS;

    [Header("Melee Attack")]
    public Transform meleePoint;
    public float attackRange = 0.5f;
    public float meleeAttackStrength = 5.0f;
    public LayerMask amayaLayer;
    public BossDamage amayasHp;


	[Header("Booleanos e Direcao")]
    public bool isOnCoolDown;
	public bool isAttacking;
	public bool isShooting;
    public bool isNotMoving;
	public bool isDashing;
	public bool recentlyDamaged;
    public int direction;
	public bool isImmuneToDamage;
    public bool isShielded;
    public bool canBeDamaged;
	public bool castingLaser;
	private CapsuleCollider playerCollider;

	[Header("Rigidbody do Player")]
    public Rigidbody rb;
	
	[Header("Sistema de Mana")]
	public PlayerAttributes playerAttributes;

    [Header("Transform do firePoint da bala")]

    public Transform firePoint;

    public Transform laserFirePoint;
    public GameObject bulletPrefab;
    public GameObject laserPrefab;
    public GameObject shieldObject;
	public GameManagerScript gameManager;
    public GameObject singleShield;
    public GameObject yurinaHealing;
    public GameObject yurinaExplosions;
    public GameObject yurinaDash;
    public GameObject spawnedLaser;
	private BossMirrorAttack bossMirrorAttack;
	[SerializeField] private TUTORIALMirror tutorialMirror;
	
	//direcoes
	// 1 cima
	// 2 baixo
	// 3 esquerda
	// 4 direita
	// 5 cima-esquerda
	// 6 cima-direita
	// 7 baixo-esquerda
	// 8 baixo-direita

    //Vetor de posicao para utilizar no raycast.

    Vector3 position;
	
	//layer do chao para o raycast ser mirado apenas no ch�o 
	[SerializeField] public LayerMask layerMask;

	void Awake()
	{
		gameManager = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
		bossMirrorAttack = GameObject.Find("MirrorPoint").GetComponent<BossMirrorAttack>();
		spawnedLaser = Instantiate(laserPrefab, laserFirePoint.transform) as GameObject;
		DisableLaserOnAwake();
	}

    void Start()
    {
		castingLaser = false;
		gameManager.TimeScaleNormal();
		playerAttributes = GetComponent<PlayerAttributes>();
        rb = GetComponent<Rigidbody>();
		recentlyDamaged = false;
        dashDuration = startDashTime;
		isDashing = false;
		playerCollider = GetComponent<CapsuleCollider>();
		resetDashTimer = resetDashCooldown;
        isShielded = false;
		gameManager.gameStarted = true;
        singleShield.SetActive(false);
        canBeDamaged = true;
    }

    void Update()
    {
        //Definindo a mira para ficar funcionando 100% do tempo no update.
		if(!gameManager.fadingToMenu)
		{
			Aim();
		}
        
		//DashImmune();
		//lembrar de checar esse imune nas balas depois
		
        //Definindo o botao esquerdo do mouse para atirar.
		
		// check do tiro normal
		if(fireRateTimer > 0)
		{
			fireRateTimer -= Time.deltaTime;
		}
		
		if(Input.GetButton("Fire1") && !gameManager.pausedGame && fireRateTimer <= 0 && !isAttacking && !gameManager.fadingToMenu)
        {
			isShooting = true;
			yurinaAnimator.SetBool("isShooting", true);		
		} else if(Input.GetButtonUp("Fire1") && !gameManager.pausedGame)
		{
			isShooting = false;
			yurinaAnimator.SetBool("isShooting", false);
		}
		
		// tiro normal
        if (Input.GetButton("Fire1") && !gameManager.pausedGame && fireRateTimer <= 0 && !isDashing && !isAttacking && !gameManager.fadingToMenu && !castingLaser)
        {
            Shoot();
			isShooting = true;
            FindObjectOfType<AudioManager>().PlayOneShot("MagicShot");
			fireRateTimer = fireRate;
        }
		
		// cura
		if (Input.GetKeyDown(KeyCode.R) && playerAttributes.currentMana == playerAttributes.maxMana && !gameManager.fadingToMenu && !gameManager.pausedGame && playerAttributes.currentLife < 6)
		{
            GameObject healing = Instantiate(yurinaHealing,transform.position, yurinaHealing.transform.rotation) as GameObject;
			playerAttributes.CastHeal();
            FindObjectOfType<AudioManager>().PlayOneShot("HealSpell");
		}
		
		// check do golpe melee
		if(meleeRateTimer > 0)
		{
			meleeRateTimer -= Time.deltaTime;
		}
		
		if(Input.GetKeyDown(KeyCode.Q) && !gameManager.pausedGame && meleeRateTimer <= 0 && !isShooting && !isAttacking && !gameManager.fadingToMenu && !isDashing && !castingLaser)
		{
			isAttacking = true;
			yurinaAnimator.SetBool("isAttacking", true);
		} else if(Input.GetKeyUp(KeyCode.Q) && !gameManager.pausedGame && !isShooting)
		{
			StartCoroutine("MeleeAttackCooldown");
		}
		
		// golpe melee
        if(Input.GetKeyUp(KeyCode.Q) && !gameManager.pausedGame && meleeRateTimer <= 0 && !isDashing && !isShooting && !gameManager.fadingToMenu && !castingLaser){
            MeleeAttack();
			meleeRateTimer = meleeRate;
			//isAttacking = true;
            FindObjectOfType<AudioManager>().PlayOneShot("YurinaMeleeSwoosh");
        } 
		
		// raio que vai ser no NÚMERO/LETRA R
        if (Input.GetMouseButtonDown(1) && playerAttributes.currentMana >= 5 && !gameManager.fadingToMenu && !gameManager.pausedGame && !isDashing && !isShooting)
		{
			castingLaser = true;
            EnableLaser();
        }else if(Input.GetMouseButtonDown(1) && playerAttributes.currentMana < 5 && !gameManager.fadingToMenu && !gameManager.pausedGame && !isDashing && !isShooting)
        {
			castingLaser = false;
            DisableLaser();
        }

        if(Input.GetMouseButton(1) && playerAttributes.currentMana >= 5 && !gameManager.fadingToMenu && !gameManager.pausedGame && !isDashing && !isShooting)
        {
            UpdateLaser();
        }

        if(Input.GetMouseButtonUp(1) && !gameManager.fadingToMenu && !gameManager.pausedGame && !isDashing && !isShooting)
        {
			castingLaser = false;
            DisableLaser();
        }
		
		// shield
        if(Input.GetKey(KeyCode.E) && playerAttributes.currentMana >= 20 && !gameManager.pausedGame && !isShielded && !gameManager.fadingToMenu)
        {
            isShielded = true;
			playerAttributes.currentMana -= 20;
            shieldObject.SetActive(true);
            FindObjectOfType<AudioManager>().PlayOneShot("ShieldSpell");
        }

        if(isShielded)
        {
            singleShield.SetActive(true);
        }else if(!isShielded)
        {
            singleShield.SetActive(false);
        }
		
    }

    void FixedUpdate()
    {
        //Inputs de WASD para movimentacao em 8 direcoes utilizando a multiplicacao de velocidade por tempo.

        if (!recentlyDamaged && !isDashing && !gameManager.fadingToMenu && !gameManager.pausedGame)
        {

            if (Input.GetKey(KeyCode.W))
            {
                rb.MovePosition(rb.position + (Vector3.right + Vector3.forward) * (moveSpeed) * Time.fixedDeltaTime);
                direction = 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                rb.MovePosition(rb.position + (Vector3.left + Vector3.back) * (moveSpeed) * Time.fixedDeltaTime);
                direction = 2;
            }

            if (Input.GetKey(KeyCode.A))
            {
                rb.MovePosition(rb.position + (Vector3.forward + Vector3.left) * (moveSpeed) * Time.fixedDeltaTime);
                direction = 3;
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.MovePosition(rb.position + (Vector3.back + Vector3.right) * (moveSpeed) * Time.fixedDeltaTime);
                direction = 4;
            }

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                direction = 5;
            }

            if (Input.GetKey(KeyCode.W)&& Input.GetKey(KeyCode.D))
            {
                direction = 6;
            }

            if (Input.GetKey(KeyCode.S)&& Input.GetKey(KeyCode.A))
            {
                direction = 7;
            }

            if (Input.GetKey(KeyCode.S)&& Input.GetKey(KeyCode.D))
            {
                direction = 8;
            }
        }
            if (dashDuration <= 0)
            {
                dashDuration = startDashTime;
                rb.velocity = Vector3.zero;
				isDashing = false;
            }
            else
            {
                
				//FORÇA ADICIONADA EM CADA DIREÇÃO, os que tem /mathf.sqrt(2f) são por que são em diagonal, aí precisa dividir por raiz de 2 para ter o dash na mesma distância que os nas direções normais
                if (Input.GetKey(KeyCode.Space) && direction == 1 && !isDashing && !castingLaser)
                {
					if(dashAmount < maxDashAmount)
					{
                        GameObject dashTrail = Instantiate(yurinaDash,transform.position, transform.rotation) as GameObject;
                        dashTrail.transform.parent = gameObject.transform;
						rb.AddForce((Vector3.right + Vector3.forward) * ((dashForce)/Mathf.Sqrt(2f)), ForceMode.VelocityChange);
						isDashing = true;                    
						direction = 0;
						dashAmount++;
						isShooting = false;
					}					
                }
                else if (Input.GetKey(KeyCode.Space) && direction == 2 && !isDashing && !castingLaser)
                {     
					if(dashAmount < maxDashAmount)
					{			
                        GameObject dashTrail = Instantiate(yurinaDash,transform.position, transform.rotation) as GameObject;
                        dashTrail.transform.parent = gameObject.transform;
						rb.AddForce((Vector3.left + Vector3.back) * ((dashForce)/Mathf.Sqrt(2f)), ForceMode.VelocityChange);
						isDashing = true;
						direction = 0;
						dashAmount++;
						isShooting = false;
					}
                }
                else if (Input.GetKey(KeyCode.Space) && direction == 3 && !isDashing && !castingLaser)
                {     
					if(dashAmount < maxDashAmount)
					{
                        GameObject dashTrail = Instantiate(yurinaDash,transform.position, transform.rotation) as GameObject;
                        dashTrail.transform.parent = gameObject.transform;
						rb.AddForce((Vector3.forward + Vector3.left) * ((dashForce)/Mathf.Sqrt(2f)), ForceMode.VelocityChange);
						isDashing = true;
						direction = 0;
						dashAmount++;
						isShooting = false;
					}			
                }
                else if (Input.GetKey(KeyCode.Space) && direction == 4 && !isDashing && !castingLaser)
                {     
					if(dashAmount < maxDashAmount)
					{
                        GameObject dashTrail = Instantiate(yurinaDash,transform.position, transform.rotation) as GameObject;
                        dashTrail.transform.parent = gameObject.transform;
						rb.AddForce((Vector3.back + Vector3.right) * ((dashForce)/Mathf.Sqrt(2f)), ForceMode.VelocityChange);
						isDashing = true;
						direction = 0;
						dashAmount++;
						isShooting = false;
					}
					
                }
                else if (Input.GetKey(KeyCode.Space) && direction == 5 && !isDashing && !castingLaser)
                {     
					if(dashAmount < maxDashAmount)
					{
                        GameObject dashTrail = Instantiate(yurinaDash,transform.position, transform.rotation) as GameObject;
                        dashTrail.transform.parent = gameObject.transform;
						rb.AddForce(Vector3.forward * dashForce, ForceMode.VelocityChange);
						isDashing = true;
						direction = 0;
						dashAmount++;
						isShooting = false;
					}				
                }
                else if (Input.GetKey(KeyCode.Space) && direction == 6 && !isDashing && !castingLaser)
                {     
					if(dashAmount < maxDashAmount)
					{
                        GameObject dashTrail = Instantiate(yurinaDash,transform.position, transform.rotation) as GameObject;
                        dashTrail.transform.parent = gameObject.transform;
						rb.AddForce(Vector3.right * dashForce, ForceMode.VelocityChange);
						isDashing = true;
						direction = 0;
						dashAmount++;
						isShooting = false;
					}					
                }
                else if (Input.GetKey(KeyCode.Space) && direction == 7 && !isDashing && !castingLaser)
                {     
					if(dashAmount < maxDashAmount)
					{
                        GameObject dashTrail = Instantiate(yurinaDash,transform.position, transform.rotation) as GameObject;
                        dashTrail.transform.parent = gameObject.transform;
						rb.AddForce(Vector3.left * dashForce, ForceMode.VelocityChange);
						isDashing = true;
						direction = 0;
						dashAmount++;
						isShooting = false;
					}					
                }         
                else if (Input.GetKey(KeyCode.Space) && direction == 8 && !isDashing && !castingLaser)
                {     
					if(dashAmount < maxDashAmount)
					{
						GameObject dashTrail = Instantiate(yurinaDash,transform.position, transform.rotation) as GameObject;
                        dashTrail.transform.parent = gameObject.transform;
						rb.AddForce(Vector3.back * dashForce, ForceMode.VelocityChange);
						isDashing = true;
						direction = 0;
						dashAmount++;
						isShooting = false;
					}
                }
				if(isDashing)
				{
					dashDuration -= Time.deltaTime;
				}
				if(dashAmount > 0 && resetDashTimer > 0f)
				{
					resetDashTimer -= Time.deltaTime;
				} else 
				{
					ResetDash();
				}
					
            }
        }
		
	public void ResetDash()
	{
		dashAmount = 0;
		resetDashTimer = resetDashCooldown;
	}
	
    void Aim()
    {
        // Utilizacao de Raycast para identificar a posicao do mouse na tela atrav�s da c�mera.
        RaycastHit hit;

        // Pegando o input de posicao do mouse com raycast.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit , float.MaxValue, layerMask))
        {
            position = new Vector3(hit.point.x, 0f, hit.point.z);
        }

        //Criando quaternion que define a rotacao do player em relacao ao mouse.
        Quaternion newRotation = Quaternion.LookRotation(position - transform.position, Vector3.forward);

        //Zerando o quaternion de x e z para evitar que o player rode de formas estranhas...
        newRotation.x = 0f; 
        newRotation.z = 0f;

        //controle da velocidade da rota��o.
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 20);
    }
	
	/*public void DashImmune()
	{
		if(isDashing)
		{
			playerCollider.enabled = false;
		} else {
			playerCollider.enabled = true;
		}
	}*/
	
    void Shoot()
    {
        //Instanciamento de prefab do tiro de personagem.
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        //Adicao de forca no tiro para impulsionar o prefab.
        bulletRb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }

    public void EnableLaser()
    {
        if(playerAttributes.currentMana >= 5f)
		{
			FindObjectOfType<AudioManager>().PlayOneShot("LaserSpell1");
			spawnedLaser.SetActive(true); 
			FindObjectOfType<AudioManager>().PlayOneShot("LaserSpell2");
		}
    }

    public void UpdateLaser()
    {
        spawnedLaser.transform.position = laserFirePoint.transform.position;
        playerAttributes.currentMana = playerAttributes.currentMana - Time.deltaTime * 8;
		if(playerAttributes.currentMana < 5f)
		{
			DisableLaser();
		}
    }
    public void DisableLaser()
    {
        FindObjectOfType<AudioManager>().Play("LaserSpell3");
        spawnedLaser.SetActive(false);
    }

	public void DisableLaserOnAwake()
    {
        spawnedLaser.SetActive(false);
    }

    void MeleeAttack(){
        Collider[] hitEnemies = Physics.OverlapSphere(meleePoint.position, attackRange, amayaLayer);

        foreach(Collider enemy in hitEnemies){


            if(enemy.tag == "Boss" && !gameManager.pausedGame)
            {
                amayasHp = enemy.GetComponent<BossDamage>();
                amayasHp.bossHPCurrent -=meleeAttackStrength;
				StartCoroutine(WaitMeleeBossSound(0.3f));
                //MeleeBossSound();FindObjectOfType<AudioManager>().PlayOneShot("YurinaMeleeConnect");
                //GameObject explosion = Instantiate(yurinaExplosions, meleePoint.transform.position, Quaternion.identity) as GameObject;
            }

            if(enemy.tag == "Mirror1" && !gameManager.pausedGame)
            {
				Debug.Log("acertou mirror 1");
                if(gameManager.gameStarted)
				{
					bossMirrorAttack.Mirror1Break();
				} else if(gameManager.tutorialStarted)
				{
					tutorialMirror.Mirror1Break();
				}
				StartCoroutine(WaitMeleeMirrorSound(0.1f));
                //MirrorBreakSound();//FindObjectOfType<AudioManager>().PlayOneShot("YurinaMeleeMirrorBreak");
                //GameObject explosion = Instantiate(yurinaExplosions, meleePoint.transform.position, Quaternion.identity) as GameObject;
            }
			
			if(enemy.tag == "Mirror2"  && !gameManager.pausedGame)
			{
				Debug.Log("acertou mirror 2");
				if(gameManager.gameStarted)
				{
					bossMirrorAttack.Mirror2Break();
				} else if(gameManager.tutorialStarted)
				{
					tutorialMirror.Mirror2Break();
				}
				StartCoroutine(WaitMeleeMirrorSound(0.1f));
                //MirrorBreakSound();//FindObjectOfType<AudioManager>().PlayOneShot("YurinaMeleeMirrorBreak");
                //GameObject explosion = Instantiate(yurinaExplosions, meleePoint.transform.position, Quaternion.identity) as GameObject;
			}

			if (enemy.tag == "Mirror3" && !gameManager.pausedGame)
			{
				Debug.Log("acertou mirror 3");
				
				if (gameManager.tutorialStarted)
				{
					tutorialMirror.Mirror3Break();
				}
				StartCoroutine(WaitMeleeMirrorSound(0.1f));
				//MirrorBreakSound();//FindObjectOfType<AudioManager>().PlayOneShot("YurinaMeleeMirrorBreak");
				//GameObject explosion = Instantiate(yurinaExplosions, meleePoint.transform.position, Quaternion.identity) as GameObject;
			}

			if (enemy.tag == "Mirror4" && !gameManager.pausedGame)
			{
				Debug.Log("acertou mirror 4");

				if (gameManager.tutorialStarted)
				{
					tutorialMirror.Mirror4Break();
				}
				StartCoroutine(WaitMeleeMirrorSound(0.1f));
				//MirrorBreakSound();//FindObjectOfType<AudioManager>().PlayOneShot("YurinaMeleeMirrorBreak");
				//GameObject explosion = Instantiate(yurinaExplosions, meleePoint.transform.position, Quaternion.identity) as GameObject;
			}
		}
    }
	
	public void MeleeBossSound()
	{
		FindObjectOfType<AudioManager>().PlayOneShot("YurinaMeleeConnect");
		GameObject explosion = Instantiate(yurinaExplosions, meleePoint.transform.position, Quaternion.identity) as GameObject;
	}
	public void MirrorBreakSound()
	{
		FindObjectOfType<AudioManager>().PlayOneShot("YurinaMeleeMirrorBreak");
		GameObject explosion = Instantiate(yurinaExplosions, meleePoint.transform.position, Quaternion.identity) as GameObject;
	}
	public IEnumerator WaitMeleeBossSound(float n)
	{
		yield return new WaitForSeconds(n);
		MeleeBossSound();
	}
	public IEnumerator WaitMeleeMirrorSound(float n)
	{
		yield return new WaitForSeconds(n);
		MirrorBreakSound();
	}
    void OnDrawGizmosSelected() {
        if(meleePoint == null)
            return;

        Gizmos.DrawSphere(meleePoint.position, attackRange);
    }
	
	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Wall")
		{
			rb.velocity = Vector3.zero;
		} else if(col.gameObject.tag == "Boss")
		{
			rb.velocity = Vector3.zero;
		}
	}

    void OnTriggerEnter(Collider other){

        if(other.gameObject.tag == "BossBullet" && isShielded &&!isDashing)
        {
            isShielded = false;
            shieldObject.SetActive(false);
        }
    }
	
	void OnDestroy()
	{
		Debug.Log("destruído");
	}
	
	public IEnumerator MeleeAttackCooldown()
	{
		yield return new WaitForSeconds(0.3f);
		isAttacking = false;
		yurinaAnimator.SetBool("isAttacking", false);
	}

    public IEnumerator DamagedReset()
    {
		StartCoroutine("CharacterBlinking");
        yield return new WaitForSeconds(damageResetTimer);
        canBeDamaged = true;
    }
	
	public IEnumerator CharacterBlinking()
	{
		yield return new WaitForSeconds(0.150f);
		characterModel.SetActive(false);
		yield return new WaitForSeconds(0.140f);
		characterModel.SetActive(true);
		yield return new WaitForSeconds(0.130f);
		characterModel.SetActive(false);
		yield return new WaitForSeconds(0.120f);
		characterModel.SetActive(true);
		yield return new WaitForSeconds(0.110f);
		characterModel.SetActive(false);
		yield return new WaitForSeconds(0.110f);
		characterModel.SetActive(true);
		yield return new WaitForSeconds(0.100f);
		characterModel.SetActive(false);
		yield return new WaitForSeconds(0.100f);
		characterModel.SetActive(true);
		yield return new WaitForSeconds(0.090f);
		characterModel.SetActive(false);
		yield return new WaitForSeconds(0.090f);
		characterModel.SetActive(true);
		yield return new WaitForSeconds(0.080f);
		characterModel.SetActive(false);
		yield return new WaitForSeconds(0.080f);
		characterModel.SetActive(true);
		yield return new WaitForSeconds(0.070f);
		characterModel.SetActive(false);
		yield return new WaitForSeconds(0.070f);
		characterModel.SetActive(true);
		yield return new WaitForSeconds(0.060f);
		characterModel.SetActive(false);
		yield return new WaitForSeconds(0.060f);
		characterModel.SetActive(true);
		/*yield return new WaitForSeconds(0.050f);
		characterModel.SetActive(false);
		yield return new WaitForSeconds(0.050f);
		characterModel.SetActive(true);
		yield return new WaitForSeconds(0.050f);
		characterModel.SetActive(false);
		yield return new WaitForSeconds(0.050f);
		characterModel.SetActive(true);
		yield return new WaitForSeconds(0.050f);
		characterModel.SetActive(false);
		yield return new WaitForSeconds(0.050f);*/
		yield return new WaitForSeconds(0.050f);
		characterModel.SetActive(false);
		yield return new WaitForSeconds(0.040f);
		characterModel.SetActive(true);
		yield return new WaitForSeconds(0.030f);
		characterModel.SetActive(false);
		yield return new WaitForSeconds(0.030f);
		characterModel.SetActive(true);
	}
}
