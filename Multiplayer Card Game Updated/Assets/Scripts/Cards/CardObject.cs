using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    [SerializeField]
    private CardSo currentCard;
    [SerializeField]
    private SpriteRenderer cardImage;

    private void Awake()
    {
        cardImage.sprite = currentCard.cardImage;
    }
}
