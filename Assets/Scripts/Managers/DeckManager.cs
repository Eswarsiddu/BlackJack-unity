using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public abstract class DeckManager : MonoBehaviour
{
    private int betamount;
    private Stack<Card> cards;
    private List<Card> finished_cards;
    private bool deal_end;
    
    [SerializeField] private PlayerDeck player_deck;
    [SerializeField] private DealerDeck dealer_deck;

    [SerializeField] private GameObject hit_button;
    [SerializeField] private GameObject stay_button;

    const string IMAGESPATH = "PlayingCards";
    [SerializeField] private GameObject card_prefab;
    [SerializeField] private TextMeshProUGUI playerwintext;

    protected void initializeDeck()
	{
        cards = new Stack<Card>();
        deal_end = false;
        finished_cards = new List<Card>();
        player_deck.initializePack();
        dealer_deck.initializePack();
        playerwintext.text = "";
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
        cards.Shuffle();
        printStack();
    }

    protected void resetDeck()
	{
        player_deck.resetDeck(finished_cards);
        dealer_deck.resetDeck(finished_cards);
        if (cards.Count <= 15)
		{
            foreach(Card finished_card in finished_cards){
                cards.Push(finished_card);
			}
            finished_cards.Clear();
            // TODO: play animation
		}
        startDeal(); // TODO: change this triiger to bet placement

    }

    private void playerAddCard()
    {
        player_deck.addCard(cards.Pop());
        printStack();
    }

    private void dealerAddCard()
	{
        dealer_deck.addCard(cards.Pop());
        printStack();
    }

    private void playDealer()
	{
        dealer_deck.playerStayed();
        player_deck.playerStayed();
        while(dealer_deck.final_value < 17)
		{
            dealerAddCard();
		}
        if(dealer_deck.final_value > 21 || player_deck.final_value > dealer_deck.final_value)
		{
            player_deck.win_status = WIN_STATUS.WIN;
		}
        else if(player_deck.final_value == dealer_deck.final_value)
		{
            player_deck.win_status = WIN_STATUS.PUSH;
		}
		else
		{
            player_deck.win_status = WIN_STATUS.LOSE;
		}
        dealEnd();
	}

    private void dealEnd()
	{
        // TODO: dealer_deck.finishedDeal();
        // TODO: player_deck.finishedDeal();
        playerwintext.text = player_deck.win_status.ToString();
		switch (player_deck.win_status)
		{
			case WIN_STATUS.WIN:
            case WIN_STATUS.BLACKJACK:
                doublebetMoney();
                takeBetMoney();
                break;
            case WIN_STATUS.PUSH:
                takeBetMoney();
                break;
            case WIN_STATUS.BUST:
            case WIN_STATUS.LOSE:
                removeBetMoney();
                break;
		}
	}

	private void checkInitialWinStatus()
	{
        WIN_STATUS player_win_status = player_deck.win_status;

		if( player_win_status== WIN_STATUS.BLACKJACK ||player_win_status == WIN_STATUS.BUST)
        {
            dealEnd();
        }
	}

    private void checkPostWinStatus()
	{
        WIN_STATUS player_win_status = player_deck.win_status;

        if(player_win_status == WIN_STATUS.BUST)
		{
            dealEnd();
            return;
		}

        if(player_win_status == WIN_STATUS.BLACKJACK)
		{
            playDealer();
		}

    }

	public void startDeal()
	{
        playerwintext.text = "";

        playerAddCard();
        dealerAddCard();

        playerAddCard();
        dealerAddCard();

        checkInitialWinStatus();
    }

    public void playerHit()
    {
        playerAddCard();
        checkPostWinStatus();
    }

    public void playerStay()
    {
        playDealer();
    }


	#region Bet Amount

	private void removeBetMoney()
    {
        betamount = 0;
    }

    private void takeBetMoney()
    {
        // TODO: Add betmoney to player coins
    }

    private void doublebetMoney()
    {
        betamount = betamount * 2;
    }

	#endregion

	#region Test

    private void printStack()
	{
        string s = "";
        foreach(Card card in cards)
		{
            s += card.ToString()+",";
		}
        Debug.Log(s);
	}

	#endregion
}
