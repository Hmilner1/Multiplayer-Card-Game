using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Cards")]
public class CardSo : ScriptableObject
{
    public int cardID;
    public string cardName;
    public int cardDamage;
    public Weight cardWeight;

    public Sprite cardImage;
    public enum Weight
    { 
        Light,
        Medium,
        Heavy
    }
}
