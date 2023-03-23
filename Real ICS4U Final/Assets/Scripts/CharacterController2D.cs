using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    private Collision coll;
    public Rigidbody2D rb;
    public LayerMask enemyLayer;
    public LayerMask interactLayer;
    public Transform attackPoint;
    public Transform interactPoint;

    public float speed = 5f;
    public float jumpForce = 8f;
    public float slideSpeed = 3f;
    public float attackRange = 5f;
    public float interactRange = 8f;
    public int attackDamage = 5;
    public int side = 1;
    public bool canMove;
    public bool wallSlide;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 dir = new Vector2(x, y);

        Walk(dir);

        if (Input.GetButtonDown("Jump"))
        {
            if(coll.onGround) Jump();
        }

        if(coll.onWall && !coll.onGround && x != 0)
        {
            wallSlide = true;
            WallSide();
        }

        if(!coll.onWall || coll.onGround)
        {
            wallSlide = false;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

        if(Input.GetButtonDown("Fire2"))
        {
            Interact();
        }

        if(x > 0)
        {
            if(side == -1) Flip();
            side = 1;
        }
        if(x < 0)
        {
            if(side == 1) Flip();
            side = -1;
        }
    }

    private void WallSide()
    {
       

        rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
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

    private void Attack()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private void Interact()
    {
        Collider2D[] interactNPC = Physics2D.OverlapCircleAll((Vector2)transform.position, interactRange, interactLayer);

        foreach(Collider2D NPC in interactNPC)
        {
            NPC.GetComponent<Shop>().OpenShop();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null) return;
        if (interactPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere((Vector2)transform.position, interactRange);
    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        Vector3 theScale2 = attackPoint.localScale;
        theScale2.x *= -1;
        attackPoint.localScale = theScale2;
    }



}
