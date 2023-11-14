using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDispenser : MonoBehaviour
{
    [SerializeField]
    private GameObject playerHand;
    [SerializeField]
    private GameObject Card;
    private int handSize;

    public void OnClickDrawCard(int CardsToAdd)
    {
        handSize = 0;

        foreach (var card in GameObject.FindGameObjectsWithTag("Card"))
        {
            handSize++;
        }

        if (handSize < 7)
        {
            Instantiate(Card, playerHand.transform);
        }
    }
}
