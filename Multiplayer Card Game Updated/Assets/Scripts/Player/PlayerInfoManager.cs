using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour
{
    private PlayerInfoManager Instance;
    public string playerName;
    public int playerLevel;
    public int playerTotalXP;
    public int[] deck = new int[30];
    public List<int> unlockedCards = new List<int>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);

        Debug.Log(Application.persistentDataPath);

        PlayerInfo saveInfo = SaveManager.LoadPlayerInfo();
        if (saveInfo != null)
        {
            playerName = saveInfo.playerName;
            playerLevel = saveInfo.playerLevel;
            playerTotalXP = saveInfo.playerTotalXP;
            deck = saveInfo.deck;
            unlockedCards = saveInfo.unlockedCards;
        }
        else
        {
            playerName = "Enter A Name";
            playerLevel = 0;
            playerTotalXP = 0;
            deck = new int[30];
            for (int i = 0;i < deck.Length; i++) 
            {
                deck[i] = 0;
            }
            unlockedCards.Add(0);
            unlockedCards.Add(1);
            unlockedCards.Add(2);
            unlockedCards.Add(3);
            SaveManager.SavePlayerInfo(this);
        }
    }

    public void UpdateInfo()
    {
        PlayerInfo saveInfo = SaveManager.LoadPlayerInfo();
        if (saveInfo == null)
        {
            SaveManager.SavePlayerInfo(this);
        }
        else
        {
            playerName = saveInfo.playerName;
            playerLevel = saveInfo.playerLevel;
            playerTotalXP = saveInfo.playerTotalXP;
            deck = saveInfo.deck;

        }
    }

    public void SaveUpdatePlayerInfo()
    {
        SaveManager.SavePlayerInfo(this);
    }
}
