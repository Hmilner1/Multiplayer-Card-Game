using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEditor.PackageManager;

public class SpawnManager : NetworkBehaviour
{
    public List<Transform> spawnPoints;
    public NetworkObject playerPrefab;
    private IReadOnlyList<NetworkClient> players;
    private bool playersSpawned;

    public override void OnNetworkSpawn()
    {
        playersSpawned = false;
    }

    private void Update()
    {
        if (!IsServer) return;
        BothPlayersConnected();
    }

    private void BothPlayersConnected()
    {
        if (playersSpawned) return;
        if (IsServer)
        {
            players = NetworkManager.Singleton.ConnectedClientsList;

            if (players.Count == 2)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    int spawnIndex = i % spawnPoints.Count;
                    var playerObject = Instantiate(playerPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
                    playerObject.Spawn();
                    playerObject.GetComponent<NetworkObject>().ChangeOwnership(players[i].ClientId);
                }
                playersSpawned = true;
            }
        }
    }

    

}
