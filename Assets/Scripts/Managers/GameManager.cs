using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : DeckManager
{
    [SerializeField]private Transform cardparent;
    void Awake()
    {
        Constants.offsiteCardsParent = cardparent;
        initializeDeck();
    }
}
