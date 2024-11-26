using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class RoomController : MonoBehaviour
{
    //private GameManager gameManager;

    public GameObject player;

    private void Start() {
        //gameManager = GameManager.instance; // can delete maybe
        LoadText();
        //ClearTheHUD();
        InitInventory();
        DisableCollectedItems();
        //CheckPuzzle(); should remove this and put it somewhere else
        //THESE SUDDENLY DONT WORK WHEN ON AWAKE() BECAUSE THEY CANNOT SEE THE GAME MANAGER FOR WHATEVER REASON SO I MOVED THEM TO START AND THEY WORK????

        SpawnPlayer();
    }

    private void Awake() {
        
    }

    public void DisableCollectedItems() {
        //if an item is in my inventory, remove it from the scene wont work with puzzle
        foreach (Item item in GameManager.instance.InventoryList) {
            GameObject anObject = GameObject.Find(item.itemName);
            if (anObject != null) {
                    Destroy(anObject);
            }
        }
    }

    public void LoadText() {
        //to get scene name and other info?
        string sceneName = SceneManager.GetActiveScene().name;

        int textNumb = 0;

        string sceneNumber = sceneName.Substring(sceneName.Length-1);
        //Debug.Log("Room number is " + sceneNumber);

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
            textNumb = 0;
        }
        //increment the number of times a room was entered
        ++GameManager.instance.numberEnter[sceneNumb-1];
        //Debug.Log(GameManager.instance.numberEnter[sceneNumb-1]);
        
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
            SceneManager.LoadScene("WinScene");
        }
    }

    private void SpawnPlayer() {
        //find scene number
        int sceneCurrent = int.Parse(SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length-1)) - 1;

        GameObject spawn = null;

        //spawn point object reference
        if (GameManager.instance.doorFrom > 0 && GameManager.instance.doorFrom <= 6) {
            spawn = GameObject.Find("SpawnPoint" + GameManager.instance.roomConnectorSpawns[sceneCurrent,(GameManager.instance.doorFrom - 1)].ToString());
            Debug.Log(spawn.name);
        }
        else {
            Debug.Log("Unknown room entered from!");
        }
        
        if (spawn != null) {
            //spawn player to spawn point
            //player.transform.position = spawn.transform.position;
            Instantiate(player, spawn.transform.position, spawn.transform.rotation);
        }
        else {
            Instantiate(player, new Vector3(0,0,0), Quaternion.Euler(0,0,0));
            Debug.Log("Spawning at 0,0,0!");
        }
    }

    private void InitInventory() {
        int whichOne = 1;
        Texture2D tex;
        Sprite mySprite;

        foreach(Item item in GameManager.instance.InventoryList) {
            //find the gameobject for each image
            GameObject anObject = GameObject.Find("Image" + whichOne);

            //set texture to found sprite
            tex = item.itemIcon;

            //create sprite with texture
            mySprite = Sprite.Create(tex, new Rect(0,0,64,64), new Vector2(0.5f, 0.5f));

            //sets the objects sprite to the found sprite
            anObject.GetComponent<Image>().sprite = mySprite;

            //counter
            ++whichOne;
        }
    }

    public void ClearTheHUD() {
        //Clear out the hud
        string imgName;
        GameObject invGameObj;
        Image myImageComponent;


        for(int k = 1; k < 11; k++) {
            imgName = "Image" + k.ToString();
            invGameObj = GameObject.Find(imgName);
            myImageComponent = invGameObj.GetComponent<Image>();
            myImageComponent.sprite = null;
        }
    }

    public void LoadTheHUD() {
        int imgNumber = 1;
        Image myImageComponent;

        foreach(Item anItem in GameManager.instance.InventoryList) {
            //get image name for position
            string theImageName;
            theImageName = "Image" + imgNumber.ToString();

            GameObject anObject = GameObject.Find(theImageName);
            myImageComponent = anObject.GetComponent<Image>();

            //build the Sprite
            Texture2D tex = anItem.itemIcon;
            Sprite mySprite = Sprite.Create(tex, new Rect(0,0,64,64), new Vector2(0.5f, 0.5f));

            //post the sprite to the image object
            myImageComponent.sprite = mySprite;

            //counter
            imgNumber++;
        }
    }
    
}