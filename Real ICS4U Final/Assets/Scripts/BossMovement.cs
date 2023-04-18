using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    private Collision coll;
    public LayerMask playerMask;
    public float speed = 5f;
    public float jumpForce = 9f;
    public float detectRadius = 10f;
    public float ignoreRadius = 15f;
    public float attackRadius = 2f;
    public float rangeAttackRadius = 6f;
    public int attackDamage = 10;
    public int dashAttackDamage = 20;
    public float dashSpeed = 8;
    public GameObject player;
    public Animator swordAnimator;

    private bool inAgro = false;
    private bool canAttack = true;
    private bool canMove = true;
    private int side = 1;
    private bool isDashing = false;
    private bool canDashAttack = true;
    private bool isDashAttackCooldown = false;
    private BoxCollider2D dashAttackBox;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
        bc = transform.Find("AttackBox").GetComponent<BoxCollider2D>();
        dashAttackBox = transform.Find("DashAttackBox").GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inAgro && Mathf.Abs(player.transform.position.x - transform.position.x) < detectRadius)
        {
            inAgro = true;
        }
        else if (inAgro && Mathf.Abs(player.transform.position.x - transform.position.x) > ignoreRadius)
        {
            inAgro = false;
        }

        if (0 <= side * (player.transform.position.x - transform.position.x) && side * (player.transform.position.x - transform.position.x) < attackRadius && canAttack)
        {
            Attack();
        }
        else if (0 <= side * (player.transform.position.x - transform.position.x) && side * (player.transform.position.x - transform.position.x) < rangeAttackRadius && canAttack && !isDashAttackCooldown)
        {
            DashAttack();
        }

        if (inAgro && canMove)
        {
            if (player.transform.position.y > transform.position.y && coll.wallSide == side && coll.onGround)
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
            else if (player.transform.position.x < transform.position.x && !coll.onLeftWall)
            {
                if (side == 1) Flip();
                side = -1;
                Walk(new Vector2(-0.8f, 0));
            }
        }

        //if in attack range for certain time
        //attack, damage player
        if (isDashing && canDashAttack)
        {
            Collider2D[] cols = Physics2D.OverlapBoxAll(dashAttackBox.bounds.center, dashAttackBox.bounds.extents, 0f, LayerMask.GetMask("Player"));
            foreach (Collider2D c in cols)
            {
                c.GetComponent<CharacterController2D>().TakeDamage(dashAttackDamage);
                canDashAttack = false;
                break;
            }
        }
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

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        //Vector3 theScale2 = attackPoint.localScale;
        //theScale2.x *= -1;
        //attackPoint.localScale = theScale2;
    }

    private void Attack()
    {
        swordAnimator.SetTrigger("Attack");
        canAttack = false;
        StartCoroutine(AttackPrepare());
        Collider2D[] cols = Physics2D.OverlapBoxAll(bc.bounds.center, bc.bounds.extents, 0f, LayerMask.GetMask("Player"));
        StartCoroutine(AttackWait(2f));
        foreach (Collider2D c in cols)
        {
            c.GetComponent<CharacterController2D>().TakeDamage(attackDamage);
            
            break;
        }
    }

    private void DashAttack()
    {
        Debug.Log("DashAttack");
        StartCoroutine(AttackPrepare());
        isDashing = true;
        canDashAttack = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(side, 0);

        rb.velocity = dir * dashSpeed;
        

        StartCoroutine(DashAttackWait(0.5f));
        StartCoroutine(DashAttackWait());
    }

    

    IEnumerator DashAttackWait()
    {
        isDashAttackCooldown = true;
        yield return new WaitForSeconds(5f);
        isDashAttackCooldown = false;
    }

    IEnumerator AttackPrepare()
    {
        canMove = false;
        canAttack = false;
        yield return new WaitForSeconds(1f);
        canMove = true;
        canAttack = true;
    }

    IEnumerator AttackWait(float t)
    {
        canMove = false;

        yield return new WaitForSeconds(t);

        canMove = true;
    }

    IEnumerator DashAttackWait(float t)
    {
        canMove = false;
        canAttack = false;

        yield return new WaitForSeconds(t);

        canMove = true;
        isDashing = false;
    }
}
