using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/CardDatabase")]
public class CardDataBase : ScriptableObject
{
    [SerializeField]
    public List<CardSo> cardDatabase;

    [ContextMenu("Populate ID")]
    public void PopulateDatabase()
    { 
        cardDatabase = new List<CardSo>();

        var allCards = Resources.LoadAll<CardSo>("Cards");
        foreach (var card in allCards) 
        { 
            cardDatabase.Add(card);
            card.cardID = cardDatabase.Count -1;
        }
    }


}
