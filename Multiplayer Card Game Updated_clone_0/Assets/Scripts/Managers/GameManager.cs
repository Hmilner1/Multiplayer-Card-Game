using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    private float timeRemaining;
    private bool timerStarted;
    private bool player1Turn;
    private bool player2Turn;
    [SerializeField]
    private float totalTime = 600f;
    [SerializeField]
    private TMP_Text gameTimer;
    [SerializeField]
    private Button endTurnButton;


    private void OnEnable()
    {
        PlayerSetupManager.OnPlayerSetUp += TimerStart;
        PlayerSetupManager.OnPlayerSetUp += PlayerTurnOrderSetUp;

    }

    private void OnDisable()
    {
        PlayerSetupManager.OnPlayerSetUp -= TimerStart;
        PlayerSetupManager.OnPlayerSetUp -= PlayerTurnOrderSetUp;
    }

    public override void OnNetworkSpawn()
    {
        player1Turn = false;
        player2Turn = false;
        timerStarted = false;
        timeRemaining = totalTime;
    }

    private void FixedUpdate()
    {
        if (timerStarted) { TimerCountDown(); }
    }

    private void PlayerTurnOrderSetUp()
    {
        SetTurnServerRpc();
    }
    private void TimerStart()
    {
        timerStarted = true;
    }

    private void TimerCountDown()
    {
        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            TimerUI();
        }
        else
        {
            Debug.Log("Countdown reached zero!");
        }
    }

    private void TimerUI()
    {
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            string timeString = ( minutes + ":" + seconds);

            gameTimer.text = timeString;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InitialSetTurnServerRpc()
    {
        InitialSetTurnClientRpc();
    }

    [ClientRpc]
    private void InitialSetTurnClientRpc()
    {
        //PlayerStateManager[] playerManager = GameObject.FindObjectsOfType<PlayerStateManager>();
        //foreach (PlayerStateManager playerStateManager in playerManager)
        //{
        //    playerStateManager.currentState = PlayerStateManager.playerState.idle;
        //}
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetTurnServerRpc()
    {
        SetTurnClientRpc();
    }

    [ClientRpc]
    private void SetTurnClientRpc()
    {
        PlayerStateManager[] playerManager = GameObject.FindObjectsOfType<PlayerStateManager>();
        foreach (PlayerStateManager playerStateManager in playerManager) 
        {
            playerStateManager.currentState = PlayerStateManager.playerState.idle;
        }
    }
}
