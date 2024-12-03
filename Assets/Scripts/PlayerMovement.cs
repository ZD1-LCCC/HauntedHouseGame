using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _cc;   

    public GameObject PauseHUD;

    public GameObject camera2;
    public float speed = 10.0f;
    const float GRAVITY = -5f;
    float gravity = -9.8f;
    float camXAxis;
    public float sensTurn = 5.0f;
    //bool airborne = false;

    // Start is called before the first frame update 
    void Start()
    {
        _cc = GetComponent<CharacterController>();
        camera2 = GameObject.Find("Player Camera");
        camera2.transform.position = gameObject.transform.position + new Vector3(0, 0.75f, 0);
        //attempt to lock cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.pauseValue == false) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                RoomController.UpdateInventorySelect(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                RoomController.UpdateInventorySelect(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                RoomController.UpdateInventorySelect(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                RoomController.UpdateInventorySelect(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                RoomController.UpdateInventorySelect(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6)) {
                RoomController.UpdateInventorySelect(5);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
                RoomController.UpdateInventorySelect(--GameManager.instance.inventorySelect);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
                RoomController.UpdateInventorySelect(++GameManager.instance.inventorySelect);
            }
            
            float deltaX = Input.GetAxis("Horizontal")*speed;
            float deltaZ = Input.GetAxis("Vertical")*speed;

            Vector3 myDirection = new Vector3(deltaX, gravity, deltaZ);

            //rotates the camera vertically with mouse
            camXAxis += Input.GetAxis("Mouse Y")*sensTurn*-1;
            //Debug.Log(this.gameObject.transform.rotation.y);
            camXAxis = Mathf.Clamp(camXAxis, -90f, 90f);
            camera2.transform.localRotation = Quaternion.Euler(camXAxis, 0f, 0f);

            //turn when mouse left and right
            transform.Rotate(0, Input.GetAxis("Mouse X")*sensTurn, 0);

            //adjust for framerate
            myDirection = myDirection * Time.deltaTime;

            //limit max speed
            myDirection = Vector3.ClampMagnitude(myDirection,speed);

            //control direction to match game speed instead of object orientation
            Vector3 myPlayerDirection = transform.TransformDirection(myDirection);

            //actually moving the player, everything before is the logic for where to move
            _cc.Move(myPlayerDirection);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameManager.instance.pauseValue == false) {
                GameManager.instance.pauseValue = true;
                Instantiate(PauseHUD, new Vector3(0,0,0), Quaternion.identity);
                Cursor.lockState = CursorLockMode.None;
            }
            else {
                MenuController.Unpause();
            }
        }
    }
}
