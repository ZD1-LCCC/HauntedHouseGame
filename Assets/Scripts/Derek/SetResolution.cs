using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolution : MonoBehaviour
{
    // Public variables for resolution settings
    public int width = 1920;  // Desired screen width
    public int height = 1080; // Desired screen height
    public bool fullScreen = true; // Fullscreen mode

    void Start()
    {
        // Set the screen resolution
        Screen.SetResolution(width, height, fullScreen);
        Debug.Log($"Resolution set to {width}x{height}, Fullscreen: {fullScreen}");
    }
}
