using ParrelSync;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class PrivateMatchMaker : MonoBehaviour
{
    #region Events
    public delegate void PrivateLobbyHost(string joinCode);
    public static event PrivateLobbyHost OnPrivateLobbyHost;
    #endregion

    private string clientJoinCode;
    private UnityTransport networkManagerTransport;

    private async void Awake()
    {
        networkManagerTransport = GameObject.FindAnyObjectByType<UnityTransport>();


        await AuthenticatePlayer();
    }

    private static async Task AuthenticatePlayer()
    {
        var options = new InitializationOptions();

#if UNITY_EDITOR
        // Remove this if you don't have ParrelSync installed. 
        // It's used to differentiate the clients, otherwise lobby will count them as the same
        options.SetProfile(ClonesManager.IsClone() ? ClonesManager.GetArgument() : "Primary");
#endif

        await UnityServices.InitializeAsync(options);

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void OnHostGame()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        OnPrivateLobbyHost?.Invoke(joinCode);

        networkManagerTransport.SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);
        NetworkManager.Singleton.StartHost();
    }

    public void SetClientJoinCode(string JoinCode)
    {
        clientJoinCode = JoinCode;
    }

    public async void OnClientJoin()
    {
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(clientJoinCode);
        networkManagerTransport.SetClientRelayData(joinAllocation.RelayServer.IpV4, (ushort)joinAllocation.RelayServer.Port, joinAllocation.AllocationIdBytes, joinAllocation.Key, joinAllocation.ConnectionData, joinAllocation.HostConnectionData);

        NetworkManager.Singleton.StartClient();
        SceneMan.Instance.UnloadMatchMaking(2);


    }
}
