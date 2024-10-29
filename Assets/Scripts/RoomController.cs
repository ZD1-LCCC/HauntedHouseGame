using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class RoomController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //load text method for accesability?
        LoadText();

    }

    public void LoadText() {
        //to get scene name and other info?
        string sceneName = SceneManager.GetActiveScene().name;

        string sceneNumber = sceneName.Substring(sceneName.Length-1);
        Debug.Log("Room number is " + sceneNumber);

        int sceneNumb;
        sceneNumb = int.Parse(sceneNumber);

        //figure out the text
        string myText = "";

        //actually changes the text
        //GameObject.Find("RoomText").GetComponent<TextMeshProUGUI>().text = myText;

        //looks into game manager to find the room info array and output the string for the text
        GameObject.Find("RoomText").GetComponent<TextMeshProUGUI>().text = GameManager.instance.roomInfo[sceneNumb-1,0];
        
    }
}
