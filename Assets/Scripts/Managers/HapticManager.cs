using System.Collections;
using UnityEngine;

public class HapticManager : MonoBehaviour
{
	[SerializeField] private Settings settings;
	private static HapticManager _this;

	private void Awake() { _this = this; }

	public static void Vibrate() { if (_this.settings.haptic) Handheld.Vibrate(); }
}
