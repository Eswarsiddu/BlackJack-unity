using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private Settings settings;

    private Button haptic;
    private Button sound;

    private Color defaultcolor;
    [SerializeField] private Color changed_color;


	private void Awake()
	{
        settings= Resources.Load<Settings>(Constants.SETTINGS_PATH);
        foreach (Transform child in transform)
        {
            switch (child.tag)
            {
                case TAGS.HAPTIC:
                    haptic = child.GetComponent<Button>();
                    break;
                case TAGS.SOUND:
                    sound = child.GetComponent<Button>();
                    break;
                default:
                    break;
            }
        }
    }

	void Start()
    {
        defaultcolor = haptic.colors.normalColor;
        UpdateHapticGraphics();
        UpdateSoundGraphics();
        haptic.onClick.AddListener(ToggleHaptic);
        sound.onClick.AddListener(ToggleSound);
    }

    private void UpdateHapticGraphics()
	{
        if (settings.haptic)
		{
            haptic.GetComponent<Image>().color = defaultcolor;
		}
		else
		{
            haptic.GetComponent<Image>().color = changed_color;
        }
    }

    private void UpdateSoundGraphics()
    {
        if (settings.sound)
        {
            sound.GetComponent<Image>().color = defaultcolor;
        }
        else
        {
            sound.GetComponent<Image>().color = changed_color;
        }
    }

    public void ToggleHaptic() // UI Button
	{
        SoundManager.PlayUIElementClickSound();
        settings.ToggleHaptic();
        UpdateHapticGraphics();
        HapticManager.Vibrate();
    }

    public void ToggleSound() // UI Button
    {
        settings.ToggleSound();
        SoundManager.SoundToggled();
        UpdateSoundGraphics();
    }
}
