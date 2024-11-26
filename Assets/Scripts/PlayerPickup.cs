using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPickup : MonoBehaviour
{
    
    public int theOne;
    public GameObject invGameObject;
    public string itemName;


    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (GameManager.instance.selectedObject != null) {
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
    }

    void OnTriggerExit(Collider collided) {
        if (collided.gameObject.tag == "KeyObject") {
            GameManager.instance.selectedObject = null;
            Debug.Log("Left collision with Key Object: " + collided.GetComponent<Collider>());
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
}
