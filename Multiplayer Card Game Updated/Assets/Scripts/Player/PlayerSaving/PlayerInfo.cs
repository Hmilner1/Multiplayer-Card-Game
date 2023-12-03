using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public string playerName;
    public int playerLevel;
    public int playerTotalXP;
    public int[] deck = new int[30];
    public List<int> unlockedCards;

    public PlayerInfo(PlayerInfoManager saveInfo)
    {
        string playerName = saveInfo.playerName;
        int playerLevel = saveInfo.playerLevel;
        int playerTotalXP = saveInfo.playerTotalXP;
        int[] deck = saveInfo.deck;
        List<int> unlockedCards = saveInfo.unlockedCards;
    }

}
