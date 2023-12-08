using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDispenser : MonoBehaviour
{
    [SerializeField]
    private GameObject playerHand;
    [SerializeField]
    private GameObject Card;
    [SerializeField]
    private CardDataBase cardData;
    [SerializeField]
    private List<int> deckCards = new List<int>();
    private int handSize;
    private PlayerInfoManager playerInfoManager;

    private void Start()
    {
        playerInfoManager = GameObject.FindGameObjectWithTag("Info").GetComponent<PlayerInfoManager>();
        LoadDeck();
    }

    private void LoadDeck()
    {
        foreach (int cardID in playerInfoManager.deck)
        { 
            deckCards.Add(cardID);
        }
        Shuffle(deckCards);
    }

    public void OnClickDrawCard(int CardsToAdd)
    {
        if (deckCards.Count > 0)
        {
            handSize = 0;

            foreach (var card in GameObject.FindGameObjectsWithTag("Card"))
            {
                handSize++;
            }

            for (int i = 0; i < CardsToAdd; i++)
            {
                if (handSize < 7)
                {
                    var cardSpawned = Instantiate(Card, playerHand.transform);
                    HandCard card = cardSpawned.GetComponent<HandCard>();

                    card.currentCard = cardData.cardDatabase[deckCards[0]];
                    deckCards.RemoveAt(0);
                }
            }
        }
    }

    public void Shuffle(List<int> Deck)
    {
        List<int> tempDeck = new List<int>();
        tempDeck = Deck;

        for (int i = 0; i < Deck.Count; i++)
        {
            int index = Random.Range(0, tempDeck.Count);
            deckCards.Add(tempDeck[index]);
            tempDeck.RemoveAt(index);
        }
    }
}
