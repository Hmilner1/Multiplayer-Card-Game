using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSetupManager : NetworkBehaviour
{
    #region Events
    public delegate void PlayerSetUp();
    public static event PlayerSetUp OnPlayerSetUp;
    #endregion

    [SerializeField]
    private GameObject playerCam;
    [SerializeField]
    private GameObject playerHand;
    [SerializeField]
    private GameObject PlayerUI;

    private void Start()
    {
        if (!IsOwner)
        {
            Destroy(playerCam);
            Destroy(playerHand);
            Destroy(PlayerUI);
        }
        OnPlayerSetUp?.Invoke();
    }
}
