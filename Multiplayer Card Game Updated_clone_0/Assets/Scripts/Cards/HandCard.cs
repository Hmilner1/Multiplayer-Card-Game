using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCard : MonoBehaviour
{
    public CardSo currentCard;
    public int stamina { get; private set; }
    [SerializeField]
    private Image cardImage;

    private void Start()
    {
        cardImage.sprite = currentCard.cardImage;
        DetermineStam();
    }

    private void DetermineStam()
    { 
        switch (currentCard.cardWeight) 
        { 
            case CardSo.Weight.Light:
                stamina = 1;
                break;
            case CardSo.Weight.Medium:
                stamina = 2;
                break;
            case CardSo.Weight.Heavy:
                stamina = 3;
                break;
        }
    }
}
