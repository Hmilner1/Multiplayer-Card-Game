using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using static PlayerStateManager;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    private GameObject player1;
    private GameObject player2;
    private string playerColourChoice;

    [SerializeField]
    private GameObject coinTossCanvas;
    private GameObject p1Panel;
    private GameObject p2Panel;
    private bool coinTossSpawned;
    private bool CardsDrawn;
    private bool player1Turn;
    private bool player2Turn;

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
        CoinTossPanelUI.OnCoinSelected += SetupCoinTossColourClientRpc;
        CoinTossPanelUI.OnCoinSelected += PreformCoinToss;
    }

    private void OnDisable()
    {
        GameTimerUI.OnTimerEnd -= EndTimer;
        CoinTossPanelUI.OnCoinSelected -= SetupCoinTossColourClientRpc;
        CoinTossPanelUI.OnCoinSelected -= PreformCoinToss;
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

        coinTossSpawned = false;
        CardsDrawn = false;
        player1Turn = false;
        player2Turn = false;
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
                if (!player1Turn)
                {
                    player1.GetComponent<PlayerStateManager>().currentState = playerState.selecting;

                    var p1CardDispense = player1.transform.gameObject.GetComponentInChildren<CardDispenser>();
                    if (p1CardDispense != null)
                    {
                        p1CardDispense.OnClickDrawCard(1);
                    }
                    player1Turn = true;
                }
                break;
            case GameState.player2:
                if (!player2Turn)
                {
                    player2.GetComponent<PlayerStateManager>().currentState = playerState.selecting;

                    var p2CardDispense = player2.transform.gameObject.GetComponentInChildren<CardDispenser>();
                    if (p2CardDispense != null)
                    {
                        p2CardDispense.OnClickDrawCard(1);
                    }
                    player2Turn = true;
                }
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
                player1 = playerObject;
            }
            else
            { 
                player2 = playerObject;
            }
        }
    }


    private void StartingPhase()
    {
        SetPlayerStateServerRpc();
        if (!coinTossSpawned && IsServer)
        {
            SetupCoinTossServerRpc();

            p2Panel.SetActive(false);
            p1Panel.SetActive(true);
        }
    }

    private void PreformCoinToss(string Colour)
    {
        playerColourChoice = Colour;
        StartCoroutine(RemoveCoinTossUI());
    }

    IEnumerator RemoveCoinTossUI()
    {
        yield return new WaitForSeconds(1f);
        p1Panel.transform.parent.gameObject.SetActive(false);
        if (IsServer)
        {
            int random = Random.Range(1, 3);
            CoinTossAnimationServerRpc(random);
        }
        StopCoroutine(RemoveCoinTossUI());
    }

    private void CoinTossAnimation(int num)
    {
        GameObject Coin = GameObject.Find("Coin");
        Animator CoinAnimator = Coin.GetComponent<Animator>();
        if (num == 1)
        {
            CoinAnimator.SetTrigger("Red");
            if (playerColourChoice == "RED")
            {
                currentState = GameState.player1;
            }
            else
            {
                currentState = GameState.player2;
            }
        }
        else if (num == 2)
        {
            CoinAnimator.SetTrigger("Blue");
            if (playerColourChoice == "BLUE")
            {
                currentState = GameState.player1;
            }
            else
            {
                currentState = GameState.player2;
            }
        }

        if (!CardsDrawn)
        {
            var p1CardDispense = player1.transform.gameObject.GetComponentInChildren<CardDispenser>();
            if (p1CardDispense != null)
            {
                p1CardDispense.OnClickDrawCard(3);
            }

            var p2CardDispense = player2.transform.gameObject.GetComponentInChildren<CardDispenser>();
            if (p2CardDispense != null)
            {
                p2CardDispense.OnClickDrawCard(3);
            }

            CardsDrawn = true;
        }
    }

    private void EndPhase()
    {
        EndGameServerRpc();
    }

    private void EndTimer()
    {
        currentState = GameState.End;
    }

    [ServerRpc]
    private void CoinTossAnimationServerRpc(int num)
    {
        CoinTossAnimationClientRpc(num);
    }

    [ClientRpc]
    private void CoinTossAnimationClientRpc(int num)
    {
        CoinTossAnimation(num);
    }

    [ClientRpc]
    private void SetupCoinTossColourClientRpc(string Colour)
    {
        CoinTossPanelUI coinText = p2Panel.GetComponentInParent<CoinTossPanelUI>();
        coinText.OnColourSetText(Colour);
        StartCoroutine(RemoveCoinTossUI());
    }

    [ServerRpc]
    private void SetupCoinTossServerRpc()
    {
        SetupCoinTossClientRpc();
    }

    [ClientRpc]
    private void SetupCoinTossClientRpc()
    {
        Instantiate(coinTossCanvas);
        if (IsClient)
        {
            p1Panel = GameObject.Find("P1 Panel");
            p2Panel = GameObject.Find("P2 Panel");

            p1Panel.SetActive(true);
            p1Panel.SetActive(false);
        }
        coinTossSpawned = true;
    }

    [ServerRpc]
    private void SetPlayerStateServerRpc()
    {
        SetPlayerStateClientRpc();
    }

    [ClientRpc]
    private void SetPlayerStateClientRpc()
    {
        player1.GetComponent<PlayerStateManager>().currentState = playerState.idle;
        player2.GetComponent<PlayerStateManager>().currentState = playerState.idle;
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
