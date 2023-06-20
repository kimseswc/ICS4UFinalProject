using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public LayerMask groundLayer;
    public bool onGround;
    public bool onWall;
    public bool onLeftWall;
    public bool onRightWall;
    public int wallSide;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    public float collisionRadius = 0.3f;


    // checks the circle is overlapping the ground layer
    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);

        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);

        // if leftwall : -1   if rightwall : 1
        wallSide = onLeftWall ? -1 : (onRightWall ? 1 : 0);
    }

    // draws circles on character
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawSphere((Vector2)transform.position + leftOffset, collisionRadius);
        Gizmos.DrawSphere((Vector2)transform.position + rightOffset, collisionRadius);
    }
}
