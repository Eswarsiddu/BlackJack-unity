using System.Collections;
using UnityEngine;

public class HapticManager : MonoBehaviour
{
	private Settings settings;
	private static HapticManager _this;

	private void Awake()
	{
		_this = this;
		settings = Resources.Load<Settings>(Constants.SETTINGS_PATH);
	}

	public static void Vibrate() { if (_this.settings.haptic) Handheld.Vibrate(); }
}
