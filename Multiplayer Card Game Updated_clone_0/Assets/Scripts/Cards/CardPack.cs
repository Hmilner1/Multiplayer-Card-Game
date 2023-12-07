using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPack : MonoBehaviour
{
    #region Events
    public delegate void CardClicked(GameObject cardObject);
    public static event CardClicked OnCardClicked;
    #endregion

    public void OnClickCard()
    {
        OnCardClicked?.Invoke(this.gameObject);
    }
}
