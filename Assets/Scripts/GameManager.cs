using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    //public bool[,] objectStatus = {0};

    public int[] numberEnter = {0,0,0,0,0,0};

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
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

}
