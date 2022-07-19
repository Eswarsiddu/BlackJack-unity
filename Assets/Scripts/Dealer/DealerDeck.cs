using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerDeck : Pack
{

	private bool first_card;

	public override void initializePack()
	{
		base.initializePack();
		first_card = true;
	}

	public override void resetDeck(List<Card> finished_deck)
	{
		first_card = true;
		base.resetDeck(finished_deck);
	}

	public override void addCard(Card card)
	{
		if (first_card)
		{
			card.faceDown();
			first_card = false;
		}
		else
		{
			card.faceUp();
		}

		base.addCard(card);
	}

}
