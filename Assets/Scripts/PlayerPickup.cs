using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerPickup : MonoBehaviour
{
    public GameObject LoseHUD;
    public int theOne;
    public GameObject invGameObject;
    public string itemName;
    public string roomDestination;

    //holds the index of the burnable item
    public int itemIndex = -1;

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (GameManager.instance.selectedObject != null) {
                if (GameManager.instance.selectedObject.gameObject.tag == "KeyObject" || GameManager.instance.selectedObject.gameObject.tag == "Diggable") {
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
                        GameManager.instance.selectedObject = null;
                        //moves player to room
                        MoveToRoom(scene, door);
                    }

                    //else if door is locked
                    else {
                        //changes room text if locked without key, unlocks door if you have key, and moves you if unlocked
                        LockedDoorTest(scene, door);
                    }
                }
                else if (GameManager.instance.selectedObject.gameObject.tag == "Locked") {
                    //unlock door by removing it and replacing it
                    GameManager.instance.interactableArray[2][0] = true;
                    Destroy(GameManager.instance.selectedObject);
                    GameManager.instance.selectedObject = null;
                    GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "";
                    GameManager.instance.CheckPuzzle(2);
                }
                else if (GameManager.instance.selectedObject.gameObject.tag == "NPC") {
                    foreach (Item item in GameManager.instance.InventoryList) {
                        if (item == GameManager.instance.theItems[16]) {
                            GameManager.instance.CheckPuzzle(4);
                            GameObject.Find("RoomText").GetComponent<TextMeshProUGUI>().text = "You should be at the win screen by now! Congrats on this sequence break!";
                            break;
                        }
                        else {
                            GameObject.Find("RoomText").GetComponent<TextMeshProUGUI>().text = "Me and [ghostname] have fallen in love. The only thing we need to make it official is a wedding ring. I don’t want to leave her side, can you find one for me?";
                        }
                    }
                }
                else if (GameManager.instance.selectedObject.gameObject.tag == "Fireplace") {
                    BurnItem();
                    switch (GameManager.instance.InventoryList[itemIndex].itemId) {
                        case 3:
                            GameManager.instance.InventoryList[itemIndex] = GameManager.instance.theItems[17];
                            RoomController.ClearTheHUD();
                            RoomController.InitInventory();
                            if (FindBurnable() == true) {
                                GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Burn: " + GameManager.instance.InventoryList[itemIndex].displayName;
                            }
                            else {
                                GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Nothing to Burn";
                            }
                            break;
                        case 4:
                            GameManager.instance.InventoryList[itemIndex] = GameManager.instance.theItems[18];
                            RoomController.ClearTheHUD();
                            RoomController.InitInventory();
                            if (FindBurnable() == true) {
                                GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Burn: " + GameManager.instance.InventoryList[itemIndex].displayName;
                            }
                            else {
                                GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Nothing to Burn";
                            }
                            break;
                        case 5:
                            GameManager.instance.InventoryList[itemIndex] = GameManager.instance.theItems[16];
                            RoomController.ClearTheHUD();
                            RoomController.InitInventory();
                            if (FindBurnable() == true) {
                                GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Burn: " + GameManager.instance.InventoryList[itemIndex].displayName;
                            }
                            else {
                                GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Nothing to Burn";
                            }
                            break;
                        default:
                            Debug.Log("Unknown item + " + GameManager.instance.InventoryList[itemIndex].displayName);
                            break;
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
            Debug.Log("Collided with Key Object: " + collided.name);

            FindinItemList(collided.name);
            itemName = GameManager.instance.theItems[theOne].displayName;
            GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Pick Up: " + itemName;
            
        }
        else if (collided.gameObject.tag == "Door") {
            int doorNum;
            //sets collided object to the gamemanager's selected object
            GameManager.instance.selectedObject = collided.gameObject;
            Debug.Log("Collided with Door: " + collided.name);

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
        else if (collided.gameObject.tag == "Diggable") {
            Debug.Log("Collided with Grave Dig Spot: " + collided.name);
            foreach (Item item in GameManager.instance.InventoryList) {
                //if you have shovel in inventory
                if (item == GameManager.instance.theItems[6]) {
                    GameManager.instance.selectedObject = collided.gameObject;
                    //just using "Grave" for the name cause they are supposed to be similar
                    GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Dig: Grave";
                    break;
                }
            }
        }
        else if (collided.gameObject.tag == "Locked") {
            Debug.Log("Collided with Locked Door: " + collided.name);
            foreach (Item item in GameManager.instance.InventoryList) {
                if (item == GameManager.instance.theItems[1]) {
                    GameManager.instance.selectedObject = collided.gameObject;
                    //just using "Grave" for the name cause they are supposed to be similar
                    GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Unlock";
                    break;
                }
                else {
                    GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Locked";
                }
            }
            if (GameManager.instance.InventoryList.Count == 0) {
                GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Locked";
            }
        }
        else if (collided.gameObject.tag == "NPC") {
            GameManager.instance.selectedObject = collided.gameObject;
            Debug.Log("Collided with NPC: " + collided.name);
            GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Talk";
        }
        else if (collided.gameObject.tag == "Fireplace") {
            Debug.Log("Collided with Fireplace");
            if (FindBurnable() == true) {
                GameManager.instance.selectedObject = collided.gameObject;
                GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Burn: " + GameManager.instance.InventoryList[itemIndex].displayName;
            }
            else {
                GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "[E] Nothing to Burn";
            }
        }
    }

    void OnTriggerExit(Collider collided) {
        if (collided.gameObject.tag == "KeyObject") {
            GameManager.instance.selectedObject = null;
            Debug.Log("Left collision with Key Object: " + collided.name);
            GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "";
        }
        else if (collided.gameObject.tag == "Door") {
            GameManager.instance.selectedObject = null;
            Debug.Log("Left collision with Door: " + collided.name);
            GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "";
        }
        else if (collided.gameObject.tag == "Diggable") {
            GameManager.instance.selectedObject = null;
            Debug.Log("Left collision with Grave Dig Spot: " + collided.name);
            GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "";
        }
        else if (collided.gameObject.tag == "Locked") {
            GameManager.instance.selectedObject = null;
            Debug.Log("Left collision with Locked Door: " + collided.name);
            GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "";
        }
        else if (collided.gameObject.tag == "NPC") {
            GameManager.instance.selectedObject = null;
            Debug.Log("Left collision with NPC: " + collided.name);
            GameObject.Find("ItemPrompt").GetComponent<TextMeshProUGUI>().text = "";
        }
        else if (collided.gameObject.tag == "Fireplace") {
            GameManager.instance.selectedObject = null;
            itemIndex = -1;
            Debug.Log("Left collision with Fireplace");
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
        //carry amount check, to lose game
        if (!haveIt) {
            if (GameManager.instance.InventoryList.Count >= 6) {
                //detach camera from player, delete player, add HUD to "lose" the game
                Debug.Log("You lost the game, why would you do that? You broke your back trying to carry everything");
                Transform camera2 = GameObject.Find("Player Camera").GetComponent<Transform>();
                GameObject player = GameObject.Find("Player(Clone)");
                camera2.parent = null;
                Destroy(player);
                Instantiate(LoseHUD);

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
        if (GameManager.instance.CompletedPuzzles() == false) {
            //moves to next scene ONLY IF the player didn't beat the game since it would move you from the win screen to the scene if you beat the game during transition
            SceneManager.LoadScene(nextScene);
        }
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
                        GameManager.instance.selectedObject = null;
                        //unlocking this door solves the first puzzle
                        GameManager.instance.CheckPuzzle(1);
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
                    if (item == GameManager.instance.theItems[15]) {
                        GameManager.instance.lockedDoorArray[scene, door] = false;
                        GameManager.instance.doorFrom = scene;
                        GameManager.instance.selectedObject = null;
                        GameManager.instance.CheckPuzzle(3);
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

    private bool FindBurnable() {
        foreach (Item item in GameManager.instance.InventoryList) {
            if ((item == GameManager.instance.theItems[3]) || (item == GameManager.instance.theItems[4]) || (item == GameManager.instance.theItems[5])) {
                itemIndex = GameManager.instance.InventoryList.IndexOf(item);
                Debug.Log("true");
                return true;
            }
        }
        Debug.Log("false");
        return false;
    }

    private void BurnItem() {

    }
}