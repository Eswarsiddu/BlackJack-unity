using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private Settings settings;

    private Button haptic;
    private Button sound;

    private Image haptic_image;
    private Image sound_image;

    [SerializeField] private Color defaultcolor;
    [SerializeField] private Color changed_color;

    public void ToggleHaptic() // UI Button
	{
        settings.ToggleHaptic();
        HapticManager.Vibrate();
        UpdateHapticGraphics();
        SoundManager.PlayUIElementClickSound();
    }

    public void ToggleSound() // UI Button
    {
        settings.ToggleSound();
        SoundManager.SoundToggled();
        UpdateSoundGraphics();
    }

    private void UpdateHapticGraphics()
    {
        haptic_image.color = settings.haptic ? defaultcolor : changed_color;
    }

    private void UpdateSoundGraphics()
    {
        sound_image.color = settings.sound ? defaultcolor : changed_color;
    }

    private void Awake()
    {
        settings = Resources.Load<Settings>(Constants.SETTINGS_PATH);
        foreach (Transform child in transform)
        {
            switch (child.tag)
            {
                case TAGS.HAPTIC:
                    haptic = child.GetComponent<Button>(); break;

                case TAGS.SOUND:
                    sound = child.GetComponent<Button>(); break;
            }

            if(haptic != null && sound != null) break;
        }
    }

    void Start()
    {
        //defaultcolor = haptic.colors.normalColor;

        haptic.onClick.AddListener(ToggleHaptic);
        sound.onClick.AddListener(ToggleSound);

        haptic_image = haptic.GetComponent<Image>();
        sound_image = sound.GetComponent<Image>();

        UpdateHapticGraphics();
        UpdateSoundGraphics();
    }

}
