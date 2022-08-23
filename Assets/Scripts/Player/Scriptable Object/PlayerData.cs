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
		Debug.Log("Resetting coins");
		_coins = Constants.DEFAULT_COINS;
		RefreshCoinsText();
	}

}
