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
    private Animator bossAnimator;
    private GameObject sword;

    private bool inAgro = false;
    private bool canAttack = true;
    private bool canMove = true;
    private int side = 1;
    private bool isDashing = false;
    private bool canDashAttack = true;
    private bool isDashAttackCooldown = false;
    private BoxCollider2D dashAttackBox;
    private TrailRenderer swordTrail;
    private ParticleSystem footDust;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
        bc = transform.Find("AttackBox").GetComponent<BoxCollider2D>();
        dashAttackBox = transform.Find("DashAttackBox").GetComponent<BoxCollider2D>();
        swordTrail = transform.Find("PlayerSword").Find("Trail").GetComponent<TrailRenderer>();
        swordTrail.enabled = false;
        footDust = transform.Find("FootParticle").GetComponent<ParticleSystem>();
        bossAnimator = transform.GetComponent<Animator>();
        sword = transform.Find("PlayerSword").gameObject;
        sword.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inAgro && Mathf.Abs(player.transform.position.x - transform.position.x) < detectRadius)
        {
            inAgro = true;
            bossAnimator.SetBool("BossInAgro", true);
            sword.SetActive(true);
        }
        else if (inAgro && Mathf.Abs(player.transform.position.x - transform.position.x) > ignoreRadius)
        {
            inAgro = false;
            bossAnimator.SetBool("BossWalking", false);
            bossAnimator.SetBool("BossInAgro", false);
            sword.SetActive(false);
        }

        if(canAttack)
        {
            if (0 <= side * (player.transform.position.x - transform.position.x) && side * (player.transform.position.x - transform.position.x) < attackRadius)
            {
                StartCoroutine(Attack());
            }
            else if(0 <= side * (player.transform.position.x - transform.position.x) && side * (player.transform.position.x - transform.position.x) < rangeAttackRadius)
            {
                if (!isDashAttackCooldown) StartCoroutine(DashAttack());
            }
        }

        if (inAgro && canMove)
        {
            bossAnimator.SetBool("BossWalking", true);
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
        else
        {
            bossAnimator.SetBool("BossWalking", false);
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
        footDust.Play();
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

    IEnumerator DashAttack()
    {
        isDashAttackCooldown = true;
        canAttack = false;
        canMove = false;
        yield return new WaitForSeconds(1f);

        swordTrail.enabled = true;
        isDashing = true;
        canDashAttack = true;
        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(side, 0) * dashSpeed;
        yield return new WaitForSeconds(0.5f);

        rb.velocity = Vector2.zero;
        swordTrail.enabled = false;
        yield return new WaitForSeconds(3f);

        canAttack = true;
        canMove = true;

        yield return new WaitForSeconds(5f);
        isDashAttackCooldown = false;
    }

    IEnumerator Attack()
    {
        canAttack = canMove = false;
        yield return new WaitForSeconds(0.7f);
        swordAnimator.SetTrigger("Attack");
        swordTrail.enabled = true;
        yield return new WaitForSeconds(0.1f);
        Collider2D[] cols = Physics2D.OverlapBoxAll(bc.bounds.center, bc.bounds.extents, 0f, LayerMask.GetMask("Player"));
        foreach (Collider2D c in cols) c.GetComponent<CharacterController2D>().TakeDamage(attackDamage);
        yield return new WaitForSeconds(0.7f);
        canAttack = canMove = true;
        swordTrail.enabled = false;
    }
}
