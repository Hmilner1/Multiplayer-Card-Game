using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkDebugGui : MonoBehaviour
{
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(16,16,300,300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        { 
            if(GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if(GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        }
        GUILayout.EndArea();
    }
}
