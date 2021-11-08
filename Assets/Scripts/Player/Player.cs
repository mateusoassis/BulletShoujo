using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //velocidade de movimento do player e do tiro, respectivamente.
	[Header("Velocidade de Personagem e Bala")]
    public float moveSpeed = 10f;
    public float bulletForce = 20f;
    public float reflectionSpeed;

	[Header("Dash")]
    public float dashForce = 10f;
    public float dashDuration;
    public float startDashTime;
	public int dashAmount;
	public int maxDashAmount = 2;
	public float resetDashCooldown;
	public float resetDashTimer;

    [Header("Melee Attack")]
    public Transform meleePoint;
    public float attackRange = 0.5f;
    public float meleeAttackStrength = 5.0f;
    public LayerMask amayaLayer;
    public BossDamage amayasHp;


	[Header("Booleanos e Direcao")]
    public bool isOnCoolDown;
    public bool isNotMoving;
	public bool isDashing;
	public bool recentlyDamaged;
    public int direction;
	public bool isImmuneToDamage;
    public bool isShielded;
	private CapsuleCollider playerCollider;

	[Header("Rigidbody do Player")]
    public Rigidbody rb;
	
	[Header("Sistema de Mana")]
	public PlayerAttributes playerAttributes;

    [Header("Transform do firePoint da bala")]

    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject laserPrefab;
    public GameObject shieldObject;
	public GameManagerScript gameManager;
	
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
	}

    void Start()
    {
		gameManager.TimeScaleNormal();
		playerAttributes = GetComponent<PlayerAttributes>();
        rb = GetComponent<Rigidbody>();
		recentlyDamaged = false;
        dashDuration = startDashTime;
		isDashing = false;
		playerCollider = GetComponent<CapsuleCollider>();
		resetDashTimer = resetDashCooldown;
        isShielded = false;
    }

    void Update()
    {
        //Definindo a mira para ficar funcionando 100% do tempo no update.

        Aim();
		//DashImmune();
		//lembrar de checar esse imune nas balas depois
		
        //Definindo o bot�o esquerdo do mouse para atirar.

        if (Input.GetButtonDown("Fire1") && !gameManager.pausedGame)
        {
            Shoot();
            FindObjectOfType<AudioManager>().PlayOneShot("MagicShot");
        }
		if (Input.GetKey(KeyCode.E) && playerAttributes.currentMana == playerAttributes.maxMana)
		{
			playerAttributes.CastHeal();
		}
        if(Input.GetMouseButtonDown(1) && !gameManager.pausedGame){
            MeleeAttack();
        }
        if (Input.GetKey(KeyCode.Q))
		{
			StartCoroutine("Beam");
		}
        if(Input.GetKey(KeyCode.F) && playerAttributes.currentMana >= 20)
        {
            isShielded = true;
            shieldObject.SetActive(true);
			playerAttributes.currentMana -= 20;
        }
		
    }

    void FixedUpdate()
    {
        //Inputs de WASD para movimentacao em 8 direcoes utilizando a multiplicacao de velocidade por tempo.

        if (!recentlyDamaged && !isDashing)
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
                if (Input.GetKey(KeyCode.Space) && direction == 1 && !isDashing)
                {
					if(dashAmount < maxDashAmount)
					{
						rb.AddForce((Vector3.right + Vector3.forward) * ((dashForce)/Mathf.Sqrt(2f)), ForceMode.VelocityChange);
						isDashing = true;                    
						direction = 0;
						dashAmount++;
					}					
                }
                else if (Input.GetKey(KeyCode.Space) && direction == 2 && !isDashing)
                {     
					if(dashAmount < maxDashAmount)
					{			
						rb.AddForce((Vector3.left + Vector3.back) * ((dashForce)/Mathf.Sqrt(2f)), ForceMode.VelocityChange);
						isDashing = true;
						direction = 0;
						dashAmount++;
					}
                }
                else if (Input.GetKey(KeyCode.Space) && direction == 3 && !isDashing)
                {     
					if(dashAmount < maxDashAmount)
					{
						rb.AddForce((Vector3.forward + Vector3.left) * ((dashForce)/Mathf.Sqrt(2f)), ForceMode.VelocityChange);
						isDashing = true;
						direction = 0;
						dashAmount++;
					}			
                }
                else if (Input.GetKey(KeyCode.Space) && direction == 4 && !isDashing)
                {     
					if(dashAmount < maxDashAmount)
					{
						rb.AddForce((Vector3.back + Vector3.right) * ((dashForce)/Mathf.Sqrt(2f)), ForceMode.VelocityChange);
						isDashing = true;
						direction = 0;
						dashAmount++;
					}
					
                }
                else if (Input.GetKey(KeyCode.Space) && direction == 5 && !isDashing)
                {     
					if(dashAmount < maxDashAmount)
					{
						rb.AddForce(Vector3.forward * dashForce, ForceMode.VelocityChange);
						isDashing = true;
						direction = 0;
						dashAmount++;
					}				
                }
                else if (Input.GetKey(KeyCode.Space) && direction == 6 && !isDashing)
                {     
					if(dashAmount < maxDashAmount)
					{
						rb.AddForce(Vector3.right * dashForce, ForceMode.VelocityChange);
						isDashing = true;
						direction = 0;
						dashAmount++;
					}					
                }
                else if (Input.GetKey(KeyCode.Space) && direction == 7 && !isDashing)
                {     
					if(dashAmount < maxDashAmount)
					{
						rb.AddForce(Vector3.left * dashForce, ForceMode.VelocityChange);
						isDashing = true;
						direction = 0;
						dashAmount++;
					}					
                }         
                else if (Input.GetKey(KeyCode.Space) && direction == 8 && !isDashing)
                {     
					if(dashAmount < maxDashAmount)
					{
						rb.AddForce(Vector3.back * dashForce, ForceMode.VelocityChange);
						isDashing = true;
						direction = 0;
						dashAmount++;
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
        //Utilizacao de Raycast para identificar a posicao do mouse na tela atrav�s da c�mera.

        RaycastHit hit;

        //Pegando o input de posicao do mouse com raycast.

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit , float.MaxValue, layerMask))
        {
            position = new Vector3(hit.point.x, 0, hit.point.z);
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

    public IEnumerator Beam()
    {
        //Instanciamento de prefab do tiro de personagem.
        yield return new WaitForSeconds(0.5f);
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        Rigidbody laserRb = laser.GetComponent<Rigidbody>();

        //Adicao de forca no tiro para impulsionar o prefab.
        
        laserRb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }

    void MeleeAttack(){
        Collider[] hitEnemies = Physics.OverlapSphere(meleePoint.position, attackRange, amayaLayer);

        foreach(Collider enemy in hitEnemies){


            if(enemy.tag == "Boss" && !gameManager.pausedGame){
                amayasHp = enemy.GetComponent<BossDamage>();
                amayasHp.bossHPCurrent -=meleeAttackStrength;
            }


            else if(enemy.tag == "Mirror" && !gameManager.pausedGame){
                Debug.Log("paixão é chata");
                enemy.gameObject.SetActive(false);
            }
        }
    }

    void OnDrawGizmosSelected() {
        if(meleePoint == null)
            return;

        Gizmos.DrawSphere(meleePoint.position, attackRange);
    }
	
	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Wall"){
			rb.velocity = Vector3.zero;
		}
	}

    void OnTriggerEnter(Collider other){

        if(other.gameObject.tag == "BossBullet" && isShielded)
        {
            isShielded = false;
            shieldObject.SetActive(false);
        }
    }
}
