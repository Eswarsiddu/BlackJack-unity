using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class DeckManager : MonoBehaviour
{
	private int bet_amount;
	private int delaer_player_turn = 0;

	private bool dealer_play;
	private bool player_hit;
	private bool startdeal;
	private bool dealended = true;

	private Stack<Card> cards;
	private List<Card> finished_cards;

	public Action nextDeal;
	public Action DisableDealOptions;
	public Action GameScreenDealEnd;
	public Action EnableDealOptions;

	private PlayerData playerdata;

	private AnimationStateReference add_card_animation_completed;

	[SerializeField] private PlayerDeck player_deck;
	[SerializeField] private DealerDeck dealer_deck;

	[SerializeField] private TextMeshProUGUI playerwintext;

	public void SetBetAmount(int bet_amount)
	{
		this.bet_amount = bet_amount;
		playerdata.decreaseCoins(bet_amount);
	}

	private void AddCard(DealerDeck deck)
	{
		SoundManager.PlayCardMovingSound();
		deck.AddCard(cards.Pop(), add_card_animation_completed);
		printStack();
	}

	/*private void checkInitialWinStatus()
	{
		if (player_deck.win_status == WIN_STATUS.BLACKJACK)
			dealEnd();
	}*/

	public void playerHit() // UI Button
	{
		if (dealended == false && player_hit == false)
		{
			add_card_animation_completed.Reset();
			player_hit = true;
		}
	}

	public void playerStay() // UI Button
	{
		SoundManager.PlayUIElementClickSound();
		DisableDealOptions();
		playDealer();
	}

	private void playDealer()
	{
		player_deck.Stayed();
		dealer_deck.PlayerStayed();
		add_card_animation_completed.Reset();
		dealer_play = true;
	}

	private void DealerCompleted()
	{
		if (dealer_deck.final_value > 21 || player_deck.final_value > dealer_deck.final_value)
			player_deck.win_status = WIN_STATUS.WIN;
		else if (player_deck.final_value == dealer_deck.final_value)
			player_deck.win_status = WIN_STATUS.PUSH;
		else
			player_deck.win_status = WIN_STATUS.LOSE;

		dealEnd();
	}

	private void checkPostWinStatus()
	{
		WIN_STATUS player_win_status = player_deck.win_status;

		if (player_win_status == WIN_STATUS.BUST)
			dealEnd();

		if (player_win_status == WIN_STATUS.BLACKJACK)
			playDealer();
	}

	private void dealEnd()
	{
		dealended = true;
		GameScreenDealEnd();
		WIN_STATUS win_status = player_deck.win_status;

		playerwintext.text = Enum.GetName(typeof(WIN_STATUS), player_deck.win_status);
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
		add_card_animation_completed = new AnimationStateReference();
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
		}

		nextDeal?.Invoke();
	}

	public void startDeal()
	{
		startdeal = true;
		dealended = false;
		add_card_animation_completed.Reset();
		delaer_player_turn = 0;
	}


	private void Update()
	{
		if (startdeal)
		{
			if (!add_card_animation_completed.running && !add_card_animation_completed.value)
				AddCard((delaer_player_turn == 0 || delaer_player_turn == 2) ? player_deck : dealer_deck);
			else if (!add_card_animation_completed.running)
			{
				add_card_animation_completed.Reset();
				if (delaer_player_turn == 3)
				{
					startdeal = false;
					EnableDealOptions();
					//checkInitialWinStatus();
					if (player_deck.win_status == WIN_STATUS.BLACKJACK)
						dealEnd();
				}
				delaer_player_turn++;
			}
		}
		else
		{
			if (player_hit)
			{
				if (!add_card_animation_completed.running && !add_card_animation_completed.value) AddCard(player_deck);
				else if (!add_card_animation_completed.running)
				{
					checkPostWinStatus();
					player_hit = false;
				}
			}

			if (dealer_play)
			{
				if (dealer_deck.final_value < 17)
				{
					if (!add_card_animation_completed.running && !add_card_animation_completed.value)
						AddCard(dealer_deck);
					else if (!add_card_animation_completed.running)
						add_card_animation_completed.Reset();
				}
				else
				{
					dealer_play = false;
					DealerCompleted();
				}
			}
		}

		VirtualUpdate();
	}

	protected abstract void VirtualUpdate();

	#region Test

	[SerializeField] private float seconds;

	private void printStack()
	{
		string s = "";
		foreach (Card card in cards)
		{
			s += card.ToString() + ",";
		}
		Debug.Log(s);
	}

	#endregion
}
