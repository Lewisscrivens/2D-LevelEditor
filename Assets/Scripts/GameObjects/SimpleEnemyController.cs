using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyController : MonoBehaviour
{

    private bool movingRight = false;
    private float direction = 5;
    private Vector2 movement;
    private Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.x == 0)
        {
            flip();
        }

        if (movingRight == true)
        {
            movement = new Vector2(direction, 0);
        }
        else if (movingRight == false)
        {
            movement = new Vector2(-direction, 0);
        }

        rb.velocity = movement;
    }

    // flip the boolean valiable.
    void flip()
    {
        if (movingRight == true)
        {
            movingRight = false;
        }
        else
        {
            movingRight = true;
        }
    }

    // Called when the trigger collider on the enemy registers a hit.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")//If enemy hits player it changes direction so the 2 rigid bodies do no cause bugs by colliding.
        {
            flip();
        }
    }
}

