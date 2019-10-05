using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Actor actor;
    Controller controller;

    

    void Start()
    {
        controller = GetComponent<Controller>();
        actor = GetComponent<Actor>();
    }

    // Update is called once per frame
    void Update()
    {
        
        HandleMovement();
    }

    void HandleMovement()
    {
        //reset horizontal speed
        actor.velocity.x = 0;

        //if the player is jumping, add jump velocity to jump
        if (Input.GetButtonDown("Jump") && controller.collisions.below)
        {
            actor.velocity.y += actor.jumpVelocity;
        }
        //move left
        if (Input.GetButton("Left"))
        {
            actor.velocity.x -= actor.moveSpeed;
        }
        //move right
        if (Input.GetButton("Right"))
        {
            actor.velocity.x += actor.moveSpeed;
        }
    }
}
