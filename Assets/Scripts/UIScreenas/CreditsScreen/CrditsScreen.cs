using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrditsScreen : MonoBehaviour
{
    [SerializeField] private Button back;
    
    void Start()
    {
        back.onClick.AddListener(CloseScreen);
    }

    private void CloseScreen()
	{
        gameObject.SetActive(false);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
		{
            CloseScreen();
		}
    }
}
