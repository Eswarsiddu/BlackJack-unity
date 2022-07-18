using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Generator generator;

    void Start()
    {
        generator = GetComponent<Generator>();
        generateDeck();
    }

    #region DeckManager

    private Stack<Card> cards;

    private void generateDeck()
	{
        generator.generateDeck(cards);
    }

    public void playerHit()
	{

	}

    public void playerStay()
	{

	}


	#endregion


	#region GameManager



	#endregion
}
