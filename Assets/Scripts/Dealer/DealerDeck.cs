public class DealerDeck : PlayerDeck
{
	protected override void ChangeCardState(Card card)
	{
		if (cards.Count == 0)
		{
			card.TurnFaceDown();
			return;
		}

		card.TurnFaceUp();
	}

	public override void playerStayed()
	{
		base.playerStayed();
		cards[0].TurnFaceUp();
	}
}
