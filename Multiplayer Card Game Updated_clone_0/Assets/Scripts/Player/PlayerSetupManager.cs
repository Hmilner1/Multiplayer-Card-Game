using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSetupManager : NetworkBehaviour
{ 
    [SerializeField]
    private GameObject playerCam;

    private void Start()
    {
        if (!IsOwner)
        {
            Destroy(playerCam);
        }
    }


}
