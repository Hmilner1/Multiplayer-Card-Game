using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public LineRenderer lineRenderer;

    void Start()
    {
        if (lineRenderer != null && lineRenderer.material != null)
        {
            // Set the render queue to a value greater than the UI's render queue
            lineRenderer.material.renderQueue = 3000;
        }
    }

}
