using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    private Collision coll;
    public Rigidbody2D rb;
    public LayerMask enemyLayer;
    public Transform attackPoint;

    public float speed = 5f;
    public float jumpForce = 8f;
    public float slideSpeed = 3f;
    public float attackRange = 5f;
    public int attackDamage = 5;
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

        if(coll.onWall && !coll.onGround)
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

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("hit");

            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

        
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }



}
