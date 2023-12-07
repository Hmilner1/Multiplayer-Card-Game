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

    private void OnEnable()
    {
        MenuGroup.OnNewTab += RePopulateList;
    }

    private void OnDisable()
    {
        MenuGroup.OnNewTab -= RePopulateList;
    }

    private void Start()
    {
        playerInfoManager = GameObject.Find("PlayerInfo").GetComponent<PlayerInfoManager>();
    }

    private void PopulateList()
    {
        playerInfoManager = GameObject.Find("PlayerInfo").GetComponent<PlayerInfoManager>();

        for (int i = 0; i < playerInfoManager.unlockedCards.Count; i++)
        {
            var spawnCard = Instantiate(catalogueCardPrefab, catalogueHolder.transform);
            var cardToDisplay = playerInfoManager.unlockedCards[i];
            spawnCard.GetComponent<CatalogueCard>().currentSetCard = cardDataBase.cardDatabase[cardToDisplay];
            spawnCard.GetComponent<Image>().sprite = cardDataBase.cardDatabase[cardToDisplay].cardImage;
        }
    }

    private void RePopulateList()
    {
        foreach (Transform children in catalogueHolder.transform)
        { 
            Destroy(children.gameObject);
        }
        PopulateList();
    }
}
