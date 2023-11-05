using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Net.NetworkInformation;

public class PlayerStateManager : NetworkBehaviour
{
    private Camera playerCam;
    [SerializeField]
    private GameObject cardSpawnPoint;
    [SerializeField]
    private CardSpawn cardToSpawn;
    [SerializeField]
    private int cardSpeed = 2000;


    public enum playerState
    { 
        selecting,
        firing,
        idle
    }

    public playerState currentState;

    private void OnEnable()
    {
        HandUIController.OnCardClicked += CardClicked;
        PlayerSetupManager.OnPlayerSetUp += PlayerSpawn;
    }

    private void OnDisable()
    {
        HandUIController.OnCardClicked -= CardClicked;
        PlayerSetupManager.OnPlayerSetUp += PlayerSpawn;
    }

    private void Update()
    {
        if (!IsOwner) return;
        switch (currentState)
        {
            case playerState.idle:
                Idle();
                break;
            case playerState.selecting:
                Selecting();
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
        playerCam = GetComponentInChildren<Camera>();
    }

    private void CardClicked()
    {
        if (!IsOwner) return;
        currentState = playerState.firing;
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
            var FirePoint = new Vector3(0, 0, 0);
            FirePoint = Input.mousePosition;
            FirePoint.z = playerCam.nearClipPlane;
            FirePoint = playerCam.ScreenToWorldPoint(FirePoint);
            Debug.Log(FirePoint.x + " , " + FirePoint.y);
        }

        if (Input.GetMouseButtonUp(0))
        {
            var dir = transform.forward;
            FireServerRpc(dir);

            Fire(dir);
        }
    }

    [ServerRpc]
    private void FireServerRpc(Vector3 dir)
    {
        FireClientRpc(dir);
    }

    [ClientRpc]
    private void FireClientRpc(Vector3 dir)
    {
        if (!IsOwner)
        {
            Fire(dir);
        }
    }

    private void Fire(Vector3 dir)
    {
        var Card = Instantiate(cardToSpawn, cardSpawnPoint.transform.position, cardSpawnPoint.transform.rotation);
        Card.Init(dir * cardSpeed);
    }

}
