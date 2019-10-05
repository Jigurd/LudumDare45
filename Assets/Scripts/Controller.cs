using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller : MonoBehaviour
{
    BoxCollider2D collision;
    RaycastOrigins raycastOrigins;

    public LayerMask collisionMask;

    const float skinWidth = 0.15f;
    public int rayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    void Start()
    {
        collision = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();

    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        if(velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }

        if(velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }
        transform.Translate(velocity);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collision.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRaySpacing = Mathf.Clamp(rayCount, 2, int.MaxValue);
        verticalRaySpacing = Mathf.Clamp(rayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (rayCount - 1);
        verticalRaySpacing = bounds.size.x / (rayCount - 1);


    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < rayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < rayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collision.bounds;
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

