using UnityEngine;
using System.Collections.Generic;
using System;


public class SoundManager : MonoBehaviour
{
    private AudioSource background_sound;
    private AudioSource card_moving_sound;
    private AudioSource shuffle_sound;
    private AudioSource ui_button_sounds;
    private AudioSource win_status_voice;

    [SerializeField] private Settings settings;

    private const string VOICESPATH = "Sounds/Voices";

    private Dictionary<WIN_STATUS, AudioClip> audio_clips;

    private static SoundManager _this;

    void Awake()
    {
        _this = this;
        
        foreach(Transform child in transform)
		{
			switch (child.name)
			{
                case "Background":
                    background_sound = child.GetComponent<AudioSource>();
                    break;
                case "CardMoving":
                    card_moving_sound = child.GetComponent<AudioSource>();
                    break;
                case "shuffling":
                    shuffle_sound = child.GetComponent<AudioSource>();
                    break;
                case "UIClick":
                    ui_button_sounds = child.GetComponent<AudioSource>();
                    break;
                case "WinStatus":
                    win_status_voice = child.GetComponent<AudioSource>();
                    break;
            }
		}

        audio_clips = new Dictionary<WIN_STATUS, AudioClip>();
        AudioClip[] clips = Resources.LoadAll<AudioClip>(VOICESPATH);
        foreach(AudioClip clip in clips)
		{
            WIN_STATUS status = (WIN_STATUS)Enum.Parse(typeof(WIN_STATUS), clip.name);

            audio_clips[status] = clip;
		}

		background_sound.loop = true;

		if (settings.sound)
			PlayBackground();
	}

    public static void SoundToggled()
	{
        _this.card_moving_sound.mute = !_this.settings.sound;
        _this.shuffle_sound.mute = !_this.settings.sound;
        _this.ui_button_sounds.mute = !_this.settings.sound;
        _this.win_status_voice.mute = !_this.settings.sound;

		if (_this.settings.sound)
		{
            PlayBackground();
		}
		else
		{
            StopBackground();
		}
    }
    
    public static void PlayBackground()
	{
        _this.background_sound.Play();
	}

    public static void StopBackground()
	{
        _this.background_sound.Stop();
    }

    public static void PlayCardMovingSound()
	{
            _this.card_moving_sound.Play();
	}

    public static void PlayShuffleSound()
	{
           _this.shuffle_sound.Play();
	}

    public static void PlayUIElementClickSound()
	{
            _this.ui_button_sounds.Play();
    }

    public static void PlayWinText(WIN_STATUS winstatus)
	{
            _this.win_status_voice.clip = _this.audio_clips[winstatus];
            _this.win_status_voice.Play();
	}

}
