using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Actor actor;

    //jump stuff
    bool canJump = true;
    public float jumpHeight;

    void Start()
    {
        actor = GetComponent<Actor>();
        actor.HitGround += OnHitGround;
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
        if (Input.GetButtonDown("Jump") && canJump == true)
        {
            actor.velocity.y += jumpHeight;
            canJump = false;
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

    //when Actor tells us we've hit the ground
    void OnHitGround()
    {
        canJump = true;
    }
}
