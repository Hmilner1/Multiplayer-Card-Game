using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandUIController : MonoBehaviour
{
    private PlayerStateManager playerStateManager;
    private bool clicked;
    public GameObject HandPanel;

    private void Start()
    {
        playerStateManager = GetComponentInParent<PlayerStateManager>();
        clicked = false;
    }

    private void Update()
    {
        
    }

    public void OnMouseOverHand()
    {
        HandPanel = this.gameObject;
        if (playerStateManager.currentState == PlayerStateManager.playerState.selecting && !clicked)
        {
            HandPanel.transform.position = new Vector3(HandPanel.transform.position.x, 50f, 0);
            LeanTween.scale(HandPanel, new Vector3(1.5f, 1.5f, 1.5f), .2f).setEase(LeanTweenType.easeOutElastic);

        }
    }

    public void OnMouseExitHand()
    {
        HandPanel = this.gameObject;
        if (playerStateManager.currentState == PlayerStateManager.playerState.selecting && !clicked)
        {
            HandPanel.transform.position = new Vector3(HandPanel.transform.position.x, 0, 0);
            LeanTween.scale(HandPanel, new Vector3(1, 1, 1), .2f).setEase(LeanTweenType.easeOutElastic);

        }
    }

    public void OnClickedCard()
    {
        HandPanel = this.gameObject;
        if (playerStateManager.currentState == PlayerStateManager.playerState.selecting && !clicked)
        {
            clicked = true;
            LeanTween.moveLocal(HandPanel, new Vector3(0, 300, 0), 0.2f).setEase(LeanTweenType.easeInOutCubic);
            StartCoroutine(SelectionPause());
        }
    }

    public void OnCLickFire()
    {
        if (playerStateManager.currentState == PlayerStateManager.playerState.firing)
        {
            LeanTween.scale(HandPanel, new Vector3(1.2f, 1.2f, 1.2f), .1f).setEase(LeanTweenType.easeOutElastic);
            StartCoroutine(FirePause(HandPanel));
        }
    }

    IEnumerator SelectionPause()
    {
        yield return new WaitForSeconds(0.1f);
        playerStateManager.currentState = PlayerStateManager.playerState.firing;
        playerStateManager.handUIController = this;
    }
    IEnumerator FirePause(GameObject card)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(card);
    }
}
