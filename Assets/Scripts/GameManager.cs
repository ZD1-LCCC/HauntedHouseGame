using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    public GameObject selectedObject = null;

    //list to record inventory
    public List<Item> InventoryList = new List<Item>();
    //lits for all possible items
    public List<Item> theItems = new List<Item>();

    //int to store where the the player came from
    public int doorFrom = 0;

    //int array to handle what room has what spawn number: [room in currently, room entered from]
    public int[,] roomConnectorSpawns = {
        {0,1,0,0,2,3},
        {1,0,2,3,0,0},
        {0,1,0,0,0,0},
        {0,1,0,0,0,0},
        {1,0,0,0,0,0},
        {1,0,0,0,0,0}
    };

    //array for puzzle items needed for each room?
    public string [][] roomNeedsArray = new string[][]
        {
            new string [1] {"FrontDoorKey"},                //room1 items needed to complete, check for items to see if puzzle is solved
            new string [1] {"StolenObject"},                //room2 items needed to complete
            new string [1] {"KitchenFreezerKey"},           //room3 items needed to complete
            new string [1] {"RingBook"},                    //room4 items needed to complete, need actual ring
            new string [1] {"RingBook"},      //room5 items needed to complete, need actual ring
            new string [1] {"BasementKey"}                       //room6 items needed to complete
        };
    //variables to track how many puzzles are solved
    public static int puzzlesSolved = 0;
    //indices matching the needed items to the inventory list
    public List<int> IndicesOfItemsFound = new List<int>();
    public List<Item> InventoryUsed = new List<Item>();

    //array of sprite images for items
    public Sprite[] theSprites;
    //tell the program where to find the sprite images
    public string filePath = "SampleSprites";

    //how many times each room was entered
    public int[] numberEnter = {0,0,0,0,0,0};

    //strings for room text
    public string[,] roomInfo = {
        {"So this is where he disappeared to? Doesn’t look too haunted.","The trees are so dense, it’s like you are walled in.","Not yet"},
        {"Is that a wall made of ghosts?","Seems like the only room I can access is the kitchen.","Not yet"},
        {"An entire walk-in freezer, they must’ve been rich.","Starting to get hungry","Not yet"},
        {"Anything good to read in here?","I'm surprised that the fireplace is working.","Not yet"},
        {"What was that? Is someone down here?","I hope that banging sound wasn’t coming from down here.","Not yet"},
        {"A graveyard next to a house? That is not a good sign.","The silence is deafening.","Not yet"}
    };
    
    void Awake() {
        //to make one instance of the object and make it consistant between scenes
        if (instance == null) {
            instance = this;
            LoadItems();
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    //method for...
    private void LoadItems() { //loads the sprites and item info for all items
        //loads the sprites
        theSprites = Resources.LoadAll<Sprite>(filePath);
        Debug.Log(theSprites.Length + " sprites have been loaded");

        //creates objects using Item class, need one for each item in game.
        theItems.Add(new Item("FrontDoorKey", 0, "Front Door Key", "a key for the Front Door."));
        theItems.Add(new Item("KitchenFreezerKey", 1, "Kitchen Freezer Key", "a key for the Kitchen Freezer."));
        theItems.Add(new Item("StolenObject", 2, "Stolen Object", "a stolen object.")); //need to know what the stolen object is?
        theItems.Add(new Item("FishBook", 3, "Fisherman's Guide: Vol 1", "a book, smells fishy."));
        theItems.Add(new Item("FishBook2", 4, "Fisherman's Guide: Vol 2", "a book, smells fishy"));
        theItems.Add(new Item("RingBook", 5, "Widow's Diary", "a book that contains a ring."));
        theItems.Add(new Item("Shovel", 6, "Shovel", "a shovel."));
        theItems.Add(new Item("BasementKey", 7, "Basement Key", "a key for the basement."));

        //assigns sprites to each item
        foreach(Item anItem in theItems) {
            anItem.itemIcon = Resources.Load<Texture2D>("SampleSprites/" + anItem.itemName);
        }
    }
}
