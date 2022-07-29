using UnityEngine;
using UnityEngine.UI;

public class CreditsScreen : MonoBehaviour
{
	private Button back;

	private void Awake()
	{
		foreach(Transform child in transform)
		{
			if (child.CompareTag(TAGS.BACK))
			{
				back = child.GetComponent<Button>();
				return;
			}
		}
	}

	void Start()
	{
		back.onClick.AddListener(SoundManager.PlayUIElementClickSound);
		back.onClick.AddListener(HapticManager.Vibrate);
		back.onClick.AddListener( () => gameObject.SetActive(false) );
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			back.onClick.Invoke();
	}

}
