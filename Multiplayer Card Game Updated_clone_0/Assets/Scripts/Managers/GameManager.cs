using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using static PlayerStateManager;
using UnityEngine.UI;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    private GameObject player1;
    private GameObject player2;

    private TextMeshProUGUI player1Name;
    private TextMeshProUGUI player2Name;

    private string playerColourChoice;

    [SerializeField]
    private GameObject coinTossCanvas;
    [SerializeField]
    private Button endTurnButton;
    private GameObject p1Panel;
    private GameObject p2Panel;
    private bool coinTossSpawned;
    private bool CardsDrawn;
    private bool player1Turn;
    private bool player2Turn;
    private bool coinTossed;
    private bool namesSet;

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

        endTurnButton.interactable = false;

        var playerSaves = GameObject.FindObjectsOfType<PlayerInfoManager>();
        foreach (var saves in playerSaves)
        { 
            Destroy(saves.gameObject);
        }

        currentState= GameState.Start;

        SetPlayerID();

        coinTossSpawned = false;
        CardsDrawn = false;
        player1Turn = false;
        player2Turn = false;
        coinTossed = false;
        namesSet = false;
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
                    SetPlayer1StateServerRpc();
                    player1Turn = true;
                }
                break;
            case GameState.player2:
                if (!player2Turn)
                {
                    SetPlayer2StateServerRpc();
                    player2Turn = true;
                }
                break;
            case GameState.End:
                EndPhase();
                break;
        }
    }

    protected virtual void SetPlayerID()
    {
        PlayerStateManager[] playerManagers = GameObject.FindObjectsOfType<PlayerStateManager>();
        foreach (var playerManager in playerManagers)
        { 
            var networkObj = playerManager.GetComponentInChildren<NetworkObject>();
            if (networkObj.OwnerClientId == 0)
            {
                player1 = playerManager.transform.gameObject;

            }
            else if(networkObj.OwnerClientId == 1)
            { 
                player2 = playerManager.transform.gameObject;
            }
        }
    }

    public void SetPlayerNames()
    {
        if (!namesSet)
        {
            var playerSave = GameObject.FindObjectOfType<PlayerInfoManager>();
            if (IsServer)
            {
                SetName1ServerRpc(playerSave.playerName);
            }
            else
            { 
                SetName2ServerRpc(playerSave.playerName);

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
        if (IsServer)
        {
            RemoveCoinServerRpc();
        }
    }

    IEnumerator RemoveCoin()
    {
        yield return new WaitForSeconds(2f);
        GameObject Coin = GameObject.Find("Coin");
        LeanTween.scale(Coin, new Vector3(0, 0, 0), 0.2f).setEase(LeanTweenType.easeInOutElastic).setDestroyOnComplete(true);
        StopCoroutine(RemoveCoin());
    }

    public void OnClickEndTurn()
    {
        endTurnButton.interactable = false;
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

    [ServerRpc(RequireOwnership = false)]
    private void SetName1ServerRpc(string Name)
    {
        SetName1ClientRpc(Name);
    }

    [ClientRpc]
    private void SetName1ClientRpc(string Name)
    {
        player1Name = GameObject.Find("Player1 Name").GetComponent<TextMeshProUGUI>();
        player1Name.text = Name;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetName2ServerRpc(string Name)
    {
        SetName2ClientRpc(Name);
    }

    [ClientRpc]
    private void SetName2ClientRpc(string Name)
    {
        player2Name = GameObject.Find("Player2 Name").GetComponent<TextMeshProUGUI>();
        player2Name.text = Name;
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

    [ServerRpc]
    private void RemoveCoinServerRpc()
    {
        RemoveCoinClientRpc();
    }

    [ClientRpc]
    private void RemoveCoinClientRpc()
    {
        StartCoroutine(RemoveCoin());
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
        player1Turn = false;
        player2Turn = false;
        if (IsServer)
        {
            endTurnButton.interactable = true;
        }

        player1.GetComponent<PlayerStateManager>().currentState = playerState.selecting;
        player2.GetComponent<PlayerStateManager>().currentState = playerState.idle;

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
        player1Turn = false;
        player2Turn = false;
        if (!IsServer && IsClient)
        {
            endTurnButton.interactable = true;
        }

        player1.GetComponent<PlayerStateManager>().currentState = playerState.idle;
        player2.GetComponent<PlayerStateManager>().currentState = playerState.selecting;

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
