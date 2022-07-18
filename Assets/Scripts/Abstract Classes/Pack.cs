using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pack
{
	protected int total1;
	protected int total2;
	protected int final_value;
	protected List<Card> cards;

	private bool finished_deal;


    public abstract void playerStayed();

    protected abstract void checkWinStatus();

    public void finishedDeal()
    {
        finished_deal = true;
    }

    public int getFinal_value()
    {
        return final_value;
    }

    public void addCard(Card card)
    {
        cards.Add(card);
        calculateTotal();
    }

    protected void calculateTotal()
    {
        int no_of_A = 0;
        int total = 0;
        foreach (Card card in cards)
        {
            if (!card.isFaceDown())
            {
                total += card.value;
                if (card.value == 1) no_of_A += 1;
            }
        }
        total1 = total;
        total2 = total + ((no_of_A > 0) ? 10 : 0);
        checkWinStatus();
    }

    public void resetDeck(List<Card> finished_deck)
    {
        total1 = 0;
        total2 = 0;
        finished_deal = false;
        finished_deck.AddRange(cards);
        cards.Clear();
    }
}
