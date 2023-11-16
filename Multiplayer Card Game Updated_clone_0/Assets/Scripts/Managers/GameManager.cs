using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using static PlayerStateManager;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    private GameObject player1;
    private GameObject player2;
    private CardDispenser p1CardDispense;
    private CardDispenser p2CardDispense;
    private string playerColourChoice;

    [SerializeField]
    private GameObject coinTossCanvas;
    private GameObject p1Panel;
    private GameObject p2Panel;
    private Button endTurnButton;
    private bool coinTossSpawned;
    private bool CardsDrawn;
    private bool player1Turn;
    private bool player2Turn;
    private bool coinTossed;

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

        endTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
        endTurnButton.interactable = false;

        currentState= GameState.Start;

        SetPlayerID();

        coinTossSpawned = false;
        CardsDrawn = false;
        player1Turn = false;
        player2Turn = false;
        coinTossed = false;
    }

    private void Update()
    {
        endTurnButton.onClick.AddListener(OnClick);
        if (!IsServer) return;
        switch (currentState)
        {
            case GameState.Start:
                StartingPhase();
                break;
            case GameState.player1:
                if (!player1Turn)
                {
                    SetPlayer1StateServerRpc();

                    if (!CardsDrawn)
                    {
                        p1CardDispense.OnClickDrawCard(1);
                        CardsDrawn = true;
                    }
                    player1Turn = true;
                }
                break;
            case GameState.player2:
                if (!player2Turn)
                {
                    SetPlayer2StateServerRpc();

                    if (!CardsDrawn)
                    {
                        if (p2CardDispense != null)
                        {
                            p2CardDispense.OnClickDrawCard(1);
                            CardsDrawn = true;
                        }
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
                p1CardDispense = playerObject.GetComponentInChildren<CardDispenser>();

            }
            else
            { 
                player2 = playerObject;
                p2CardDispense = playerObject.GetComponentInChildren<CardDispenser>();
            }
        }
    }

    private void StartingPhase()
    {
        SetPlayerStartStateServerRpc();
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
            CoinTossAnimationServerRpc();
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
            if (IsServer)
            {
                if (playerColourChoice == "RED")
                {
                    SetStateServerRpc(GameState.player1);
                }
                else
                {
                    SetStateServerRpc(GameState.player2);
                }
            }
        }
        else if (num == 2)
        {
            CoinAnimator.SetTrigger("Blue");
            if (IsServer)
            {
                if (playerColourChoice == "BLUE")
                {
                    SetStateServerRpc(GameState.player1);
                }
                else
                {
                    SetStateServerRpc(GameState.player2);
                }
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

    private void OnClick()
    {
        player1Turn = false;
        player2Turn = false;
        if (currentState == GameState.player1)
        {
            ChangeStateServerRpc(GameState.player2);
        }
        else if (currentState == GameState.player2)
        {
            ChangeStateServerRpc(GameState.player1);
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
    private void CoinTossAnimationServerRpc()
    {
        int random = 3;
        if (!coinTossed)
        {
            random = Random.Range(1, 3);
            coinTossed = true;
        }
        CoinTossAnimationClientRpc(random);
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
    private void SetPlayerStartStateServerRpc()
    {
        SetPlayerStartStateClientRpc();
    }

    [ClientRpc]
    private void SetPlayerStartStateClientRpc()
    {
        player1.GetComponent<PlayerStateManager>().currentState = playerState.idle;
        player2.GetComponent<PlayerStateManager>().currentState = playerState.idle;
    }

    [ServerRpc]
    private void SetPlayer1StateServerRpc()
    {
        SetPlayer1StateClientRpc();
    }

    [ClientRpc]
    private void SetPlayer1StateClientRpc()
    {
        player1.GetComponent<PlayerStateManager>().currentState = playerState.selecting;
        player2.GetComponent<PlayerStateManager>().currentState = playerState.idle;


        if (IsClient)
        {
            endTurnButton.interactable = false;
        }

        if (IsServer)
        {
            endTurnButton.interactable = true;
        }
        CardsDrawn = false;
    }

    [ServerRpc]
    private void SetPlayer2StateServerRpc()
    {
        SetPlayer2StateClientRpc();
    }

    [ClientRpc]
    private void SetPlayer2StateClientRpc()
    {
        player1.GetComponent<PlayerStateManager>().currentState = playerState.idle;
        player2.GetComponent<PlayerStateManager>().currentState = playerState.selecting;

        if (IsServer)
        {
            endTurnButton.interactable = false;
        }

        if (!IsServer && IsClient)
        {
            endTurnButton.interactable = true;
        }
        CardsDrawn = false;
    }

    [ServerRpc]
    private void SetStateServerRpc(GameState state)
    {
        SetStateClientRpc(state);
    }

    [ClientRpc]
    private void SetStateClientRpc(GameState state)
    {
        currentState = state;
        player1Turn = false;
        player2Turn = false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeStateServerRpc(GameState state)
    {
        ChangeStateClientRpc(state);
    }

    [ClientRpc]
    private void ChangeStateClientRpc(GameState state)
    {
        currentState = state;
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