using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDamage : MonoBehaviour
{
	[Header("Variáveis da Segunda Fase")]
	public float hpToChangeToPhase2;
	public bool canTakeDamage;
	
	[Header("Vida, Collider e Menu")]
	public BoxCollider bossCollider;

    public float bossHP;
    public float bossHPCurrent;

    public static bool bossIsDead;
    public GameManagerScript pauseMenuInvk;
	
	public GameObject winPanelObject;
	
	[Header("Barra de HP do boss")]
	public Slider BossHPBar;
	public float bossFillBar;
	
	public PlayerAttributes playerAttributes;
    public Player playerScript;

    public GameObject yurinaExplosions;
	
	public BossState bossStateScript;
	public Transform secondPhaseTransform;
	
	[Header("States")]
	public int IDLE_STATE = 0;
	public int MOVE_STATE = 1;
	public int MELEE_STATE = 2;
	public int DASH_STATE = 3;
	public int ORBS_STATE = 4;
	public int FIRE_SWIRL_STATE = 5;
	public int FULL_FIRE_STATE = 6;
	public int MIRROR_CAST = 7;
	public int PHASE_CHANGE = 8;

    // Start is called before the first frame update
    void Start()
    {
		bossStateScript.secondPhase = false;
        bossHPCurrent = bossHP;
        bossIsDead = false;
		BossHPBar.value = bossHPCurrent/bossHP;
		playerAttributes = GameObject.Find("Player").GetComponent<PlayerAttributes>();
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        pauseMenuInvk = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
    }

    void Update()
    {
        if(bossHPCurrent <= hpToChangeToPhase2 && !bossStateScript.secondPhase)
        {
			bossHPCurrent = bossHP;
			bossStateScript.secondPhase = true;
			bossStateScript.ChangeState(PHASE_CHANGE);
        } else if(bossHPCurrent <= 0 && bossStateScript.secondPhase)
		{
			// era pra ter "destroy this gameobject" aqui
			bossIsDead = true;
			Time.timeScale = 0f;
            pauseMenuInvk.pausedGame = true;
			winPanelObject.SetActive(true);
		}
		BossHPBar.value = bossHPCurrent/bossHP;
    }
    
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "PlayerAttack" && !canTakeDamage)
        {
            bossHPCurrent--;
			if(playerAttributes.currentMana < playerAttributes.maxMana){
				playerAttributes.currentMana++;
			}			
            GameObject explosion = Instantiate(yurinaExplosions, transform.position, Quaternion.identity) as GameObject;
            Destroy(col.gameObject);
        }
    }    

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "PlayerLaser")
        {
            bossHPCurrent = bossHPCurrent - Time.deltaTime * playerScript.laserDPS;
        }
    } 
}
