using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string[,] roomInfo = {
        {"Room#1 Text#1", "Room#1 Text#2"},
        {"Room#2 Text#1", "Room#2 Text#2"},
        {"Room#3 Text#1", "Room#3 Text#2"},
        {"Room#4 Text#1", "Room#4 Text#2"},
        {"Room#5 Text#1", "Room#5 Text#2"},
        {"Room#6 Text#1", "Room#6 Text#2"}
    };


    public static GameManager instance;

    void Awake() {
        if (instance == null){
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


}
