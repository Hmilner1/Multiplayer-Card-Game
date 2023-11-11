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
    [SerializeField]
    Canvas handCanvas;
   
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
        PlayerSetupManager.OnPlayerSetUp += PlayerSpawn;
    }

    private void Start()
    {
        if (IsOwner)
        {
            DisableMatchMakingServerRpc();
            DisablePrivateMatchMakingServerRpc();
        }
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
        var FirePoint = new Vector2(0, 0);
        if (Input.GetMouseButton(0))
        {
            Vector2 MousePos = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)handCanvas.transform, MousePos, null, out FirePoint);
            Debug.Log(FirePoint);
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
        currentState = playerState.selecting;
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
