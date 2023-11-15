using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using static PlayerStateManager;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    private GameObject Player1;
    private GameObject Player2;

    public enum GameState
    { 
        Start,
        player1,
        player2,
        End
    }
    public GameState currentState;

    private void OnEnable()
    {
        GameTimerUI.OnTimerEnd += EndTimer;
    }

    private void OnDisable()
    {
        GameTimerUI.OnTimerEnd -= EndTimer;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        currentState= GameState.Start;

        SetPlayerID();
    }

    private void Update()
    {
        if (!IsServer) return;
        switch (currentState)
        {
            case GameState.Start:
                StartingPhase();
                break;
            case GameState.player1:
                
                break;
            case GameState.player2:
                
                break;
            case GameState.End:
                EndPhase();
                break;
        }
    }

    private void SetPlayerID()
    {
        PlayerStateManager[] playerManagers = GameObject.FindObjectsOfType<PlayerStateManager>();
        foreach (var playerManager in playerManagers)
        { 
            var playerObject = playerManager.transform.gameObject;
            var networkObj = playerObject.GetComponent<NetworkObject>();
            if (networkObj.OwnerClientId == 0)
            {
                Player1 = playerObject;
            }
            else
            { 
                Player2 = playerObject;

            }
        }
    }

    private void EndTimer()
    { 
        currentState= GameState.End;
    }

    private void StartingPhase()
    {
        SetPlayerStateServerRpc();
    }

    private void EndPhase()
    {
        EndGameServerRpc();
    }

    [ServerRpc]
    private void SetPlayerStateServerRpc()
    {
        SetPlayerStateClientRpc();
    }

    [ClientRpc]
    private void SetPlayerStateClientRpc()
    {
        Player1.GetComponent<PlayerStateManager>().currentState = playerState.idle;
        Player2.GetComponent<PlayerStateManager>().currentState = playerState.idle;
    }

    [ServerRpc]
    private void EndGameServerRpc()
    {
        EndGameClientRpc();
    }

    [ClientRpc]
    private void EndGameClientRpc()
    {
        SceneMan.Instance.LoadGivenScene(4);
    }
}
