using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticManager : MonoBehaviour
{
	[SerializeField] private Settings settings;
	private static HapticManager _this;
	private AndroidJavaObject vibrator;

	private const int SHORT_TIME = 100;
	private const int MEDIUM_TIME = 250;
	private const int LONG_TIME = 500;

	private void Awake()
	{
		_this = this;

		/*
		if (isAndroid()){
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			vibrator = currentActivity.Call<AndroidJavaObject>("getSystemSErvice", "vibrator");
		}
		*/
		
	}

	private bool isAndroid()
	{
		return true; // TODO: Check the condition of android
	}

	public static void ShortVibration()
	{
		Debug.Log("Short Vibrattion");
		_this.Vibrate(SHORT_TIME);
	}

	public static void MediumVibration()
	{
		_this.Vibrate(MEDIUM_TIME);
	}

	public static void LongVibration()
	{
		_this.Vibrate(LONG_TIME);
	}

	private void Vibrate(int milliseconds)
	{
		if (settings.haptic)
		{
			if (isAndroid())
			{
				Debug.Log("vibrating:" + milliseconds);
				//vibrator.Call("vibrate", milliseconds); // milliseconds
			}
			else
			{
				Handheld.Vibrate();
			}
		}
	}
}
