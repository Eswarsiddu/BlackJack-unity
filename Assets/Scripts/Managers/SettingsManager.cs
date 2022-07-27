using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Settings settings;

    [SerializeField] private Button haptic;
    [SerializeField] private Button sound;

    private Color defaultcolor;
    [SerializeField] private Color changedcolor;

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
            StartHaptic();
		}
		else
		{
            haptic.GetComponent<Image>().color = changedcolor;
            StopHaptic();
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
            sound.GetComponent<Image>().color = changedcolor;
            Stopsound();
        }
    }

    public void ToggleHaptic() // UI Button
	{
        Debug.Log("Toggle Haptic");
        settings.ToggleHaptic();
        UpdateHapticGraphics();
	}

    public void ToggleSound() // UI Button
    {
        settings.ToggleSound();
        UpdateSoundGraphics();
    }

    public void StartSound()
	{

	}

    public void Stopsound()
	{

	}

    public void StartHaptic()
	{
        
	}

    public void StopHaptic()
	{

	}

}
