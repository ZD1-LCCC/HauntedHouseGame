using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    
    public void ReStart() {
        //reset values to completely restart the game
        ResetValues();
        Debug.Log("All values reset!");
        SceneManager.LoadScene("Room#1");
    }
    
    public void Exit() {
        Application.Quit();
        //commented out cause it prevents me from building the game
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public static void Unpause() {
        GameManager.instance.pauseValue = false;
        Destroy(GameObject.Find("PauseHUD(Clone)"));
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ResetValues() {
        GameManager GM = GameManager.instance;

        //just for safety, unpauses the game when restarting
        GM.pauseValue = false;

        //resets the locked door array by unlocking all doors then locking the ones that are supposed to be unlocked
        for (int x = 0; x < 6; ++x) {
            for (int y = 0; y < 4; ++y) {
                GM.lockedDoorArray[x,y] = false;
            }
        }
        GM.lockedDoorArray[0,0] = true;
        GM.lockedDoorArray[0,3] = true;

        //resets interactables
        GM.interactableArray[2][0] = false;
        GM.interactableArray[0][0] = false;

        //resets the doorfron munber to prevent errors of the player spawning
        GM.doorFrom = -1;

        //resets the inventory list to prevent carrying items between runs
        GM.InventoryList.Clear();

        //resets the number of times each room was entered for the room text
        for (int x = 0; x < 6; ++x) {
            GM.numberEnter[x] = 0;
        }

        //resets the puzzles solved and puzzles list
        GM.puzzlesSolved = 0;
        for (int x = 0; x < 4; ++x) {
            GM.puzzlesList[x] = false;
        }
    }
}
