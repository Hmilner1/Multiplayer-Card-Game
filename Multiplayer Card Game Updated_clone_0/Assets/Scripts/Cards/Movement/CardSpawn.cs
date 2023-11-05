using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CardSpawn : MonoBehaviour
{

    private Vector3 _dir;

    public void Init(Vector3 dir)
    {
        GetComponent<Rigidbody>().AddForce(dir);
    }
}
