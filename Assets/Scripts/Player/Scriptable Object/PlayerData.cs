using System;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable/Playerdata",fileName ="PlayerData")]
public class PlayerData : ScriptableObject
{
	[SerializeField] private int _coins;
	public event EventHandler UpdateCoins;

	public int coins { get => _coins; }

	void ForceSerialization()
	{
#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetDirty(this);
#endif
	}

	public void RefreshCoinsText()
	{
		UpdateCoins.Invoke(null, EventArgs.Empty);
		SaveSystem.SaveData();
	}

	public void increaseCoins(int amount)
	{
		_coins += amount;
		ForceSerialization();
		RefreshCoinsText();
		SoundManager.PlayIncreaseCoinsSound();
	}

	public void decreaseCoins(int amount)
	{
		_coins -= amount;
		ForceSerialization();
		RefreshCoinsText();
	}

	public void ResetCoins()
	{
		_coins = Constants.DEFAULT_COINS;
		ForceSerialization();
		RefreshCoinsText();
	}

	public void SetCoins(int coins)
	{
		_coins = coins;
	}
}
