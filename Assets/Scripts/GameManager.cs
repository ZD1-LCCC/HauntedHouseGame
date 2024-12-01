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
    public int doorFrom = -1;

    //handles room connections
    public int[,] roomGridArray = {
        {2,6,0,5},
        {0,3,1,4},
        {2,0,0,0},
        {0,0,2,0},
        {0,0,0,1},
        {0,0,0,1}
    };

    //parallel array for locked doors (North, East, South, West)
    public bool[,] lockedDoorArray = {
        {true, false, false, true},
        {false, false, false, false},
        {false, false, false, false},
        {false, false, false, false},
        {false, false, false, false},
        {false, false, false, false}
    };

    //array for destination names
    public string[,] roomNameArray = {
        {"Foyer", "Graveyard", "", "Basement"},
        {"", "Kitchen", "Forest", "Library"},
        {"Foyer", "", "", ""},
        {"", "", "Foyer", ""},
        {"", "" , "", "Forest"},
        {"", "" , "", "Forest"}
    };

    //int array to handle what room has what spawn number: [room in currently, room entered from]
    public int[,] roomConnectorSpawns = {
        {0,1,0,0,2,3},
        {1,0,2,3,0,0},
        {0,1,0,0,0,0},
        {0,1,0,0,0,0},
        {1,0,0,0,0,0},
        {1,0,0,0,0,0}
    };

    //bool array to store if item has been interacted with yet
    public bool [][] interactableArray = new bool[][] {
        new bool [1] {false}, //room1: car door
        new bool [0] {}, //room2: nothing
        new bool [1] {false}, //room3: kitchen freezer door
        new bool [1] {false}, //room4: fireplace interaction spot
        new bool [0] {}, //room5: nothing?
        new bool [0] {}  //room6: nothing
    };

    //displayname for the interactables
    /*
    public string [][] interactableDisplayNames = new string[][] {
        new string [1] {false}, //room1: car door
        new string [0] {}, //room2: nothing
        new string [1] {false}, //room3: kitchen freezer door
        new string [1] {false}, //room4: fireplace interaction spot
        new string [0] {}, //room5: nothing?
        new string [9] {false, false, false, false, false, false, false, false, false}  //room6: graves 1-8, key grave
    };  */

    //array for puzzle items needed for each room? currently making obsolete
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
    //integer for hopw many have been completed, 4 in total?
    public int puzzlesSolved = 0;
    //array to store which puzzle was completed
    public bool[] puzzlesList = {false, false, false, false};

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
        theItems.Add(new Item("FrontDoorKey", "FrontDoorKey", 0, "Front Door Key", "a key for the Front Door."));
        theItems.Add(new Item("KitchenFreezerKey", "KitchenFreezerKey", 1, "Kitchen Freezer Key", "a key for the Kitchen Freezer."));
        theItems.Add(new Item("StolenObject", "StolenObject", 2, "Stolen Object", "a stolen object.")); //need to know what the stolen object is?
        theItems.Add(new Item("FishBook", "FishBook", 3, "Fisherman's Guide: Vol 1", "a book, smells fishy."));
        theItems.Add(new Item("FishBook2", "FishBook", 4, "Fisherman's Guide: Vol 2", "a book, smells fishy"));
        theItems.Add(new Item("RingBook", "RingBook", 5, "Widow's Diary", "a book that contains a ring."));
        theItems.Add(new Item("Shovel", "Shovel", 6, "Shovel", "a shovel."));
        theItems.Add(new Item("GraveDirt1", "GraveDirt", 7, "Dirt", "a pile of dirt from a grave"));
        theItems.Add(new Item("GraveDirt2", "GraveDirt", 8, "Dirt", "a pile of dirt from a grave"));
        theItems.Add(new Item("GraveDirt3", "GraveDirt", 9, "Dirt", "a pile of dirt from a grave"));
        theItems.Add(new Item("GraveDirt4", "GraveDirt", 10, "Dirt", "a pile of dirt from a grave"));
        theItems.Add(new Item("GraveDirt5", "GraveDirt", 11, "Dirt", "a pile of dirt from a grave"));
        theItems.Add(new Item("GraveDirt6", "GraveDirt", 12, "Dirt", "a pile of dirt from a grave"));
        theItems.Add(new Item("GraveDirt7", "GraveDirt", 13, "Dirt", "a pile of dirt from a grave"));
        theItems.Add(new Item("GraveDirt8", "GraveDirt", 14, "Dirt", "a pile of dirt from a grave"));
        theItems.Add(new Item("GraveDirt9", "BasementKey", 15, "Basement Key", "a key for the basement."));
        theItems.Add(new Item("RingBook", "WeddingRing", 16, "Wedding Ring", "a key for the basement."));
        theItems.Add(new Item("FishBook", "RedHerring", 17, "Red Herring", "I think I might have done something wrong."));
        theItems.Add(new Item("FishBook2", "RedHerring", 18, "Red Herring", "I think I might have done something wrong."));

    }

    public void CheckPuzzle(int puzz) {
        if (puzzlesList[puzz - 1] == false) {
            ++puzzlesSolved;
            puzzlesList[puzz - 1] = true;
            Debug.Log("Puzzle solved! " + puzz);
        }
        else
            Debug.Log("Puzzle already complete " + puzz.ToString());

        if (puzzlesSolved == 4) {
            Debug.Log("Good Job! You beat the game!");
            SceneManager.LoadScene("WinScene");
        }
    }
    public bool CompletedPuzzles() {
        if (puzzlesSolved == 4) 
            return true;
        else
            return false;
    }

    /* old version of it
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
        */
}
