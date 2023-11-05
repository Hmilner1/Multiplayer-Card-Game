using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandUIController : MonoBehaviour
{
    #region Events
    public delegate void CardClicked();
    public static event CardClicked OnCardClicked;
    #endregion

    private PlayerStateManager playerStateManager;

    private void Start()
    {
        playerStateManager = GetComponentInParent<PlayerStateManager>();
    }

    public void OnMouseOverHand()
    { 
        GameObject HandPanel = this.gameObject;
        if (playerStateManager.currentState == PlayerStateManager.playerState.selecting)
        {
            HandPanel.transform.position = HandPanel.transform.position + new Vector3(0, 100, 0);
        }
    }

    public void OnMouseExitHand()
    {
        GameObject HandPanel = this.gameObject;
        if (playerStateManager.currentState == PlayerStateManager.playerState.selecting)
        {
            HandPanel.transform.position = HandPanel.transform.position - new Vector3(0, 100, 0);
        }
    }

    public void OnClickedCard()
    {
        GameObject HandPanel = this.gameObject;
        if (playerStateManager.currentState == PlayerStateManager.playerState.selecting)
        {
            HandPanel.transform.position = HandPanel.transform.position + new Vector3(0, 200, 0);
            StartCoroutine(SelectionPause());
        }
    }

    IEnumerator SelectionPause()
    {
        yield return new WaitForSeconds(0.2f);
        OnCardClicked?.Invoke();
    }
}
