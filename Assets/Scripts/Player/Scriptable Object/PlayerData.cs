using UnityEngine;
using System;


[CreateAssetMenu(menuName ="Scriptable/Playerdata",fileName ="PlayerData")]
public class PlayerData : ScriptableObject
{
	[SerializeField] private int _coins;

	public static event EventHandler UpdateCoins;

	private static PlayerData _this;

	public static int coins { get => _this._coins; }

	public PlayerData()
	{
		_this= this;
	}

	public static void increaseCoins(int amount)
	{
		_this._coins += amount;
		UpdateCoins.Invoke(null,EventArgs.Empty);
	}

	public static void decreaseCoins(int amount)
	{
		_this._coins -= amount;
		UpdateCoins.Invoke(null, EventArgs.Empty);
	}
}
