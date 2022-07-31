using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class DeckManager : MonoBehaviour
{
    private int bet_amount;
    private Stack<Card> cards;
    private List<Card> finished_cards;
    public Action nextDeal;
    public Action DisableDealOptions;
    private PlayerData playerdata;

    [SerializeField] private PlayerDeck player_deck;
    [SerializeField] private DealerDeck dealer_deck;

    [SerializeField] private TextMeshProUGUI playerwintext;


    public void SetBetAmount(int bet_amount)
	{
        this.bet_amount = bet_amount;
        playerdata.decreaseCoins(bet_amount);
    }

    public void startDeal()
    {
        playerAddCard();
        dealerAddCard();

        playerAddCard();
        dealerAddCard();

        checkInitialWinStatus();
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

    private void checkInitialWinStatus()
    {
        WIN_STATUS player_win_status = player_deck.win_status;

        if (player_win_status == WIN_STATUS.BLACKJACK || player_win_status == WIN_STATUS.BUST)
        {
            dealEnd();
        }
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

    private void playDealer()
	{
        player_deck.Stayed();
        dealer_deck.PlayerStayed();

        while(dealer_deck.final_value < 17) dealerAddCard();

        if(dealer_deck.final_value > 21 || player_deck.final_value > dealer_deck.final_value)
            player_deck.win_status = WIN_STATUS.WIN;
        else if(player_deck.final_value == dealer_deck.final_value)
            player_deck.win_status = WIN_STATUS.PUSH;
		else
            player_deck.win_status = WIN_STATUS.LOSE;

        dealEnd();
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
            playDealer();
    }

    private void dealEnd()
	{
        DisableDealOptions();
        WIN_STATUS win_status = player_deck.win_status;

        playerwintext.text = Enum.GetName(typeof(WIN_STATUS),player_deck.win_status);
        SoundManager.PlayWinText(win_status);

        if (win_status == WIN_STATUS.WIN || win_status == WIN_STATUS.BLACKJACK)
            playerdata.increaseCoins(bet_amount * Constants.DOUBLE);

        StartCoroutine(DealEndEnumerator());
	}

    private IEnumerator DealEndEnumerator()
    {
        yield return new WaitForSeconds(Constants.WAITING_TIME);
        resetDeck();
    }

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
        foreach (Sprite sprite in cardsprites)
        {
            string name = sprite.name;

            if (name == "Joker_Color" || name == "Joker_Monochrome") continue;

            int number = int.Parse(name.Substring(name.Length - 2));
            GameObject temp = Instantiate(card_prefab);
            temp.TryGetComponent<Card>(out Card card);
            card.GenerateCard(sprite, number);
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
        bet_amount = 0;
        if (cards.Count <= 15)
        {
            foreach (Card finished_card in finished_cards)
            {
                cards.Push(finished_card);
            }
            finished_cards.Clear();
            SoundManager.PlayShuffleSound();
            cards.Shuffle();
            // TODO: play animation
        }
        nextDeal();
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
