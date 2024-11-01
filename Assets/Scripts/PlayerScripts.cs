using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScripts : MonoBehaviour
{
    private CharacterController _cc;   

    public GameObject camera;
    public float speed = 10.0f;
    const float GRAVITY = -5f;
    float gravity = -5f;
    //float jumpForce = 500.0f;
    public float sensTurn = 5.0f;
    bool airborne = false;

    // Start is called before the first frame update
    void Start()
    {
        _cc = GetComponent<CharacterController>();
        camera = GameObject.Find("Main Camera");
        camera.transform.position = gameObject.transform.position + new Vector3(0, 0.75f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (airborne == true && gravity >= -20){
            gravity += GRAVITY * Time.deltaTime;
        }
        
        //turn when mouse left and right
        transform.Rotate(0, Input.GetAxis("Mouse X")*sensTurn, 0);

        float deltaX = Input.GetAxis("Horizontal")*speed;
        float deltaZ = Input.GetAxis("Vertical")*speed;

        Vector3 myDirection = new Vector3(deltaX, gravity, deltaZ);

        //rotates the camera vertically with mouse
        camera.transform.Rotate(Input.GetAxis("Mouse Y")*sensTurn*-1, 0, 0);
        
        //if statement for clamping vertical rotation? not working currently 
        /*
        if (camera.transform.rotation.x >= -90) {
            camera.transform.rotation.x = -90;
        }
        else if (camera.transform.rotation.x >= 180) {
            camera.transform.rotation.x = -90;
        }

        if (Input.GetButtonDown("Jump")) 
        {
            myDirection.y += jumpForce;
        }   
        */

        //adjust for framerate
        myDirection = myDirection * Time.deltaTime;

        //limit max speed
        myDirection = Vector3.ClampMagnitude(myDirection,speed);

        //control direction to match game speed instead of object orientation
        Vector3 myPlayerDirection = transform.TransformDirection(myDirection);

        //actually moving the player, everything before is the logic for where to move
        _cc.Move(myPlayerDirection);

    }
    void OnTriggerEnter(Collider collided)
    {
        //outputs collision confirmation into log
        //Debug.Log("Collision with " + collided.GetComponent<Collider>());
        if (collided.gameObject.tag == "KeyObject")
        {
            //makes object that the player collided with go poof
            Debug.Log("Collided with Key Object: " + collided.GetComponent<Collider>());
            collided.gameObject.SetActive(false);
        }
        //tests for floor collision to handle gravity resetting
        else if (collided.gameObject.tag == "Floor"){
            Debug.Log("Landed on Floor: " + collided.GetComponent<Collider>());
            gravity = -1f;
            airborne = false;
        }
    }
    //tests for leaving floor to make gravity increase
    void OnTriggerExit(Collider collided){
        if (collided.gameObject.tag == "Floor"){
            Debug.Log("Left Floor: " + collided.GetComponent<Collider>());
            gravity = -1f;
            airborne = true;
        }
    }

}
