using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAccountSection : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI playerLevel;
    [SerializeField]
    private TMP_InputField playerNameInput;

    private PlayerInfoManager infoManager;

    private void Start()
    {
        infoManager = GameObject.Find("PlayerInfo").GetComponent<PlayerInfoManager>();
        playerName.text = infoManager.playerName;
        playerLevel.text = "Level: " + infoManager.playerLevel.ToString();
    }

    public void OnChangePlayerName()
    {
        infoManager.playerName = playerNameInput.text;
        playerName.text = infoManager.playerName;
        infoManager.SaveUpdatePlayerInfo();
    }
}
