using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Net.NetworkInformation;

public class PlayerStateManager : NetworkBehaviour
{
    private Camera playerCam;

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
        if (!IsOwner)
        {
            enabled = false;
        }
        currentState = playerState.selecting;
        playerCam = GetComponentInChildren<Camera>();
    }

    private void CardClicked()
    {
        currentState = playerState.firing;
    }

    private void Idle()
    { 
    
    }

    private void Selecting()
    { 

    }

    private void Firing()
    {
        Vector3 FirePoint = new Vector3(0,0,0);
       
        if (Input.GetMouseButton(0))
        {
            FirePoint = Input.mousePosition;
            FirePoint.z = playerCam.nearClipPlane;
            FirePoint = playerCam.ScreenToWorldPoint(FirePoint);
            Debug.Log(FirePoint.x + " , " + FirePoint.y);
        }
    }
}
