using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour
{
    public string playerName;
    public int playerLevel;
    public int playerTotalXP;
    public int[] deck = new int[30];
    public List<int> unlockedCards = new List<int>();


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
