using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CoinTossPanelUI;

public class CoinTossPanelUI : MonoBehaviour
{
    #region Events
    public delegate void CoinSelected(string Colour);
    public static event CoinSelected OnCoinSelected;
    #endregion

    [SerializeField]
    private TMP_Text clientSelectedColour;

    public void OnClickCoinToss(string ButtonColour)
    {
        OnCoinSelected?.Invoke(ButtonColour);

        Button[] colourButton = this.GetComponentsInChildren<Button>();
        foreach (var button in colourButton)
        {
            button.interactable = false;
        }
    }

    public void OnColourSetText(string coinColour)
    {
        if (clientSelectedColour != null)
        { 
            clientSelectedColour.text = "Player Chose " + coinColour;
        }
    }
}
