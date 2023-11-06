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
            LeanTween.scale(HandPanel, new Vector3(1.5f, 1.5f, 1.5f), .2f).setEase(LeanTweenType.easeOutElastic);

        }
    }

    public void OnMouseExitHand()
    {
        GameObject HandPanel = this.gameObject;
        if (playerStateManager.currentState == PlayerStateManager.playerState.selecting)
        {
            HandPanel.transform.position = HandPanel.transform.position - new Vector3(0, 100, 0);
            LeanTween.scale(HandPanel, new Vector3(1, 1, 1), .2f).setEase(LeanTweenType.easeOutElastic);

        }
    }

    public void OnClickedCard()
    {
        GameObject HandPanel = this.gameObject;
        if (playerStateManager.currentState == PlayerStateManager.playerState.selecting)
        {
            LeanTween.moveLocal(HandPanel, new Vector3(0, 200, 0), 0.2f).setEase(LeanTweenType.easeInOutCubic);
            StartCoroutine(SelectionPause());
        }

        if (playerStateManager.currentState == PlayerStateManager.playerState.firing)
        {
            LeanTween.scale(HandPanel, new Vector3(1.2f, 1.2f, 1.2f), .1f).setEase(LeanTweenType.easeOutElastic);
            StartCoroutine(FirePause(HandPanel));
        }
    }

    IEnumerator SelectionPause()
    {
        yield return new WaitForSeconds(0.1f);
        //OnCardClicked?.Invoke();
        playerStateManager.currentState = PlayerStateManager.playerState.firing;
    }
    IEnumerator FirePause(GameObject card)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(card);
    }
}
