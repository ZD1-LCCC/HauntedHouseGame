using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerPickup : MonoBehaviour
{
    
    public int theOne;
    public GameObject invGameObject;
    public string itemName;
    public string roomDestination;


    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (GameManager.instance.selectedObject != null) {
                if (GameManager.instance.selectedObject.gameObject.tag == "KeyObject") {
                    //saves object while working
                    invGameObject = GameManager.instance.selectedObject;

                    //disables object and resets item prompt
                    GameManager.instance.selectedObject.SetActive(false);
                    GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "";
                    GameManager.instance.selectedObject = null;

                    //check if item exists as item, saves spot on list
                    FindinItemList(invGameObject.name);
                    if (theOne >= 0) { //if item exists, add to inventory
                        PickItUp(); //add to inventory list if it meets criteria
                        ShowInHUD();
                    }
                }
                else if (GameManager.instance.selectedObject.gameObject.tag == "Door") {
                    //find the scene number
                    int scene = int.Parse(SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length-1)) - 1;
                    //find door number
                    int door = DoorNumberConvert(GameManager.instance.selectedObject.gameObject.name);
                    //if door isn't locked
                    if (GameManager.instance.lockedDoorArray[scene, door] == false) {
                        //sets the scene that the player entered from for finding the correct spawn position
                        GameManager.instance.doorFrom = scene;
                        //moves player to room
                        MoveToRoom(scene, door);
                    }

                    //else if door is locked
                    else {
                        //changes room text if locked without key, unlocks door if you have key, and moves you if unlocked
                        LockedDoorTest(scene, door);
                    }
                }
            }
        }
    }

    void OnTriggerEnter(Collider collided)
    {
        //if statement to check if item is a key object
        if (collided.gameObject.tag == "KeyObject")
        {
            //sets collided object to the gamemanager's selected object
            GameManager.instance.selectedObject = collided.gameObject;
            Debug.Log("Collided with Key Object: " + collided.GetComponent<Collider>());

            FindinItemList(collided.name);
            itemName = GameManager.instance.theItems[theOne].displayName;
            GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Pick Up: " + itemName;
            
        }
        else if (collided.gameObject.tag == "Door") {
            int doorNum;
            //sets collided object to the gamemanager's selected object
            GameManager.instance.selectedObject = collided.gameObject;
            Debug.Log("Collided with Door: " + collided.GetComponent<Collider>());

            //find name of destination, first find room interacted with
            doorNum = DoorNumberConvert(collided.gameObject.name);

            //then current room
            int sceneCurrent = int.Parse(SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length-1)) - 1;

            //use array to find destination name
            roomDestination = GameManager.instance.roomNameArray[sceneCurrent, doorNum];
            //FindinItemList(collided.name);
            //itemName = GameManager.instance.theItems[theOne].displayName;
            GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Open: " + roomDestination;
        }
    }

    void OnTriggerExit(Collider collided) {
        if (collided.gameObject.tag == "KeyObject") {
            GameManager.instance.selectedObject = null;
            Debug.Log("Left collision with Key Object: " + collided.GetComponent<Collider>());
            GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "";
        }
        else if (collided.gameObject.tag == "Door") {
            GameManager.instance.selectedObject = null;
            Debug.Log("Left collision with Door: " + collided.GetComponent<Collider>());
            GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    private void FindinItemList(string name) {
        for (int I=0; I<GameManager.instance.theItems.Count; ++I) {
            if (GameManager.instance.theItems[I].itemName == name) {
                theOne = I;
                break;
            }
        }
    }

    private void PickItUp() {
        //duplicate pickup check
        bool haveIt = false;
        foreach (Item item in GameManager.instance.InventoryList) {
            if (item.itemName == invGameObject.name) {
                haveIt = true;
                break;
            }
            else {
                
            }
        }
        //carry amount check
        if (!haveIt) {
            if (GameManager.instance.InventoryList.Count >= 10) {
                Debug.Log("You lost the game, why would you do that? You broke your back trying to carry everything");
            }
            else {
                GameManager.instance.InventoryList.Add(GameManager.instance.theItems[theOne]);
                Debug.Log("Found and added to list: " + GameManager.instance.theItems[theOne].itemDesc);
            }
        }
    }

    private void ShowInHUD() {
        Debug.Log("Attempting to find item sprite...");
        Texture2D tex;
        Sprite mySprite;
        string theName;
        int whichOne;
        
        //get number for the newest inventory item
        whichOne = GameManager.instance.InventoryList.Count;

        //find name of hud image to paste it
        theName = "Image" + whichOne;

        Debug.Log("Looking for " + theName);
        GameObject anObject = GameObject.Find(theName);

        //get texture from the list
        tex = GameManager.instance.InventoryList[whichOne - 1].itemIcon;

        //create the Sprite
        mySprite = Sprite.Create(tex, new Rect(0,0,64,64), new Vector2(0.5f, 0.5f));

        //put new sprite in appropriate block
        anObject.GetComponent<Image>().sprite = mySprite;
    }

    private int DoorNumberConvert(string name) {
        switch(name) {
            case "NorthDoor":
                return 0;
            case "EastDoor":
                return 1;
            case "SouthDoor":
                return 2;
            case "WestDoor":
                return 3;
            default:
                Debug.Log("Door Invalid, " + name);
                return 0;
        }
    }

    public void MoveToRoom(int scene, int door) {
        //convert to use as string name
        string NumToStr = GameManager.instance.roomGridArray[scene, door].ToString();
        string nextScene = "Room#";
        nextScene = nextScene + NumToStr;
        //moves to next scene
        SceneManager.LoadScene(nextScene);
    }

    public void LockedDoorTest(int scene, int door) {
        string lockedText = "Error";
        if (door == 0) {
            lockedText = "Locked. I think I forgot the key in my car.";
            //doesn't work without an item in inventory
            if (GameManager.instance.InventoryList.Count == 0) {
                GameObject.Find("RoomText").GetComponent<TextMeshProUGUI>().text = lockedText;
            }
            else {
                foreach(Item item in GameManager.instance.InventoryList) {
                    if (item == GameManager.instance.theItems[0]) {
                        GameManager.instance.lockedDoorArray[scene, door] = false;
                        GameManager.instance.doorFrom = scene;
                        MoveToRoom(scene, door);
                        break;
                    }
                    GameObject.Find("RoomText").GetComponent<TextMeshProUGUI>().text = lockedText;
                }
            }
        }
        else if (door == 3) {
            lockedText = "Locked.";
            if (GameManager.instance.InventoryList.Count == 0) {
                GameObject.Find("RoomText").GetComponent<TextMeshProUGUI>().text = lockedText;
            }
            else {
                foreach(Item item in GameManager.instance.InventoryList) {
                    if (item == GameManager.instance.theItems[7]) {
                        GameManager.instance.lockedDoorArray[scene, door] = false;
                        GameManager.instance.doorFrom = scene;
                        MoveToRoom(scene, door);
                        break;
                    }
                    GameObject.Find("RoomText").GetComponent<TextMeshProUGUI>().text = lockedText;
                }
            }
        }
        else {
            lockedText = "Unknown door.";
        }
        
    }
}