using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerAttributes : MonoBehaviour
{

	[Header("Player Stats")]
    public int currentLife;
	public int maxLife = 3;
	public Image[] heartAmount;
	public Sprite filledHeart;
	public Sprite emptyHeart;
	
	public int currentMana;
	public int maxMana = 100;
	public Image[] manaAmount;
	
	public GameObject[] canUseOrNot;
	
	public Slider playerManaPoolSlider;
	public RectTransform manaFill;

    public GameManagerScript pauseMenuInvk;
	
	public GameObject losePanelObject;
	
	public Player playerScript;
	public BossState bossStateScript;
	
	[Header("HP do player, do boss e temporizador")]
	[SerializeField]TextMeshProUGUI PlayerHealth;
	[SerializeField]TextMeshProUGUI PlayerMana;
	[SerializeField]TextMeshProUGUI BossHealth;
	[SerializeField]TextMeshProUGUI TimeStamp;
	
	private float min;
	private float sec;
	

    // Start is called before the first frame update
    void Start()
    {
		playerScript = GameObject.Find("Player").GetComponent<Player>();
		bossStateScript = GameObject.Find("Boss").GetComponent<BossState>();
        currentLife = maxLife;
        pauseMenuInvk = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
		PlayerHealth.SetText("HP: " + currentLife.ToString());
		currentMana = maxMana;
		PlayerMana.SetText("Mana: " + currentMana.ToString());
		// printar no TMPro text o hp do player
    }

    // pausar se zerar HP
	// temporizador que conta no topo-direita da tela
	// começa em 0min0sec, a cada 60sec, soma 1min e subtrai 60sec
	// depois printa no TMPro text
    void Update()
    {
		UpdateHealth();
		UpdateMana();
		
		sec += Time.deltaTime;
		
		if(sec >= 60f){
			min += 1f;
			sec = 0;
		}
		
		TimeStamp.SetText(min.ToString() + "m " + Mathf.Floor(sec).ToString() + "s");

        if (currentLife <= 0)
        {
            losePanelObject.SetActive(true);
			pauseMenuInvk.pausedGame = true;
			Time.timeScale = 0f;
        }		
    }
	public void UpdateHealth()
	{
		PlayerHealth.SetText("HP: " + currentLife.ToString());
		if(currentLife == 3)
		{
			heartAmount[0].sprite = filledHeart;
			heartAmount[1].sprite = filledHeart;
			heartAmount[2].sprite = filledHeart;
		} else if (currentLife == 2)
		{
			heartAmount[0].sprite = filledHeart;
			heartAmount[1].sprite = filledHeart;
			heartAmount[2].sprite = emptyHeart;
		} else if(currentLife == 1)
		{
			heartAmount[0].sprite = filledHeart;
			heartAmount[1].sprite = emptyHeart;
			heartAmount[2].sprite = emptyHeart;
		} else if(currentLife == 0)
		{
			heartAmount[0].sprite = emptyHeart;
			heartAmount[1].sprite = emptyHeart;
			heartAmount[2].sprite = emptyHeart;
		}
	}
	public void UpdateMana()
	{
		PlayerMana.SetText("Mana: " + Mathf.Clamp(currentMana, 0f, 100f).ToString());
		playerManaPoolSlider.value = Mathf.Clamp(currentMana, 0f, 100f)/maxMana;

		for(int i = 0; i < 5; i++)
		{
			if(currentMana < ((i*20)+20))
			{
				manaAmount[i].fillAmount = (float)(currentMana-(i*20))/20;
			} else 
			{
				manaAmount[i].fillAmount = 1f;
			}
		}
		//aparecer X em cima do botão SHIELD caso não possa usar
		if(currentMana >= 20)
		{
			canUseOrNot[0].gameObject.SetActive(false);
		} else
		{
			canUseOrNot[0].gameObject.SetActive(true);
		}
		
		//aparecer X em cima do botão HEAL caso não possa usar
		if(currentMana >= 100)
		{
			canUseOrNot[1].gameObject.SetActive(false);
		} else
		{
			canUseOrNot[1].gameObject.SetActive(true);
		}
		
		//aparecer X em cima do botão LASER caso não possa usar
		if(currentMana >= 10)
		{
			canUseOrNot[2].gameObject.SetActive(false);
		} else
		{
			canUseOrNot[2].gameObject.SetActive(true);
		}
	}
	
	public void SpendMana(int amount)
	{
		currentMana -= amount;
	}
	
	public void CastHeal()
	{
		if(currentMana == maxMana)
		{
			currentMana = 0;
			currentLife = maxLife;
		}		
	}
	
    void OnTriggerEnter(Collider col)
    {
        /*if (col.gameObject.tag == "BossBullet")
        {
            currentLife--;
            col.gameObject.SetActive(false);
			PlayerHealth.SetText("HP: " + currentLife.ToString());
        }*/
    }

    /*void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Boss")
        {	
			if(!playerScript.isDashing)
			{
				if(playerScript.isShielded)
				{
					playerScript.isShielded = false;
					playerScript.shieldObject.SetActive(false);
				} else
				{
					currentLife--;
					PlayerHealth.SetText("HP: " + currentLife.ToString());
				}
			}
        }
    }*/
}
