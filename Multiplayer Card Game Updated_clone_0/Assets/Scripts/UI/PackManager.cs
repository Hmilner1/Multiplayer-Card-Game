using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class PackManager : MonoBehaviour
{
    private GameObject selectedPack;
    private PlayerInfoManager playerInfoManager;
    [SerializeField]
    private GameObject openPackOverlay;
    [SerializeField] 
    private GameObject packOverlayHolder;
    [SerializeField]
    private CardDataBase cardData;
    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private GameObject packHolder;
    [SerializeField]
    private GameObject packPrefab;

    private void OnEnable()
    {
        CardPack.OnCardClicked += OnClickSelectObject;
        MenuGroup.OnNewTab += ReloadUI;
    }

    private void OnDisable()
    {
        CardPack.OnCardClicked -= OnClickSelectObject;
        MenuGroup.OnNewTab -= ReloadUI;
    }
    private void Start()
    {
        playerInfoManager = GameObject.Find("PlayerInfo").GetComponent<PlayerInfoManager>();
    }

    private void OnClickSelectObject(GameObject selectedObject)
    {
        selectedPack = selectedObject;
    }

    public void OnClickOpen()
    {
        if (selectedPack == null) return;

        Destroy(selectedPack);
        playerInfoManager.packCount = playerInfoManager.packCount -1;
        playerInfoManager.SaveUpdatePlayerInfo();
        SetPackOverlay();
    }

    private void SetPackOverlay()
    {
        LeanTween.scale(openPackOverlay, new Vector3(0, 0, 0), 0f).setEase(LeanTweenType.easeInElastic);
        openPackOverlay.SetActive(true);
        LeanTween.scale(openPackOverlay, new Vector3(1, 1, 1), 0.2f).setEase(LeanTweenType.easeInElastic);


        for (int i = 0; i < 3; i++)
        {
            int cardID = Random.Range(0, cardData.cardDatabase.Count);
            var spawnedCard = Instantiate(cardPrefab, packOverlayHolder.transform);
            CatalogueCard cardInfo = spawnedCard.GetComponent<CatalogueCard>();
            Button cardButton = spawnedCard.GetComponent<Button>(); 
            cardInfo.enabled= false;
            cardButton.enabled= false;
            Image cardImage= spawnedCard.GetComponent<Image>();
            cardImage.sprite = cardData.cardDatabase[cardID].cardImage;
            CheckPlayerCards(cardID);
        }
        playerInfoManager.SaveUpdatePlayerInfo();
    }

    private void CheckPlayerCards(int cardID)
    {
        bool cardUnlocked = false;
        foreach (var Card in playerInfoManager.unlockedCards)
        {
            if (Card == cardID)
            { 
                cardUnlocked = true;
            }
        }

        if (!cardUnlocked)
        {
            playerInfoManager.unlockedCards.Add(cardID);
        }
    }

    public void DestroyCards()
    {
        foreach (Transform children in GameObject.Find("Cards Received").transform)
        { 
            Destroy(children.gameObject);
        }
    }

    private void ReloadUI()
    {
        foreach (Transform children in packHolder.transform)
        {
            Destroy(children.gameObject);
        }
        PopulateList();
    }

    private void PopulateList()
    { 
        playerInfoManager = GameObject.Find("PlayerInfo").GetComponent<PlayerInfoManager>();

        for (int i = 0; i < playerInfoManager.packCount; i++)
        {
            Instantiate(packPrefab, packHolder.transform);
        }
    }
}
