using UnityEngine;
using UnityEngine.UI;

public class GameManager : DeckManager
{
    [SerializeField]private Transform cardparent;
    void Awake()
    {
        Constants.offsiteCardsParent = cardparent;
    }

	private void Start()
	{
        initializeDeck();
    }
}
