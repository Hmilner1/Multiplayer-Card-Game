using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using ParrelSync;
#endif

public class MainUIMatchMake : MonoBehaviour
{
    public Lobby currentLobby;
    private UnityTransport networkManagerTransport;
    private const string JoinCodeKey = "code";
    private string currentPlayerId;

    private void Awake()
    {
        networkManagerTransport = FindObjectOfType<UnityTransport>();
    }

    private void Start()
    {
        BasicMatchMake();
    }

    private void OnDestroy()
    {
        try
        {
            StopAllCoroutines();
            if (currentLobby != null)
            {
                Lobbies.Instance.DeleteLobbyAsync(currentLobby.Id);
            }
        }
        catch (Exception exception)
        {
            Debug.Log("Error shutting down lobby: " + exception.ToString());
        }
    }

    public async void BasicMatchMake()
    {
        await AuthenticatePlayer();

        if (await JoinLobby() != null)
        {
            currentLobby = await JoinLobby();
        }
        else
        {
            currentLobby = await CreateLobby();
        }
    }

    //Used to authenticate players in unity gaming services and allowes multiple instances on the same pc if in editor
    private async Task AuthenticatePlayer()
    {
        InitializationOptions initOptions = new InitializationOptions();

#if UNITY_EDITOR
        initOptions.SetProfile(ClonesManager.IsClone() ? ClonesManager.GetArgument() : "MainEditor");
#endif
        await UnityServices.InitializeAsync(initOptions);
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        currentPlayerId = AuthenticationService.Instance.PlayerId;
    }

    private async Task<Lobby> JoinLobby()
    {
        //tries to quick join returns null if there is no open lobbys
        try
        {
            Lobby lobby = await Lobbies.Instance.QuickJoinLobbyAsync();

            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(lobby.Data[JoinCodeKey].Value);

            networkManagerTransport.SetClientRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);

            NetworkManager.Singleton.StartClient();
            SceneMan.Instance.UnloadMatchMaking(1);
            return lobby;
        }
        catch (Exception exception)
        {
            Debug.Log("No lobbies open to join: " + exception.ToString());
            return null;
        }
    }

    private async Task<Lobby> CreateLobby()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            CreateLobbyOptions options = new CreateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> { { JoinCodeKey, new DataObject(DataObject.VisibilityOptions.Public, joinCode) } }
            };
            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync("Unranked Lobby", 2, options);

            networkManagerTransport.SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);

            StartCoroutine(PingLobby(lobby.Id, 15));

            NetworkManager.Singleton.StartHost();

            Button backButton = GameObject.Find("BackButton").GetComponent<Button>();
            backButton.interactable= true;
            return lobby;
        }
        catch (Exception exception)
        {
            Debug.LogFormat("Lobby Failed" + exception.ToString());
            return null;
        }
    }

    //Pings lobby to make sure server stays open
    private static IEnumerator PingLobby(string lobbyId, float pingLobbyTime)
    {
        WaitForSecondsRealtime delay = new WaitForSecondsRealtime(pingLobbyTime);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }
}
