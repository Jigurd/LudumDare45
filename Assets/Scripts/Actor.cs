using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    //controls how fast actor falls
    [Range(1f, 1.3f)]
    public float fallSpeedMultiplier = 1.1f;

    public Vector3 velocity;

    //movement related variables

    [SerializeField]
    public float moveSpeed;

    //jump related stuff
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    float gravity;
    public float jumpVelocity;




    Controller controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }

    void LateUpdate()
    {
        if (GameState.Paused)
        {
            return;
        }
        //apply gravity to velocity

        velocity.y += gravity * Time.deltaTime;

        //if we're falling, apply fallTimeMultiplier to fall faster. This skews our jump arc right.
        if(velocity.y<0)
        {
            velocity.y *= fallSpeedMultiplier;
        }
        

        //move player. If we hit the floor, set y-direction velocity to 0 and re-enable jumping
        controller.Move(velocity * Time.deltaTime);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

    }


}
