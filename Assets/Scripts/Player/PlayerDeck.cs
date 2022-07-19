using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : Pack
{
	public override void addCard(Card card)
	{
		card.faceUp();
		base.addCard(card);
	}

}
