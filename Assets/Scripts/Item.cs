using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Item
{
    //name for item
    public string itemName;
    //numeric id for item
    public int itemId;
    //display name of item, for readability
    public string displayName;
    //description of item
    public string itemDesc;
    //icon for inventory
    public Texture2D itemIcon;


    //constructor
    public Item(string name, int id, string display, string desc) {
        itemName = name;
        //roomId = room;
        itemId = id;
        displayName = display;
        itemDesc = desc;
        //uses the file path and finds name to apply image to itemIcon
        itemIcon = Resources.Load<Texture2D>("SampleSprites/" + name);
    }
}
