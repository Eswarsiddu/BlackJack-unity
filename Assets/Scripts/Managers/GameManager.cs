using UnityEngine;
using UnityEngine.UI;

public class GameManager : DeckManager
{
    [SerializeField]private Transform cardparent;
    void Awake()
    {
        Constants.offsiteCardsParent = cardparent;
        SaveSystem.LoadData();
    }

	private void Start()
	{
        initializeDeck();
    }
}
