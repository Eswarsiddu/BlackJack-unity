using System;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable/Playerdata",fileName ="PlayerData")]
public class PlayerData : ScriptableObject
{
	[SerializeField] private int _coins;
	public event EventHandler UpdateCoins;

	public int coins { get => _coins; }


	public void RefreshCoinsText()
	{
		UpdateCoins.Invoke(null, EventArgs.Empty);
	}

	public void increaseCoins(int amount)
	{
		_coins += amount;
		RefreshCoinsText();
	}

	public void decreaseCoins(int amount)
	{
		_coins -= amount;
		RefreshCoinsText();
	}

	public void ResetCoins()
	{
		_coins = Constants.DEFAULT_COINS;
		RefreshCoinsText();
	}

}
