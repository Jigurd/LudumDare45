using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{


    //logic stuff
    bool isHeld = false;
    bool heldLastFrame = false;

    [SerializeField]
    private Platform[] _platforms; //platforms this toggles

    [SerializeField]
    private Transform _switchTransform;
    //collision stuff
    const float skinWidth = 0.15f;
    private int _rayCount = 4; 

    private BoxCollider2D _collision;

    [SerializeField]
    private LayerMask collisionMask;
    RaycastOrigins raycastOrigins; 

    float horizontalRaySpacing;
    float verticalRaySpacing;


    // Start is called before the first frame update
    void Start()
    {
        _collision = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
        UpdateRaycastOrigins();
    }

    // Update is called once per frame
    void Update()
    {
        //check if button is held
        isHeld = Collisions();
        
        //check if button has changed since last frame

        //if it was
        if (isHeld != heldLastFrame)
        {
            //toggle button state
            //if button is now held
            if (isHeld)
            {
                _switchTransform.position -= Vector3.up * 0.4f;
            } else
            {
                _switchTransform.position += Vector3.up * 0.4f;
            }
            //toggle all assigned platforms
            foreach (Platform platform in _platforms)
            {
                
                //platform.Toggle();
            }
            Debug.Log("Poke!");
        }

        //update heldLastFram
        heldLastFrame = isHeld;
    }

    //checks for collisions along top, left, and right. If it finds any, it returns.
    bool Collisions()
    {
        //check for collisons on the left
        for (int i = 0; i < _rayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomLeft;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, skinWidth, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.left* skinWidth, Color.red);

            if (hit)
            {
                return true;
            }
        }

        //check for collisions on the right
        for (int i = 0; i < _rayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, skinWidth, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * skinWidth, Color.red);

            if (hit)
            {
                return true;
            }
        }

        //check for collisions on the top
        for (int i = 0; i < _rayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, skinWidth, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * skinWidth, Color.red);

            if (hit)
            {
                return true;
            }
        }
        return false;
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = _collision.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRaySpacing = Mathf.Clamp(_rayCount, 2, int.MaxValue);
        verticalRaySpacing = Mathf.Clamp(_rayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (_rayCount - 1);
        verticalRaySpacing = bounds.size.x / (_rayCount - 1);


    }
    void UpdateRaycastOrigins()
    {
        Bounds bounds = _collision.bounds;
        bounds.Expand(skinWidth * -2);
    
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
