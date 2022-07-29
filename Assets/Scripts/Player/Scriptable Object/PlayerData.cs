using System;
using UnityEngine;


[CreateAssetMenu(menuName ="Scriptable/Playerdata",fileName ="PlayerData")]
public class PlayerData : ScriptableObject
{
	[SerializeField] private int _coins;

	public event EventHandler UpdateCoins;

	public int coins { get => _coins; }

	public void increaseCoins(int amount)
	{
		_coins += amount;
		UpdateCoins.Invoke(null,EventArgs.Empty);
	}

	public void decreaseCoins(int amount)
	{
		_coins -= amount;
		UpdateCoins.Invoke(null, EventArgs.Empty);
	}

	public void ResetCoins()
	{
		_coins = 500;
		increaseCoins(0);
	}
}
