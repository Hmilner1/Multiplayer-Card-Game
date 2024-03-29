using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public string playerName;
    public int playerLevel;
    public int playerTotalXP;
    public int playerRank;
    public int packCount;
    public int[] deck = new int[30];
    public List<int> unlockedCards = new List<int>();

    public PlayerInfo(PlayerInfoManager saveInfo)
    {
        playerName = saveInfo.playerName;
        playerLevel = saveInfo.playerLevel;
        playerTotalXP = saveInfo.playerTotalXP;
        playerRank = saveInfo.playerRank;
        packCount = saveInfo.packCount;
        deck = saveInfo.deck;
        unlockedCards = saveInfo.unlockedCards;
    }

}
