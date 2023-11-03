using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDispenser : MonoBehaviour
{
    private GameObject playerHand;
    [SerializeField]
    private GameObject Card;
    private void OnEnable()
    {
        PlayerSetupManager.OnPlayerSetUp += GetPlayerHand;   
    }
    private void OnDisable()
    {
        PlayerSetupManager.OnPlayerSetUp -= GetPlayerHand;
    }

    private void GetPlayerHand()
    {
        playerHand = GameObject.Find("Hand Panel");
    }

    public void OnClickDrawCard()
    {
        Instantiate(Card, playerHand.transform);
    }
}
