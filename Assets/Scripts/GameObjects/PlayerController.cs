using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rigidBody;
    private Animator animator;
    private float speed = 1f;
    private float jumpHeight = 6;
    private float movementX;
    private float movementY;
    private Vector2 movement;
    private float gravMultiplier = 2.5f;
    private float lowJumpGravMultiplier = 1.5f;
    private bool jumpAllowed = true;
    private bool isDead = false;
    private bool ignoreChainExit = false;
    private bool isJumping;
    private bool isRunning;
    private bool ropeMovement = false;

    private Vector3 spawnPos;


    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Escape"))
        {
            LevelEditor.instance.editorMode();
            Destroy(this.gameObject);
        }

        if (Input.GetButton("Respawn"))
        {
            LevelEditor.instance.PlayLevel();
            Destroy(this.gameObject);
        }

        if (rigidBody.velocity.y == 0)
        {
            jumpAllowed = true;
            isJumping = false;
        }
        else
        {
            jumpAllowed = false;
            isJumping = true;
        }
        if (movementX != 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        // Checking for the jump input which then runs the jump function. Also stops player from jumping if dead.
        if (Input.GetButton("Jump") && (jumpAllowed == true || ropeMovement) && isDead == false)
        {
            jump();
        }

        // Animation Controller
        animator.SetBool("Running?", isRunning);
        animator.SetBool("Jumping?", isJumping);
        animator.SetBool("Dead?", isDead);
    }

    // Using fixed update for movement avoids the rigid body 2d glitching on colliders
    void FixedUpdate()
    {
        if (isDead == false)// If the character is dead they cannot move
        {
            movementX = Input.GetAxisRaw("Horizontal");

            if (ropeMovement == true)
            {
                movementY = Input.GetAxisRaw("Vertical");
            }
            else
            {
                movementY = 0;
            }
            movement = new Vector2(movementX, movementY);
            this.gameObject.transform.Translate(movement * speed / 10);
        }

        // Increases Gravity when jump has reached its peak
        if (rigidBody.velocity.y < 0)
        {
            rigidBody.velocity += Vector2.up * Physics.gravity.y * (gravMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidBody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigidBody.velocity += Vector2.up * Physics.gravity.y * (lowJumpGravMultiplier - 1) * Time.deltaTime;
        }

        // Code bellow keeps character facing the correct way 
        if (movementX < 0)
        {
            this.gameObject.transform.localScale = new Vector2(1, transform.localScale.y);
        }
        else if (movementX > 0)
        {
            this.gameObject.transform.localScale = new Vector2(-1, transform.localScale.y);
        }
    }

    // Adds upwards force to the player and increments jumps.
    void jump()
    {
        rigidBody.velocity = this.gameObject.transform.up * jumpHeight;
    }


    // Trigger checking if the player collides with death or chain tagged gameObjects
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Death")// If player hits a gameObject with the death tag they are dead
        {
            isDead = true;
            Debug.Log("Dead!");
        }

        if (other.tag == "Exit")// If player hits a gameObject with the death tag they are dead
        {
            LevelEditor.instance.editorMode();
        }

        if (other.tag == "RopCol")
        {
            ropeMovement = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "RopCol")
        {
            ropeMovement = true;
        }
    }

    // Trigger for checking when the player has left the rope
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "RopCol")// If player exits a gameObejct with the rope tag the gravity is turned on.
        {
            ropeMovement = false;
        }

    }
}


