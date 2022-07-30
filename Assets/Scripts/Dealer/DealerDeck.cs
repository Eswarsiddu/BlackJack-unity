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

	public void PlayerStayed()
	{
		cards[0].TurnFaceUp();
	}

	protected override void UpdateScore()
	{
		SetFinalScore();
	}
}
