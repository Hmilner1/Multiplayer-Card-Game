using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour
{
    public string playerName;
    public int playerLevel;
    public int playerTotalXP;
    public int[] deck = new int[30];
    public List<int> unlockedCards;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayerInfo saveInfo = SaveManager.LoadPlayerInfo();
    }

}
