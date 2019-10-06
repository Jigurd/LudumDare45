using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    Controller controller; 

    public Vector3 velocity;
    [SerializeField]
    public float moveSpeed;

    public LayerMask layermask;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {

        controller.Move(velocity * Time.deltaTime);
    }
}
