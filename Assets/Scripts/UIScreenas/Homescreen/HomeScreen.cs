using UnityEngine;
using TMPro;
using System;

public class HomeScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coins_text;
	[SerializeField] private PlayerData playerdata;

	void Start()
	{
		playerdata.increaseCoins(0);
	}

	private void OnEnable()
	{
		playerdata.UpdateCoins += UpdateCoinsText;
	}

	private void OnDisable()
	{
		playerdata.UpdateCoins -= UpdateCoinsText;
	}

	private void UpdateCoinsText(object sender, EventArgs e)
	{
        coins_text.text = playerdata.coins.ToString();
	}
}