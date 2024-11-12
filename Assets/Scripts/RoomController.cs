using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class RoomController : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake() {
        //get rid of objects that are already in the inventory
        DisableCollectedItems();
        CheckPuzzle(); //only there for now, put it on other things like the interactable fireplace?
    }

    public void DisableCollectedItems() {
        //if an item is in my inventory, remove it from the scene wont work with puzzle
        foreach (Item item in GameManager.instance.InventoryList) {
            GameObject anObject = GameObject.Find(item.itemName);
            if (anObject != null) {
                //another if to find if it is a changing item aka book ashes
                //if (item.name == || item.name == || item.name == ) {
                //    Destroy
                //}
                //else
                    Destroy(anObject);
            }
        }
    }
    // Start is called before the first frame update
    private void Start() {
        //load text method for accesability?
        LoadText();
        gameManager = GameManager.instance;
    }



    public void LoadText() {
        //to get scene name and other info?
        string sceneName = SceneManager.GetActiveScene().name;

        int textNumb = 0;

        string sceneNumber = sceneName.Substring(sceneName.Length-1);
        Debug.Log("Room number is " + sceneNumber);

        int sceneNumb;
        sceneNumb = int.Parse(sceneNumber);

        //to determine what level of text to show
        if (GameManager.instance.numberEnter[sceneNumb-1] == 0) {
            textNumb = 0;
        }
        else if (GameManager.instance.numberEnter[sceneNumb-1] > 0) {
            textNumb = 1;
        }
        else {
            textNumb = 1;
        }
        //increment the number of times a room was entered
        ++GameManager.instance.numberEnter[sceneNumb-1];
        
        //figure out the text
        string myText = GameManager.instance.roomInfo[sceneNumb-1,textNumb];
        
        //actually changes the text
        GameObject.Find("RoomText").GetComponent<TextMeshProUGUI>().text = myText;
        
    }

    public void CheckPuzzle() {
        //find the room number?
        string theCurrentScene = SceneManager.GetActiveScene().name;
        string theRoomNumber = theCurrentScene.Substring(5);
        int roomInteger = int.Parse(theRoomNumber);

        //what stuff to solve
        int totItemsNeeded = GameManager.instance.roomNeedsArray[roomInteger-1].Length;
        Debug.Log("Room "+theRoomNumber+" needs "+totItemsNeeded+ " items.");

        //items match Check
        int totItemsFound = 0;
        GameManager.instance.IndicesOfItemsFound.Clear();
        for (int i = 0; i < totItemsNeeded; ++i) {
            string theItemToFind = GameManager.instance.roomNeedsArray[roomInteger - 1][i];
            if (string.IsNullOrEmpty(theItemToFind)) {
                break;
            }
            else {
            //ITS AN ARROW? THAT MAKES NO SENSE                             VVVV    
            int theIndex = GameManager.instance.InventoryList.FindIndex(Item => Item.itemName == theItemToFind);
                if(theIndex != -1) {
                    totItemsFound += 1;
                    GameManager.instance.IndicesOfItemsFound.Add(theIndex); //dot after add
                }
            }
        }

        if (totItemsFound == totItemsNeeded) {
            GameManager.puzzlesSolved += 1;
            for (int i = 0; i < GameManager.instance.IndicesOfItemsFound.Count; ++i) {
                int whichItem = GameManager.instance.IndicesOfItemsFound[i];
                //move item from the invetorylist to inventorylist used
                GameManager.instance.InventoryUsed.Add(GameManager.instance.InventoryList[whichItem]);
                //add logic to tell player that the puzzle was solved
                Debug.Log("Good Job! You solved a puzzle!");
            }
            //refresh and cleanup
            GameManager.instance.IndicesOfItemsFound.Clear();
        }

        if (GameManager.puzzlesSolved == 6)  { //total ammount of puzzles solved
            Debug.Log("Good Job! You beat the game!");
        }
    }
}