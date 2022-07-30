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
		if (Input.GetKeyDown(KeyCode.R))
		{
            resetDeck();
		}
    }
	#endregion

}
