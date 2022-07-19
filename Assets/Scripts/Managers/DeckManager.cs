using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class DeckManager : MonoBehaviour
{

    private Stack<Card> cards;
    private List<Card> finished_cards;
    private bool deal_end;
    
    [SerializeField] private PlayerDeck player_deck;
    [SerializeField] private DealerDeck dealer_deck;

    [SerializeField] private GameObject hit_button;
    [SerializeField] private GameObject stay_button;

    const string IMAGESPATH = "PlayingCards";
    [SerializeField] private GameObject card_prefab;

    protected void initializeDeck()
	{
        cards = new Stack<Card>();
        deal_end = false;
        finished_cards = new List<Card>();
        generateDeck();
	}
    
    private void generateDeck()
    {
        Sprite[] cardsprites = Resources.LoadAll<Sprite>(IMAGESPATH);
        foreach(Sprite sprite in cardsprites)
		{
            string name = sprite.name;

            if(name == "Joker_Color" || name == "Joker_Monochrome") continue;

            int number = int.Parse(name.Substring(name.Length - 2));
            GameObject temp = Instantiate(card_prefab);
            temp.TryGetComponent<Card>(out Card card);
            card.GenerateCard(sprite,number);
            cards.Push(card);
		}
    }

    private void playerAddCard()
	{
        player_deck.addCard(cards.Pop());
	}

    private void dealerAddCard()
	{
        dealer_deck.addCard(cards.Pop());
    }

    private void dealEnd()
	{
        //dealer_deck.finishedDeal();
        //player_deck.finishedDeal();
    }

    private void checkWinStatus()
	{
        WIN_STATUS player_win_status = player_deck.win_status;

		if( player_win_status== WIN_STATUS.BLACKJACK ||player_win_status == WIN_STATUS.BUST)
        {
            dealEnd();
        }
	}

	public void startDeal()
	{
        playerAddCard();
        dealerAddCard();

        playerAddCard();
        dealerAddCard();

        checkWinStatus();
    }

    public void playerHit()
    {

    }

    public void playerStay()
    {

    }
}
