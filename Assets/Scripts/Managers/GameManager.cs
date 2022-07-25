using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : DeckManager
{
    [SerializeField]private Transform cardparent;
    [SerializeField] private Sprite card_back_sprite;
    void Awake()
    {
        Constants.offsiteCardsParent = cardparent;
        Constants.CARDBACKSPRITE = card_back_sprite;
        initializeDeck();
    }


    #region Testing
    [Header("\n\nTesting")]
    public Button PlayButton;
    public Button HitButton;
    public Button StayButton;
	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.P))
		{
            PlayButton.onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            HitButton.onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StayButton.onClick.Invoke();
        }

    }
	#endregion

}
