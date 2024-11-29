using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Door : MonoBehaviour
{
    //array, 2d, for finding the destination   
    /*

    //this is for controlling doors and moving scenes
    private void OnTriggerEnter() {

        //find the scene name
        string sceneName = SceneManager.GetActiveScene().name;
        //Debug.Log("The active scene name is " + sceneName);

        //find door number by converting into integer
        string sceneNumStr = sceneName.Substring(sceneName.Length - 1);
        //Debug.Log("The scene num is " + sceneNumStr);

        //to convert to int
        int sceneNum;
        sceneNum = int.Parse(sceneNumStr);

        //find the door name by switch statement?
        int goNext = 0;
        int doorNum = 0;
        switch(this.name) {
            case "NorthDoor":
                goNext = roomGridArray[sceneNum-1, 0];
                doorNum = 0;
                break;
            case "EastDoor":
                goNext = roomGridArray[sceneNum-1, 1];
                doorNum = 1;
                break;
            case "SouthDoor":
                goNext = roomGridArray[sceneNum-1, 2];
                doorNum = 2;
                break;
            case "WestDoor":
                goNext = roomGridArray[sceneNum-1, 3];
                doorNum = 3;
                break;
            default:
                Debug.Log("Door Invalid, " + this.name);
                goNext = sceneNum;
                break;
        }
        //Debug.Log("The Destination is room#" + goNext);

        //to determine if the door is locked or not
        if (GameManager.instance.lockedDoorArray[sceneNum-1, doorNum] == false) {
            //sets the scene that the player entered from for finding the correct spawn position
            
        }
        else {
            GameManager.instance.doorFrom = sceneNum;
            LockedDoorTest(sceneNum, doorNum);
        }
    }

    //changes the room text to tell the player that the door is locked


    //public void ChangeRoomText() {

    //}

    //takes current room and door number to move to next room
    */
    
}
