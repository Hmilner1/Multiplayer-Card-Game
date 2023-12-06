using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalogueCard : MonoBehaviour
{
    public CardSo currentSetCard;
    private DeckManager deckManager;

    private void Start()
    {
        deckManager = GameObject.Find("Deck List").GetComponent<DeckManager>();
    }

    public void OnClickAddCard()
    { 
        deckManager.AddCard(currentSetCard);
    }
}
