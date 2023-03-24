using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public LayerMask playerMask;
    public float speed = 5f;
    public float agroRadius = 10f;
    public GameObject player;

    private int side = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0.1f;
        float y = 0;

        Vector2 dir = new Vector2(x, y);

        //Walk(dir);

        if(InAgro())
        {
            //walk to player
            if (player.transform.position.x > transform.position.x)
            {
                if (side == -1) Flip();
                side = 1;
                Walk(new Vector2(0.3f, 0));
            }
            else
            {
                if (side == 1) Flip();
                side = -1;
                Walk(new Vector2(-0.3f, 0));
            }
        }
    }

    private void Walk(Vector2 dir)
    {
        rb.velocity = (new Vector2(dir.x * speed, rb.velocity.y));
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
