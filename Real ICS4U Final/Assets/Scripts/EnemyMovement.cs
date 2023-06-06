using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
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
    public int attackDamage = 10;
    public GameObject player;
    public Animator swordAnimator;
    private Animator enemyAnimator;
    private TrailRenderer swordTrail;
    private ParticleSystem footDust;
    private GameObject sword;
    private AudioSource audioSource;

    private bool inAgro = false;
    private bool canAttack = true;
    private bool canMove = true;
    private int side = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
        bc = transform.Find("AttackBox").GetComponent<BoxCollider2D>();
        swordTrail = transform.Find("PlayerSword").Find("Trail").GetComponent<TrailRenderer>();
        swordTrail.enabled = false;
        footDust = transform.Find("FootParticle").GetComponent<ParticleSystem>();
        enemyAnimator = transform.GetComponent<Animator>();
        sword = transform.Find("PlayerSword").gameObject;
        sword.SetActive(false);
        audioSource = transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!inAgro && Mathf.Abs(player.transform.position.x - transform.position.x) < detectRadius)
        {
            SoundManager.PlaySound(audioSource, GameAssets.i.enemyAgroOn);
            inAgro = true;
            enemyAnimator.SetBool("EnemyInAgro", true);
            sword.SetActive(true);
        }
        else if (inAgro && Mathf.Abs(player.transform.position.x - transform.position.x) > ignoreRadius)
        {
            inAgro = false;
            enemyAnimator.SetBool("EnemyWalking", false);
            enemyAnimator.SetBool("EnemyInAgro", false);
            sword.SetActive(false);
        }

        if(0 <= side * (player.transform.position.x - transform.position.x) && side * (player.transform.position.x - transform.position.x) < attackRadius && canAttack)
        {
            StartCoroutine(Attack());
        }

        if (inAgro && canMove)
        {
            enemyAnimator.SetBool("EnemyWalking", true);
            if(player.transform.position.y > transform.position.y && coll.wallSide == side && coll.onGround)
            {
                SoundManager.PlaySound(audioSource, GameAssets.i.jump);
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
        else
        {
            enemyAnimator.SetBool("EnemyWalking", false);
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

    IEnumerator Attack()
    {
        
        canAttack = canMove = false;
        yield return new WaitForSeconds(0.7f);
        SoundManager.PlaySound(audioSource, GameAssets.i.swordAttack);
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
