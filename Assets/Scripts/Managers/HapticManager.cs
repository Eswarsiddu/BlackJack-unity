using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticManager : MonoBehaviour
{
	[SerializeField] private Settings settings;
	private static HapticManager _this;

	private const int SHORT_TIME = 100;
	private const int MEDIUM_TIME = 250;
	private const int LONG_TIME = 500;

	private AndroidJavaClass unityPlayer;
	private AndroidJavaObject currentActivity;
	private AndroidJavaObject vibrator;


	private void Awake()
	{
		_this = this;
#if UNITY_ANDROID && !UNITY_EDITOR
    unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#endif
	}

	private bool isAndroid()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		return true;
#else
		return false;
#endif
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
			/*
			if (isAndroid())
			{
				vibrator.Call("vibrate", milliseconds);
			}
			else
			{
				*/

				Handheld.Vibrate();
			//}
		}
	}
}
