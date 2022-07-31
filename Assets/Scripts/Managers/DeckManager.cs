using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class DeckManager : MonoBehaviour
{
    private int _betamount;
    private Stack<Card> cards;
    private List<Card> finished_cards;
    public Action nextDeal;
    public Action DisableDealOptions;
    private PlayerData playerdata;

	public int betamount { 
        set 
        { 
            _betamount = value;
            playerdata.decreaseCoins(value);
        } 
    }

    [SerializeField] private PlayerDeck player_deck;
    [SerializeField] private DealerDeck dealer_deck;

    [SerializeField] private GameObject hit_button;
    [SerializeField] private GameObject stay_button;

    [SerializeField] private TextMeshProUGUI playerwintext;

    protected void initializeDeck()
	{
        playerdata = Resources.Load<PlayerData>(Constants.PLAYER_DATA_PATH);
        cards = new Stack<Card>();
        finished_cards = new List<Card>();
        player_deck.InitializeDeckk();
        dealer_deck.InitializeDeckk();
        playerwintext.text = "";
        generateDeck();
	}
    
    private void generateDeck()
    {
        GameObject card_prefab = Resources.Load<GameObject>(Constants.CARD_PREFAB_PATH);
        Sprite[] cardsprites = Resources.LoadAll<Sprite>(Constants.CARD_FRONT_IMAGE_PATH);
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

    public void resetDeck()
	{
        playerwintext.text = "";
        player_deck.ResetDeck(finished_cards);
        dealer_deck.ResetDeck(finished_cards);
        betamount = 0;
        if (cards.Count <= 15)
		{
            foreach(Card finished_card in finished_cards){
                cards.Push(finished_card);
			}
            finished_cards.Clear();
            SoundManager.PlayShuffleSound();
            cards.Shuffle();
            // TODO: play animation
		}
        nextDeal();
    }

    private void playerAddCard()
    {
        SoundManager.PlayCardMovingSound();
        player_deck.AddCard(cards.Pop());
        printStack();
    }

    private void dealerAddCard()
	{
        SoundManager.PlayCardMovingSound();
        dealer_deck.AddCard(cards.Pop());
        printStack();
    }

    private void playDealer()
	{
        player_deck.Stayed();
        dealer_deck.PlayerStayed();
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
        DisableDealOptions();
        WIN_STATUS win_status = player_deck.win_status;
        playerwintext.text = Enum.GetName(typeof(WIN_STATUS),player_deck.win_status);
        if(win_status == WIN_STATUS.WIN || win_status == WIN_STATUS.BLACKJACK)
		{
            doublebetMoney();
        }
        else if (win_status == WIN_STATUS.BUST || win_status == WIN_STATUS.LOSE)
		{
            removeBetMoney();
		}
		
        takeBetMoney();
        SoundManager.PlayWinText(win_status);
        StartCoroutine(DealEndEnumerator());
	}

    private IEnumerator DealEndEnumerator()
	{
        yield return new WaitForSeconds(Constants.WAITING_TIME);
        resetDeck();
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

        playerAddCard();
        dealerAddCard();

        playerAddCard();
        dealerAddCard();

        checkInitialWinStatus();
    }

    public void playerHit() // UI Button
    {
        SoundManager.PlayUIElementClickSound();
        playerAddCard();
        checkPostWinStatus();
    }

    public void playerStay() // UI Button
    {
        SoundManager.PlayUIElementClickSound();
        playDealer();
    }


	private void removeBetMoney()
    {
        _betamount = 0;
    }

    private void takeBetMoney()
    {
        playerdata.increaseCoins(_betamount);
    }

    private void doublebetMoney()
    {
        _betamount = _betamount * 2;
    }


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
