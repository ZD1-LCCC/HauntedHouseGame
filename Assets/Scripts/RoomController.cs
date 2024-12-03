using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class RoomController : MonoBehaviour
{
    public static GameObject freezerDoorOpened;
    public static GameObject carOpened;
    public static GameObject frontDoorKey;
    public GameObject player;
    private void Start() {
        LoadText();
        ClearTheHUD(); //not really needed but why not
        InitInventory();
        DisableCollectedItems();
        //THESE SUDDENLY DONT WORK WHEN ON AWAKE() BECAUSE THEY CANNOT SEE THE GAME MANAGER FOR WHATEVER REASON SO I MOVED THEM TO START AND THEY WORK????
        SpawnPlayer();
        UpdateInventorySelect(GameManager.instance.inventorySelect);
    }

    private void Awake() {
        
    }

    public void DisableCollectedItems() {
        int currentRoom = int.Parse(SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length-1));
        //string objectName = "";
        //GameObject objectWork = null;
        //if an item is in my inventory, remove it from the scene wont work with puzzle
        foreach (Item item in GameManager.instance.InventoryList) {
            GameObject anObject = GameObject.Find(item.itemName);
            if (anObject != null) {
                    Destroy(anObject);
            }
        }

        //switch to handle different rooms having different interactions that need to be kept between rooms
        switch(currentRoom) {
            case 1:
                //swaps closed car with opened car
                if (GameManager.instance.interactableArray[0][0] == true) {
                    GameObject.Find("Car").SetActive(false);
                }
                else {
                    carOpened = GameObject.Find("CarOpened");
                    carOpened.SetActive(false);
                    //frontDoorKey = GameObject.Find("FrontDoorKey");
                    //frontDoorKey.SetActive(false);
                }
                Debug.Log("Room#1 INIT Completed!");
                break;
            case 2:
                Debug.Log("Room#2 INIT Completed!");
                break;
            case 3:
                //need to remove door and replace with opened door
                if (GameManager.instance.interactableArray[2][0] == true) {
                    //deletes locked door, leaving the opened door
                    GameObject.Find("FreezerDoorLocked").SetActive(false);
                }
                else {
                    //deletes opened door, leaving locked door
                    freezerDoorOpened = GameObject.Find("FreezerDoorOpened");
                    freezerDoorOpened.SetActive(false);
                }
                Debug.Log("Room#3 INIT Completed!");
                break;
            case 4: 
                Debug.Log("Room#4 INIT Completed!");
                break;
            case 5:
                Debug.Log("Room#5 INIT Completed!");
                break;
            case 6:
                Debug.Log("Room#6 INIT Completed!");
                break;
            default:
                Debug.Log("Unknown room: " + currentRoom.ToString());
                break;
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

    

    private void SpawnPlayer() {
        //find scene number
        int sceneCurrent = int.Parse(SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length-1)) - 1;

        GameObject spawn = null;

        //spawn point object reference
        if (GameManager.instance.doorFrom >= 0 && GameManager.instance.doorFrom < 6) {
            spawn = GameObject.Find("SpawnPoint" + GameManager.instance.roomConnectorSpawns[sceneCurrent,(GameManager.instance.doorFrom)].ToString());
            Debug.Log(GameManager.instance.doorFrom);
            if (spawn != null)
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

    public static void InitInventory() {
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

    public static void ClearTheHUD() {
        //Clear out the hud
        string imgName;
        GameObject invGameObj;
        Image myImageComponent;


        for(int k = 1; k < 7; k++) {
            imgName = "Image" + k.ToString();
            invGameObj = GameObject.Find(imgName);
            myImageComponent = invGameObj.GetComponent<Image>();
            myImageComponent.sprite = null;
        }
    }

        //increases or decreases the inventory selection by i,
    public static void UpdateInventorySelect(int i) {
        int holding;
        string imgName;
        GameObject imgObj;
        //if i was decreased below 0, wrap around to 5
        if (i < 0) {
            GameManager.instance.inventorySelect = 5;
        }
        //if i was increased above 5, wrap around to 0
        else if (i >= 6) {
            GameManager.instance.inventorySelect = 0;
        }
        //if above 0 but below 6, it is valid and can be used as a value
        else {
            GameManager.instance.inventorySelect = i;
        }
        //changes image color to default for all
        for (int x = 1; x < 7; ++x) {
            imgName = "Image" + x.ToString();
            imgObj = GameObject.Find(imgName);
            imgObj.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        //changes image color for selected item
        holding = GameManager.instance.inventorySelect + 1;
        imgName = "Image" + holding.ToString();
        imgObj = GameObject.Find(imgName);
        imgObj.GetComponent<Image>().color = new Color32(255, 255, 255, 125);

        //changes the text to match the item's display name
        if (GameManager.instance.InventoryList.Count > GameManager.instance.inventorySelect) {
            GameObject.Find("ItemName").GetComponent<TextMeshProUGUI>().text = GameManager.instance.InventoryList[GameManager.instance.inventorySelect].displayName;
        }
        else {
            GameObject.Find("ItemName").GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}