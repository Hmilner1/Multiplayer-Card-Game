using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDispenser : MonoBehaviour
{
    [SerializeField]
    private GameObject playerHand;
    [SerializeField]
    private GameObject Card;

    public void OnClickDrawCard()
    {
        Instantiate(Card, playerHand.transform);
    }
}
