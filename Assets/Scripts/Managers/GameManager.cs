using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : DeckManager
{
    [SerializeField]private Transform cardparent;
    void Start()
    {
        Constants.offsiteCardsParent = cardparent;
        initializeDeck();
    }
}
