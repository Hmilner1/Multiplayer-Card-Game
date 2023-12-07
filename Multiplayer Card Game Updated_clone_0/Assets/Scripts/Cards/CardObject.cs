using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    public CardSo currentCard;
    [SerializeField]
    private SpriteRenderer cardImage;

    private void Start()
    {
        cardImage.sprite = currentCard.cardImage;
    }
}
