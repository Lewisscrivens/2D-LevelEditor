using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorInput : MonoBehaviour {

    public static LevelEditorInput instance = null;

    // Variables for mouse position and speed.
    private float movementSpeed;
    private float inputX;
    private float inputY;
    private bool canMove;
    private Vector2 movement;

    [SerializeField]
    private Camera levelEditorCamera;

    //Creates LevelEditorInput instance
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        canMove = false;
        movementSpeed = 0.5f;// Variable to control camera movement speed.
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Mouse X");
        inputY = Input.GetAxisRaw("Mouse Y");

        if (Input.GetButton("Mouse ScrollWheel"))// Checks if the right mouse button is held down.
        {
            canMove = true;
        }
        else if (canMove)// If the mouse button isnt being pressed and can move is still true then set to false and remove any movement.
        {
            canMove = false;
            movement = new Vector2(0, 0);
        }

        if (Input.GetButtonDown("Reset Camera"))// If the Reset Cama button is pressed then reset the camera.
        {
            LevelEditor.instance.ResetCamPos();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)// If the scrollwheel is scrolled up then zoomOut in LevelEditor.
        {
            LevelEditor.instance.ZoomOut();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)// If the scrollwheel is scrolled down then zoomOut in LevelEditor.
        {
            LevelEditor.instance.ZoomIn();
        }

        if (Input.GetButton("Place") && !MouseOverUI.instance.getUIOver())// Check if mouse is over the UI to prevent accidently placing tiles.
        {
            LevelEditor.instance.placeTile();
        }
        else if (Input.GetButton("Destroy") && !MouseOverUI.instance.getUIOver())// Check if mouse is over the UI to prevent accidently placing tiles.
        {
            LevelEditor.instance.destroyTile();
        }
    }

    void FixedUpdate()
    {
        // If can move then add mouse X and
        if (canMove == true)
        {
            movement = new Vector2(inputX, inputY);
        }

        levelEditorCamera.transform.Translate(-movement * movementSpeed);// Set movement to negative so that the movement is click and drag.
    }
}
