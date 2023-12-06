using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCard : MonoBehaviour
{
    [SerializeField]
    private CardSo currentCard;
    [SerializeField]
    private Image cardImage;

    private void Awake()
    {
        cardImage.sprite = currentCard.cardImage;
    }

}
