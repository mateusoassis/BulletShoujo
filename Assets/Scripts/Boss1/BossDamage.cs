using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDamage : MonoBehaviour
{


    public float bossHP;
    public float bossHPCurrent;

    public static bool bossIsDead;
    public GameManagerScript pauseMenuInvk;
	
	public GameObject winPanelObject;
	
	[Header("Barra de HP do boss")]
	public Slider BossHPBar;
	public float bossFillBar;
	
	public PlayerAttributes playerAttributes;

    // Start is called before the first frame update
    void Start()
    {
        bossHPCurrent = bossHP;
        bossIsDead = false;
		BossHPBar.value = bossHPCurrent/bossHP;
		playerAttributes = GameObject.Find("Player").GetComponent<PlayerAttributes>();
        pauseMenuInvk = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bossHPCurrent <= 0)
        {
            Destroy(this.gameObject);
            bossIsDead = true;
			winPanelObject.SetActive(true);
			Time.timeScale = 0f;
            pauseMenuInvk.pausedGame = true;
        }
		BossHPBar.value = bossHPCurrent/bossHP;
    }
    
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "PlayerAttack")
        {
            bossHPCurrent--;
			if(playerAttributes.currentMana < playerAttributes.maxMana){
				playerAttributes.currentMana++;
			}			
            Destroy(col.gameObject);
        }
    }
       
}
