using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerStateManager : NetworkBehaviour
{
    [SerializeField]
    private Camera playerCam;
    [SerializeField]
    private GameObject cardSpawnPoint;
    [SerializeField]
    private CardSpawn cardToSpawn;
    [SerializeField]
    private int cardSpeed = 2000;
    [SerializeField]
    Canvas handCanvas;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private CardDataBase cardData;
    [SerializeField]
    private CardDispenser dispenser;
    private bool cardDrawn;
    private Vector2 FirePoint;
    public HandUIController handUIController;
    public int firedCard;
    public List<GameObject> spawnedCards;

    public enum playerState
    { 
        selecting,
        firing,
        idle
    }

    public playerState currentState;

    private void OnEnable()
    {
        PlayerSetupManager.OnPlayerSetUp += PlayerSpawn;
    }

    private void OnDisable()
    {
        PlayerSetupManager.OnPlayerSetUp -= PlayerSpawn;
    }

    private void Start()
    {
        if (IsOwner)
        {
            DisableMatchMakingServerRpc();
            DisablePrivateMatchMakingServerRpc();
        }
        cardDrawn = false;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (!IsOwner) return;
        switch (currentState)
        {
            case playerState.idle:
                Idle();
                cardDrawn = false;
                break;
            case playerState.selecting:
                Selecting();
                if (!cardDrawn)
                {
                    dispenser.OnClickDrawCard(1);
                    cardDrawn = true;
                }
                break;
            case playerState.firing:
                Firing();
                break;
        }
    }

    public void PlayerSpawn()
    {
        if (!IsOwner) return;
        currentState = playerState.selecting;
    }

    private void Idle()
    {
        if (!IsOwner) return;

    }

    private void Selecting()
    {
        if (!IsOwner) return;
    }

    private void Firing()
    {
        if (!IsOwner) return;
        if (Input.GetMouseButton(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)handCanvas.transform, Input.mousePosition, null, out FirePoint);
            DrawFireLineRender();
        }

        if (Input.GetMouseButtonUp(0))
        {

            lineRenderer.enabled = false;

            FireServerRpc(CalculateFireForce(), firedCard);
            Fire(CalculateFireForce(), firedCard);

            EndFireState();
        }
    }

    private void DrawFireLineRender()
    {
        lineRenderer.enabled = true;

        Vector3 LineMousePos = FirePoint / 100;
        LineMousePos.y = LineMousePos.y + 3.5f;
        LineMousePos.x = Mathf.Clamp(LineMousePos.x, -1, 1);
        LineMousePos.y = Mathf.Clamp(LineMousePos.y, -1, 0.3f);

        lineRenderer.SetPosition(0, new Vector3(0,0,1));
        lineRenderer.SetPosition(1, new Vector3(LineMousePos.x,2, LineMousePos.y));
    }

    private Vector3 CalculateFireForce()
    {
        float distance = Vector3.Distance(Input.mousePosition, handUIController.HandPanel.transform.position);
        Vector3 dir = new Vector3(0, 0, 0);
        if (IsServer)
        {
            dir = new Vector3(-(FirePoint.x / 100), transform.forward.y, (transform.forward.z * distance) / 150);
        }
        else
        {
            dir = new Vector3((FirePoint.x / 100), transform.forward.y, (transform.forward.z * distance) / 150);

        }
        return dir;
    }

    private void Fire(Vector3 dir, int card)
    {
        var Card = Instantiate(cardToSpawn, cardSpawnPoint.transform.position, cardSpawnPoint.transform.rotation);
        CardObject cardobj = Card.GetComponent<CardObject>();
        cardobj.currentCard = cardData.cardDatabase[card];
        Card.Init(dir * cardSpeed);
        if (IsOwner)
        {
            spawnedCards.Add(Card.gameObject);
        }
    }

    private void EndFireState()
    {
        handUIController.OnCLickFire();
        currentState = playerState.selecting;
    }

    [ServerRpc]
    private void FireServerRpc(Vector3 dir, int card)
    {
        FireClientRpc(dir, card);
    }

    [ClientRpc]
    private void FireClientRpc(Vector3 dir, int card)
    {
        if (!IsOwner)
        {
            Fire(dir, card);
        }
    }

    [ServerRpc]
    private void DisableMatchMakingServerRpc()
    {
        DisableMatchMakingClientRpc();
    }

    [ClientRpc]
    private void DisableMatchMakingClientRpc()
    {
        var Menu = GameObject.Find("MatchMakingCanvas");
        if (Menu != null)
        {
            Menu.SetActive(false);
        }
    }

    [ServerRpc]
    private void DisablePrivateMatchMakingServerRpc()
    {
        DisablePrivateMatchMakingClientRpc();
    }

    [ClientRpc]
    private void DisablePrivateMatchMakingClientRpc()
    {
        var Menu = GameObject.Find("PrivateMatchCanvas");
        if (Menu != null)
        {
            Menu.SetActive(false);
        }
    }

}
