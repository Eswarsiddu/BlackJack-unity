using System;
using TMPro;
using UnityEngine;

public class HomeScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coins_text;
	[SerializeField] private GameObject low_balance_pop_up;
	[SerializeField] private GameObject game_screen;
	private PlayerData playerdata;

	private void Awake()
	{
		playerdata = Resources.Load<PlayerData>(Constants.PLAYER_DATA_PATH);
	}

	private void OnEnable()
	{
		playerdata.UpdateCoins += UpdateCoinsText;
		playerdata.RefreshCoinsText();
	}

	private void OnDisable()
	{
		playerdata.UpdateCoins -= UpdateCoinsText;
	}

	private void UpdateCoinsText(object sender, EventArgs e)
	{
        coins_text.text = playerdata.coins.ToString();
	}

	public void ResetCoins() // UI button
	{
		playerdata.ResetCoins();
		low_balance_pop_up.SetActive(false);
	}

	public void insufficentbalance()
	{
		low_balance_pop_up.SetActive(true);
	}

	public void PlayPressed()
	{
		if(Constants.GetMinimumBet(playerdata.coins) == 0)
		{
			insufficentbalance();
		}
		else
		{
			game_screen.SetActive(true);
			HapticManager.Vibrate();
			gameObject.SetActive(false);
		}
	}

}
