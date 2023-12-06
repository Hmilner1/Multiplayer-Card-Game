using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    private PlayerInfoManager playerInfoManager;
    [SerializeField]
    private GameObject deckCardPrefab;
    [SerializeField]
    private GameObject deckHolder;
    [SerializeField]
    private CardDataBase cardDataBase;
    [SerializeField]
    private Button saveButton;
    private List<GameObject> deckList;
    public int[] tempDeck = new int[30];

    private void Start()
    {
        deckList = new List<GameObject>();
        playerInfoManager = GameObject.Find("PlayerInfo").GetComponent<PlayerInfoManager>();

        for (int i = 0; i < playerInfoManager.deck.Length; i++)
        { 
            var spawnCard = Instantiate(deckCardPrefab, deckHolder.transform);
            var cardToDisplay = playerInfoManager.deck[i];
            spawnCard.GetComponent<DeckCard>().currentSetCard = cardDataBase.cardDatabase[cardToDisplay]; 
            spawnCard.GetComponentInChildren<TextMeshProUGUI>().text = cardDataBase.cardDatabase[cardToDisplay].cardName;

            deckList.Add(spawnCard);
        }
    }

    private void Update()
    {
        if (deckList.Count == 30)
        {
            saveButton.interactable = true;
        }
        else if(deckList.Count < 30 || deckList.Count > 30 )
        { 
            saveButton.interactable = false;
        }
    }

    public void OnClickSave()
    {
        for (int i = 0; i < tempDeck.Length; i++)
        {
            tempDeck[i] = deckList[i].GetComponent<DeckCard>().currentSetCard.cardID;
        }
        playerInfoManager.deck = tempDeck;
        playerInfoManager.SaveUpdatePlayerInfo();
    }

    public void OnClickRemove()
    {
        Destroy(deckList[deckList.Count - 1].gameObject);
        deckList.RemoveAt(deckList.Count - 1);
    }

    public void AddCard(CardSo cardToAdd)
    {
        var spawnCard = Instantiate(deckCardPrefab, deckHolder.transform);
        spawnCard.GetComponent<DeckCard>().currentSetCard = cardToAdd;
        spawnCard.GetComponentInChildren<TextMeshProUGUI>().text = cardToAdd.cardName;
        deckList.Add(spawnCard);
    }
}
