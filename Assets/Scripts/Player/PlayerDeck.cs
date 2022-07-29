public class PlayerDeck : Pack
{
	public override void addCard(Card card)
	{
		card.faceUp();
		base.addCard(card);
	}

}
