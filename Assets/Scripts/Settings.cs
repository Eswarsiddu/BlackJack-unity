using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Settings", fileName = "Settings")]
public class Settings : ScriptableObject
{
	[SerializeField] private bool _haptic;
	[SerializeField] private bool _sound;

	public void ToggleHaptic()
	{
		_haptic = !_haptic;
	}

	public void ToggleSound()
	{
		_sound = !_sound;
	}

	public bool haptic { get => _haptic; }

	public bool sound { get => _sound; }

}
