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
    private int handSize;
    private PlayerInfoManager playerInfoManager;
    private List<int> deckCards = new List<int>();

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
    }

    public void OnClickDrawCard(int CardsToAdd)
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
                int cardInt = RandomCard();
                card.currentCard = cardData.cardDatabase[deckCards[cardInt]];
                deckCards.RemoveAt(cardInt);
            }
        }
    }

    private int RandomCard()
    {
        int CardToDraw = Random.Range(0, deckCards.Count);
        return CardToDraw;
    }
}
