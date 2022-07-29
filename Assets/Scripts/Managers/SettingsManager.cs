using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Settings settings;

    [SerializeField] private Button haptic;
    [SerializeField] private Button sound;

    private Color defaultcolor;
    [SerializeField] private Color changed_color;

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
            StartSound();
        }
        else
        {
            sound.GetComponent<Image>().color = changed_color;
            Stopsound();
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
        UpdateSoundGraphics();
    }

    public void StartSound()
	{
        SoundManager.PlayBackground();
	}

    public void Stopsound()
	{
        SoundManager.StopBackground();
	}

}
