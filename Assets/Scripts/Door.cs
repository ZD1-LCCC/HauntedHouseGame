using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    //array, 2d, for finding the destination   
    public int[,] roomGridArray = {
        {2,6,0,5},
        {0,3,1,4},
        {2,0,0,0},
        {0,0,2,0},
        {0,0,0,1},
        {0,0,0,1}
    };

    //this is for controlling doors and moving scenes
    private void OnTriggerEnter() {

    //find the scene name
    string sceneName = SceneManager.GetActiveScene().name;
    Debug.Log("The active scene name is " + sceneName);

    //find door number by converting into integer
    string sceneNumStr = sceneName.Substring(sceneName.Length - 1);
    Debug.Log("The scene num is " + sceneNumStr);

    //to convert to int
    int sceneNum;
    sceneNum = int.Parse(sceneNumStr);

    //find the door name by switch statement?
    int goNext = 0;
    switch(this.name) {
        case "NorthDoor":
            goNext = roomGridArray[sceneNum-1, 0];
            break;
        case "EastDoor":
            goNext = roomGridArray[sceneNum-1, 1];
            break;
        case "SouthDoor":
            goNext = roomGridArray[sceneNum-1, 2];
            break;
        case "WestDoor":
            goNext = roomGridArray[sceneNum-1, 3];
            break;
        default:
            Debug.Log("Door Invalid, " + this.name);
            goNext = sceneNum;
            break;
    }
    Debug.Log("The Destination is room#" + goNext);
    
    //convert to use as string name
    string NumToStr = goNext.ToString();
    string nextScene = "Room#";
    nextScene = nextScene + NumToStr;
    SceneManager.LoadScene(nextScene);
    }
}
