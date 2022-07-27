using UnityEngine;

// TODO : Enable commented code after selecting all sounds

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource background_sound;
    [SerializeField] private AudioSource card_moving_sound;
    [SerializeField] private AudioSource shuffle_sound;
    [SerializeField] private AudioSource ui_button_sounds;
    [SerializeField] private AudioSource win_status_voice;
    [SerializeField] private AudioClip WIN_VOICE;
    [SerializeField] private AudioClip BLACKJACK_VOICE;
    [SerializeField] private AudioClip PUSH_VOICE;
    [SerializeField] private AudioClip LOSE_VOICE;
    [SerializeField] private AudioClip BUST_VOICE;

    [SerializeField] private Settings settings;


    private static SoundManager _this;

    void Awake()
    {
        _this = this;
        //background_sound.loop = true;
    }

    public static void PlayBackground()
	{
        Debug.Log("Play Background");
        //_this.background_sound.Play();
	}

    public static void StopBackground()
	{
        Debug.Log("Stop Background");
        //_this.background_sound.Stop();
    }

    public static void PlayCardMovingSound()
	{
        if (_this.settings.sound)
        {
            Debug.Log("Play Card Moving");
            //_this.card_moving_sound.Play();
        }
	}

    public static void PlayShuffleSound()
	{
        if (_this.settings.sound)
        {
            Debug.Log("Play Shuffle");
            //_this.shuffle_sound.Play();
        }
	}

    public static void PlayUIElementClickSound()
	{
        if (_this.settings.sound)
        {
            Debug.Log("Play UI Elemnts");
            //_this.ui_button_sounds.Play();
        }
    }

    public static void PlayWinText(WIN_STATUS winstatus)
	{
		/*switch (winstatus)
		{
            case WIN_STATUS.WIN:
                _this.win_status_voice.clip = _this.WIN_VOICE;
                break;
            case WIN_STATUS.BLACKJACK:
                _this.win_status_voice.clip = _this.BLACKJACK_VOICE;
                break;
            case WIN_STATUS.LOSE:
                _this.win_status_voice.clip = _this.LOSE_VOICE;
                break;
            case WIN_STATUS.BUST:
                _this.win_status_voice.clip = _this.BUST_VOICE;
                break;
            case WIN_STATUS.PUSH:
                _this.win_status_voice.clip = _this.PUSH_VOICE;
                break;
        }*/
        if (_this.settings.sound)
        {
            Debug.Log("Play winnig status");
            //_this.win_status_voice.Play();
        }
	}

}
