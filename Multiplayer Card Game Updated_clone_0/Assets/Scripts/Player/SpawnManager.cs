using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class SpawnManager : NetworkBehaviour
{
    public List<Transform> spawnPoints;
    public NetworkObject playerPrefab;
    public NetworkObject gameManager;
    private IReadOnlyList<NetworkClient> players;
    private bool playersSpawned;
    private bool infoSpawned;
    private GameObject mainCam;
    [SerializeField]
    private GameObject playerSaveInfo;


    public override void OnNetworkSpawn()
    { 
        playersSpawned = false;
        mainCam = GameObject.Find("MainCamera");
    }

    private void Start()
    {
        infoSpawned = false;
    }

    private void Update()
    {
      
        if (!IsServer) return;
        BothPlayersConnected();
        if (!infoSpawned && playersSpawned)
        {
            SpawnInfoServerRpc();
        }
    }

    private void LoadPlayerInfo()
    { 
        Instantiate(playerSaveInfo);
        infoSpawned = true;
        GameManager.Instance.SetPlayerNames();
    }

    private void BothPlayersConnected()
    {
        if (playersSpawned) return;
        if (IsServer)
        {
            players = NetworkManager.Singleton.ConnectedClientsList;
            if (players != null)
            {
                if (players.Count == 2)
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        int spawnIndex = i % spawnPoints.Count;
                        var playerObject = Instantiate(playerPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
                        playerObject.name = "Player1";
                        playerObject.Spawn();
                        if (i == 1)
                        {
                            playerObject.name = "Player2";
                            playerObject.GetComponent<NetworkObject>().ChangeOwnership(players[i].ClientId);
                        }
                    }
                    playersSpawned = true;
                    DisableMainCamServerRpc();
                    var GameManagerObj = Instantiate(gameManager);
                    GameManagerObj.Spawn();
                }
            }
        }
    }

    [ServerRpc]
    private void DisableMainCamServerRpc()
    {
        DisableMainCamClientRpc();
    }

    [ClientRpc]
    private void DisableMainCamClientRpc()
    {
        Destroy(mainCam);
    }

    [ServerRpc]
    private void SpawnInfoServerRpc()
    {
        SpawnInfoClientRpc();
    }

    [ClientRpc]
    private void SpawnInfoClientRpc()
    {
        LoadPlayerInfo();
    }
}
