using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private Collision coll;
    public LayerMask playerMask;
    public float speed = 5f;
    public float jumpForce = 9f;
    public float agroRadius = 10f;
    public GameObject player;

    private int side = 1;
    private bool canWalk = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
    }

    // Update is called once per frame
    void Update()
    {

        //Walk(dir);

        if(InAgro())
        {
            if(player.transform.position.y > transform.position.y && coll.wallSide == side && coll.onGround)
            {
                Jump();
            }

            //walk to player
            if (player.transform.position.x > transform.position.x && !coll.onRightWall)
            {
                if (side == -1) Flip();
                side = 1;
                Walk(new Vector2(0.8f, 0));
            }
            else if(player.transform.position.x < transform.position.x && !coll.onLeftWall)
            {
                if (side == 1) Flip();
                side = -1;
                Walk(new Vector2(-0.8f, 0));
            }
        }

        //if in attack range for certain time
        //attack, damage player
    }

    private void Walk(Vector2 dir)
    {
        rb.velocity = (new Vector2(dir.x * speed, rb.velocity.y));
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpForce;
    }

    private bool InAgro()
    {
        if(Physics2D.OverlapCircle(transform.position, agroRadius, playerMask)) return true;
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, agroRadius);
    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        //Vector3 theScale2 = attackPoint.localScale;
        //theScale2.x *= -1;
        //attackPoint.localScale = theScale2;
    }
}
