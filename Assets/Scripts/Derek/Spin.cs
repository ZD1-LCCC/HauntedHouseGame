using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{    // Public variables for rotation speeds
    public float rotationSpeedY = 10f; // Speed for spinning around the Y-axis
    public float rotationSpeedX = 10f; // Speed for spinning around the X-axis

    // Update is called once per frame
    void Update()
    {
        // Calculate rotation
        float rotationY = rotationSpeedY * Time.deltaTime;
        float rotationX = rotationSpeedX * Time.deltaTime;

        // Apply rotation
        transform.Rotate(rotationX, rotationY, 0);
    }
}
