using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
	[SerializeField] private DeckManager deckmanager;
	[SerializeField] private TextMeshProUGUI coins_text;

	[SerializeField] private Scrollbar scrollbar;

	private int minvalue;
	private int maxvalue; // TODO: Set min and values of table based on coins
	// TODO: OR Keep Poker Coins
	private int betamount;

	private const float PRECENT = 0.2f;

	[SerializeField] private TextMeshProUGUI min_text;
	[SerializeField] private TextMeshProUGUI max_text;
	[SerializeField] private TextMeshProUGUI bet_text;

	[SerializeField] private GameObject betarea;
	[SerializeField] private GameObject dealarea;

	void Start()
	{
		PlayerData.increaseCoins(0);
		deckmanager.nextDeal = nextDeal;
		deckmanager.dealEnded = dealEnd;
	}

	public void CalculateBetAmount()
	{
		betamount = minvalue + (int)((maxvalue - minvalue) * scrollbar.value);
		bet_text.text = betamount.ToString();
	}

	private void OnEnable()
	{
		PlayerData.UpdateCoins += UpdateCoinsText;
		nextDeal();
	}

	private void OnDisable()
	{
		PlayerData.UpdateCoins -= UpdateCoinsText;
	}

	public void StartDeal()
	{
		deckmanager.betamount = betamount;
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
		minvalue = (int)(PlayerData.coins * PRECENT);
		maxvalue = (int)(PlayerData.coins * (1 - PRECENT));
		min_text.text = minvalue.ToString();
		max_text.text = maxvalue.ToString();
		CalculateBetAmount();
	}

	private void UpdateCoinsText(object sender, EventArgs e)
	{
		coins_text.text = PlayerData.coins.ToString();
		// TODO : Include Graphics
	}



}
