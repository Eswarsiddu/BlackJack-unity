using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    const string IMAGESPATH = "PlayingCards";
    [SerializeField] private GameObject cardprefab;
    
    public void generateDeck(Stack<Card> cards)
    {
        Sprite[] cardsprites = Resources.LoadAll<Sprite>(IMAGESPATH);
        foreach(Sprite sprite in cardsprites)
		{
            string name = sprite.name;

            if(name == "Joker_Color" || name == "Joker_Monochrome") continue;

            int number = int.Parse(name.Substring(name.Length - 2));
            GameObject temp = Instantiate(cardprefab);
            temp.TryGetComponent<Card>(out Card card);
            card.GenerateCard(sprite,number);
            cards.Push(card);
		}
    }
}
