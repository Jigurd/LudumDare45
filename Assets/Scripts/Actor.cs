using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    //controls how fast actor falls
    [Range(-1f, -5f)]
    public float gravity;
    Vector3 velocity;

    Controller controller;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {

        //apply gravity to velocity
        velocity.y += gravity*Time.deltaTime;
        //move player
        controller.Move(velocity * Time.deltaTime);
    }
}
