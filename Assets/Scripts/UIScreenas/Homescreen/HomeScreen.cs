using UnityEngine;
using TMPro;
using System;

public class HomeScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coins_text;

	void Start()
	{
		PlayerData.increaseCoins(0);
	}

	private void OnEnable()
	{
		PlayerData.UpdateCoins += UpdateCoinsText;
	}

	private void OnDisable()
	{
		PlayerData.UpdateCoins -= UpdateCoinsText;
	}

	private void UpdateCoinsText(object sender, EventArgs e)
	{
        coins_text.text = PlayerData.coins.ToString();
	}
}
