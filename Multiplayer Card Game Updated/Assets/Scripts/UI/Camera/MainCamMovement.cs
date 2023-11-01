using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamMovement : MonoBehaviour
{
    private Transform mainCamTransform;
    [SerializeField]
    private int rotationSpeed = 10;

    private void Start()
    {
        mainCamTransform = GetComponent<Transform>();
    }
    private void Update()
    {
        mainCamTransform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
    }
}
