using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    
    public int theOne;
    public GameObject invGameObject;

    void OnTriggerEnter(Collider collided)
    {
        //if statement to check if item is a key object
        if (collided.gameObject.tag == "KeyObject")
        {
            //makes object that the player collided with go poof
            Debug.Log("Collided with Key Object: " + collided.GetComponent<Collider>());
            collided.gameObject.SetActive(false);

            //save obj to work
            invGameObject = collided.gameObject;

            //add item to inventory list
            //check if item exists as item, saves spot on list
            FindinItemList();
            if (theOne >= 0) { //if item exists, add to inventory
                GameManager.instance.InventoryList.Add(GameManager.instance.theItems[theOne]);
                Debug.Log("Found and added to list: " + GameManager.instance.theItems[theOne].itemDesc);
            }
        }
        //tests for floor collision to handle gravity resetting
        /*
        else if (collided.gameObject.tag == "Floor"){
            Debug.Log("Landed on Floor: " + collided.GetComponent<Collider>());
            gravity = -1f;
            airborne = false;
        } */
    }

    private void FindinItemList() {
        for (int I=0; I<GameManager.instance.theItems.Count; ++I) {
            if (GameManager.instance.theItems[I].itemName == invGameObject.name) {
                theOne = I;
                break;
            }
        }
    }
}
