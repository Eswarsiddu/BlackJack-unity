using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
	[SerializeField] private DeckManager deckmanager;
	[SerializeField] private TextMeshProUGUI coins_text;

	[SerializeField] private Scrollbar scrollbar;
	[SerializeField] private Button backbutton;

	private PlayerData playerdata;

	private int minvalue;
	private int maxvalue; // TODO: Set min and values of table based on coins
	// TODO: OR Keep Poker Coins
	private int betamount;

	[SerializeField] private TextMeshProUGUI min_text;
	[SerializeField] private TextMeshProUGUI max_text;
	[SerializeField] private TextMeshProUGUI bet_text;

	[SerializeField] private GameObject betarea;
	[SerializeField] private GameObject dealarea;


	private void Awake()
	{
		playerdata = Resources.Load<PlayerData>(Constants.PLAYER_DATA_PATH);
	}

	void Start()
	{
		playerdata.increaseCoins(0);
		deckmanager.nextDeal = nextDeal;
		deckmanager.DisableDealOptions = dealEnd;
		backbutton.onClick.AddListener(SoundManager.PlayUIElementClickSound);
	}

	public void CalculateBetAmount()
	{
		betamount = minvalue + (int)((maxvalue - minvalue) * scrollbar.value);
		bet_text.text = betamount.ToString();
	}

	private void OnEnable()
	{
		playerdata.UpdateCoins += UpdateCoinsText;
		nextDeal();
	}

	private void OnDisable()
	{
		playerdata.UpdateCoins -= UpdateCoinsText;
		deckmanager.resetDeck();
	}

	public void StartDeal()
	{
		SoundManager.PlayUIElementClickSound();
		deckmanager.SetBetAmount(betamount);
		betarea.SetActive(false);
		dealarea.SetActive(true);
		deckmanager.startDeal();
	}

	private void dealEnd()
	{
		betarea.SetActive(false);
		dealarea.SetActive(false);
	}

	private void nextDeal()
	{
		betarea.SetActive(true);
		dealarea.SetActive(false);
		minvalue = (int)(playerdata.coins * Constants.PRECENT);
		maxvalue = (int)(playerdata.coins * (1 - Constants.PRECENT));
		min_text.text = minvalue.ToString();
		max_text.text = maxvalue.ToString();
		CalculateBetAmount();
	}

	private void UpdateCoinsText(object sender, EventArgs e)
	{
		coins_text.text = playerdata.coins.ToString();
		// TODO : Include Graphics
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			backbutton.onClick.Invoke();
		}
	}

}
