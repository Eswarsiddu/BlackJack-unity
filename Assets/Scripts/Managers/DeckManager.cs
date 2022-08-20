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
    [SerializeField] private float seconds;

    public void setFalse(ref bool value)
	{
        value = false;
	}

    public void SetBetAmount(int bet_amount)
	{
        this.bet_amount = bet_amount;
        playerdata.decreaseCoins(bet_amount);
    }

    private void AddCard(AnimationStateReference animation_completed,DealerDeck deck)
	{
        SoundManager.PlayCardMovingSound();
        deck.AddCard(cards.Pop(), animation_completed);
        printStack();
    }

    private void playerAddCard(AnimationStateReference animation_completed)
    {
        SoundManager.PlayCardMovingSound();
        player_deck.AddCard(cards.Pop(),animation_completed);
        printStack();
    }

    private void dealerAddCard(AnimationStateReference animation_completed)
	{
        SoundManager.PlayCardMovingSound();
        dealer_deck.AddCard(cards.Pop(), animation_completed);
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

    private AnimationStateReference add_card_animation_completed = new AnimationStateReference(true);

    public void playerHit() // UI Button
    {
        if (add_card_animation_completed.value && !add_card_animation_completed.running)
        {
            AddCard(add_card_animation_completed,player_deck);
            checkPostWinStatus();
        }
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

        while (dealer_deck.final_value < 17)
        {
            if (add_card_animation_completed.value)
            {
                AddCard(add_card_animation_completed, dealer_deck);
            }
        }

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
        //        yield return new WaitForSeconds(Constants.WAITING_TIME);
        yield return new WaitForSeconds(seconds);
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

        if (cards == null || finished_cards == null) return;
        if (cards.Count <= 0) return;
        playerwintext.text = "";
        bet_amount = 0;

        player_deck.ResetDeck(finished_cards);
        dealer_deck.ResetDeck(finished_cards);

        if (cards.Count <= 15)
        {
            foreach (Card finished_card in finished_cards) cards.Push(finished_card);
            finished_cards.Clear();
            SoundManager.PlayShuffleSound();
            cards.Shuffle();
            // TODO: play animation
        }

        //if (nextDeal == null) return;
        nextDeal?.Invoke();
        //nextDeal();
    }

    public void startDeal()
    {
        //bool c1 = false;

        c1.Reset();
        c2.Reset();
        c3.Reset();
        c4.Reset();
        startdeal = true;

        /* while (!(c1.value && c2.value && c3.value && c4.value))
         {
             if (!c1.value && !c1.running)
                 AddCard(c1, player_deck);
             else if (!c2.value && !c2.running)
                 AddCard(c2, dealer_deck);
             else if (!c3.value && !c3.running)
                 AddCard(c3, player_deck);
             else if (!c4.value && !c4.running)
                 AddCard(c4, dealer_deck);
         }

         checkInitialWinStatus();*/
    }

    private AnimationStateReference c1 = new AnimationStateReference();
    private AnimationStateReference c2 = new AnimationStateReference();
    private AnimationStateReference c3 = new AnimationStateReference();
    private AnimationStateReference c4 = new AnimationStateReference();

    private bool startdeal = false;

    private void Update()
	{
		if (startdeal)
		{
            Debug.Log("Deal started");
            if(!c1.running && !c1.value)
			{
                AddCard(c1, player_deck);
			}
			else if(!c1.running)
			{
                if (!c2.running && !c2.value)
				{
                    AddCard(c2, dealer_deck);
				}
                else if (!c2.running)
				{
                    if (!c3.running && !c3.value)
					{
                        AddCard(c3, player_deck);
                    }
                    else if (!c3.running)
					{
                        if (!c4.running && !c4.value)
						{
                            AddCard(c4, dealer_deck);
                        }
                        else if (!c4.running)
						{
                            startdeal = false;
                            checkInitialWinStatus();
                        }

                    }

                }

            }



            /*if (!c1.value && !c1.running)
                AddCard(c1, player_deck);
            else if (!c2.value && !c2.running)
                AddCard(c2, dealer_deck);
            else if (!c3.value && !c3.running)
                AddCard(c3, player_deck);
            else if (!c4.value && !c4.running)
            {
                AddCard(c4, dealer_deck);
                checkInitialWinStatus();
                startdeal = false;
            }*/
        }
        VirtualUpdate();
	}

    protected virtual void VirtualUpdate() { }

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
