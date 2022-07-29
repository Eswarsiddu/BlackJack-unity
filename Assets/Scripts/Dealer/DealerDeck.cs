using System.Collections.Generic;

public class DealerDeck : PlayerDeck
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

	protected override void ChangeCardState(Card card)
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
	}

	public override void playerStayed()
	{
		base.playerStayed();
		cards[0].faceUp();
	}
}
