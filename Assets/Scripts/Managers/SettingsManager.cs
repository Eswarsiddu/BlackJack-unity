using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingController
{
    private Button button;
    private Image image;
    private Action[] actions;
    private Func<bool> getState;

    private Color default_color;
    private Color changed_color;

    public SettingController(Button button,Color changed_color,Func<bool> getState,Action[] actions)
	{
        this.button = button;
        this.button.onClick.AddListener(Toggle);
        this.changed_color = changed_color;
        this.getState = getState;
        this.actions = actions;

        image = button.GetComponent<Image>();
        default_color = Color.black;
        UpdateGraphics();
    }

    private void Toggle()
    {
        foreach(Action action in actions)
            action.Invoke();

        UpdateGraphics();
        HapticManager.Vibrate();
        SoundManager.PlayUIElementClickSound();
    }

    public void UpdateGraphics()
    {
        image.color = getState() ? default_color : changed_color;
    }
}

public class SettingsManager : MonoBehaviour
{

    private Settings settings;

    [SerializeField] Color haptic_default;
    [SerializeField] Color sound_default;

    private void Awake()
    {
        settings = Resources.Load<Settings>(Constants.SETTINGS_PATH);
        foreach (Transform child in transform)
        {
            switch (child.tag)
            {
                case TAGS.HAPTIC:
                    Action[] arr = {
                        settings.ToggleHaptic
                    };
                    new SettingController(
                        button: child.GetComponent<Button>(),
                        changed_color: haptic_default,
                        getState: () => settings.haptic,
                        actions: arr
                        );
                    break;

                case TAGS.SOUND:
                    Action[] brr = {
                        settings.ToggleSound,
                        SoundManager.SoundToggled
                    };
                    new SettingController(
                        button: child.GetComponent<Button>(),
                        changed_color: sound_default,
                        getState: () => settings.sound,
                        actions: brr
                        );
                    break;
            }
        }
    }
}
