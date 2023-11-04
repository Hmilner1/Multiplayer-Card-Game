using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Net.NetworkInformation;

public class PlayerStateManager : NetworkBehaviour
{
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
        
    }
}
