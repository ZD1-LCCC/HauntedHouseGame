using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Item
{
    //readable name for item
    public string itemName;
    //numeric id for item
    public int itemId;
    //description of item
    public string itemDesc;
    //if the item is important for puzzle or not? useless?
    public bool puzzItem;
    //icon for inventory
    public Texture2D itemIcon;
    //id of object intended to be used with
    public int itemUseWithId;

    //constructor
    public Item(string name, int id, string desc) {
        itemName = name;
        itemId = id;
        itemDesc = desc;
        //uses the file path and finds name to apply image to itemIcon
        itemIcon = Resources.Load<Texture2D>("SampleSprites/" + name);
    }
}