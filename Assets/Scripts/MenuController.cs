using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public void ReStart() {
        SceneManager.LoadScene("Room#1");
    }
    
    public void Exit() {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
