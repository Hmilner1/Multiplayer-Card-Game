using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatalogueManager : MonoBehaviour
{
    private PlayerInfoManager playerInfoManager;
    [SerializeField]
    private GameObject catalogueCardPrefab;
    [SerializeField]
    private GameObject catalogueHolder;
    [SerializeField]
    private CardDataBase cardDataBase;

    private void Start()
    {
        playerInfoManager = GameObject.Find("PlayerInfo").GetComponent<PlayerInfoManager>();

        for (int i = 0; i < playerInfoManager.unlockedCards.Count; i++)
        {
            var spawnCard = Instantiate(catalogueCardPrefab,catalogueHolder.transform);
            var cardToDisplay = playerInfoManager.deck[i];
            spawnCard.GetComponent<CatalogueCard>().currentSetCard = cardDataBase.cardDatabase[i];
            spawnCard.GetComponent<Image>().sprite = cardDataBase.cardDatabase[i].cardImage;
        }
    }
}
