using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    //controls how fast actor falls
    [Range(-1f, -10f)]
    public float gravity;
    public Vector3 velocity;

    //movement related variables

    [SerializeField]
    public float moveSpeed;
    [Range(1.0f, 10.0f)]
    
    float fallMultiplier = 2f;
    //float lowJumpMultiplier = 1.0f;

    

    Controller controller;

    public event Action HitGround;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        //apply gravity to velocity

        velocity.y += gravity* fallMultiplier*Time.deltaTime;


        //Debug.Log(velocity.y);

        //move player. If we hit the floor, set y-direction velocity to 0 and re-enable jumping
        if(controller.Move(velocity * Time.deltaTime))
        {
            velocity.y = 0;
            HitGround?.Invoke();
        }

    }


}
