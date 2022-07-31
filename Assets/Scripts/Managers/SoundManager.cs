using UnityEngine;
using System.Collections.Generic;
using System;

public class SoundManager : MonoBehaviour
{
    private AudioSource background_sound;
    private AudioSource card_moving_sound;
    private AudioSource shuffle_sound;
    private AudioSource ui_button_sounds;
    private AudioSource increase_coins_sound;
    private AudioSource win_status_voice;

    private Settings settings;

    private Dictionary<WIN_STATUS, AudioClip> audio_clips;

    private static SoundManager _this;

    private List<AudioSource> sources = new List<AudioSource>();

    public static void SoundToggled()
	{
        foreach(AudioSource source in _this.sources)
            source.mute = !_this.settings.sound;

        if (_this.settings.sound)
            _this.background_sound.Play();
        else
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

    public static void PlayIncreaseCoinsSound()
	{
        _this.increase_coins_sound.Play();
    }

    void Awake()
    {
        _this = this;
        settings = Resources.Load<Settings>(Constants.SETTINGS_PATH);
        foreach (Transform child in transform)
        {
            switch (child.name)
            {
                case "Background":
                    background_sound = child.GetComponent<AudioSource>();
                    sources.Add(background_sound);
                    break;
                case "CardMoving":
                    card_moving_sound = child.GetComponent<AudioSource>();
                    sources.Add(card_moving_sound);
                    break;
                case "shuffling":
                    shuffle_sound = child.GetComponent<AudioSource>();
                    sources.Add(shuffle_sound);
                    break;
                case "UIClick":
                    ui_button_sounds = child.GetComponent<AudioSource>();
                    sources.Add(ui_button_sounds);
                    break;
                case "WinStatus":
                    win_status_voice = child.GetComponent<AudioSource>();
                    sources.Add(win_status_voice);
                    break;
                case "IncreaseCoins":
                    increase_coins_sound = child.GetComponent<AudioSource>();
                    sources.Add(increase_coins_sound);
                    break;
            }
        }

        audio_clips = new Dictionary<WIN_STATUS, AudioClip>();

        foreach (AudioClip clip in Resources.LoadAll<AudioClip>(Constants.VOICES_AUDIO_PATH))
        {
            WIN_STATUS status = (WIN_STATUS)Enum.Parse(typeof(WIN_STATUS), clip.name);

            audio_clips[status] = clip;
        }

        background_sound.loop = true;

        SoundToggled();
    }

}