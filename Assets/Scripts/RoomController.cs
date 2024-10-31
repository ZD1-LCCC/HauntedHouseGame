using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class RoomController : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    private void Start()
    {
        //load text method for accesability?
        LoadText();
        gameManager = GameManager.instance;
    }



    public void LoadText() {
        //to get scene name and other info?
        string sceneName = SceneManager.GetActiveScene().name;

        int textNumb = 0;

        string sceneNumber = sceneName.Substring(sceneName.Length-1);
        Debug.Log("Room number is " + sceneNumber);

        int sceneNumb;
        sceneNumb = int.Parse(sceneNumber);

        //to determine what level of text to show
        if (GameManager.instance.numberEnter[sceneNumb-1] == 0) {
            textNumb = 0;
        }
        else if (GameManager.instance.numberEnter[sceneNumb-1] > 0) {
            textNumb = 1;
        }
        else {
            textNumb = 1;
        }
        //increment the number of times a room was entered
        ++GameManager.instance.numberEnter[sceneNumb-1];
        
        //figure out the text
        string myText = GameManager.instance.roomInfo[sceneNumb-1,textNumb];
        

        //actually changes the text
        GameObject.Find("RoomText").GetComponent<TextMeshProUGUI>().text = myText;

        //looks into game manager to find the room info array and output the string for the text
        //GameObject.Find("RoomText").GetComponent<TextMeshProUGUI>().text = GameManager.instance.roomInfo[sceneNumb-1,0];

        
    }
}
