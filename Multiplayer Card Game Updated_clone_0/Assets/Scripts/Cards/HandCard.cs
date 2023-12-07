using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCard : MonoBehaviour
{
    public CardSo currentCard;
    [SerializeField]
    private Image cardImage;

    private void Start()
    {
        cardImage.sprite = currentCard.cardImage;
    }
}
