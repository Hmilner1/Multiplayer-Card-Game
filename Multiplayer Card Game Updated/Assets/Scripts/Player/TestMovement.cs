using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class TestMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;




    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(this);
        }
        else
        {
            transform.position = new Vector3(0, 1, 0);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        

        // Move the character
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed;
        movement = transform.TransformDirection(movement);
        
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);


    }
}

