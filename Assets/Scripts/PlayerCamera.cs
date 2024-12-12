using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	public float xSens;
	public float ySens;

	public Transform orientation;

	float xRot;
	float yRot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
	Cursor.visible = false;
    }

    private void Update()
    {
        //getting mouse input
	float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSens;
	float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * ySens;

	yRot += mouseX;
	xRot -= mouseY;
	xRot = Mathf.Clamp(xRot, -90f, 90f);

	//rotation and orientation
	transform.rotation = Quaternion.Euler(xRot, yRot, 0);	
	orientation.rotation = Quaternion.Euler(0, yRot, 0);	

    }
}
