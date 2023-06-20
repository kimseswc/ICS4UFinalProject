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
    public Animator swordAnimator;
    private TrailRenderer swordTrail;
    private TrailRenderer dashTrail;
    private ParticleSystem footDust;
    private Animator playerAnimator;
    private AudioSource audioSource;
    public MenuManager menuManager;
    public int health = 100;
    public int maxHealth = 100;
    public int money = 100;
    public float speed = 5f;
    public float dashSpeed = 5f;
    public float jumpForce = 8f;
    public float slideSpeed = 3f;
    public float attackRange = 5f;
    public float interactRange = 8f;
    public int attackDamage = 5;
    public int side = 1;
    public ShopItemList.ItemType quickSlotItem;
    public int quickSlotItemAmount = 0;
    public int quickSlotItemEffect = 0;
    public bool canMove;
    public bool wallSlide;
    private bool isDashing;
    private bool hasDashed;
    private bool canAttack = true;
    public bool inUI = false;
    public Vector3 spawnPoint = new Vector3(0, 0, 0);
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
        Physics2D.IgnoreLayerCollision(6, 7, true);
        swordTrail = transform.Find("PlayerSword").Find("Trail").GetComponent<TrailRenderer>();
        swordTrail.enabled = false;
        dashTrail = transform.Find("Trail").GetComponent<TrailRenderer>();
        dashTrail.enabled = false;
        footDust = transform.Find("FootParticle").GetComponent<ParticleSystem>();
        playerAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Screen.SetResolution(1920, 1080, false);
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(x, y);

        // walk to facing direction
        if(!inUI && !isDashing) {
            Walk(dir);
            if (xRaw != 0 && coll.onGround) footDust.Play();

            if (x != 0)
            {
                playerAnimator.SetBool("PlayerWalking", true);
            }
            else
            {
                playerAnimator.SetBool("PlayerWalking", false);
            }
        }

        if (Input.GetKeyDown("c"))
        {
            if (!inUI && coll.onGround && !isDashing)
            {
                SoundManager.PlaySound(audioSource, GameAssets.i.jump);
                Jump();
            }
        }

        if(Input.GetKeyDown("left shift"))
        {
            // check if player has potion, if player is already max health
            // if not, heal by potion's amount
            if(quickSlotItemAmount != 0)
            {
                if(health != maxHealth)
                {
                    SoundManager.PlaySound(audioSource, GameAssets.i.usePotion);
                    health = (health + quickSlotItemEffect > maxHealth ? maxHealth : health + quickSlotItemEffect);
                    quickSlotItemAmount--;
                }
            }
        }

        // if colliding with wall, slide
        if(coll.onWall && !coll.onGround && x != 0)
        {
            wallSlide = true;
            WallSide();
        }

        if(!coll.onWall || coll.onGround)
        {
            wallSlide = false;
        }

        if (!inUI && Input.GetKeyDown("z") && canAttack)
        {
            SoundManager.PlaySound(audioSource, GameAssets.i.swordAttack);
            Attack();
        }

        if(Input.GetKeyDown("x")) {
            // if no direction input
            if(xRaw == 0 && yRaw == 0)
            {
                Interact();
            }
            else if(!inUI && (xRaw != 0 || yRaw != 0) && !hasDashed)
            {
                SoundManager.PlaySound(audioSource, GameAssets.i.dash);
                Dash(xRaw, yRaw);
            }
        }

        if(coll.onGround)
        {
            hasDashed = false;
        }

        // flip player image direction
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

    private void Dash(float x, float y)
    {
        hasDashed = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait(x, y));
    }

    IEnumerator DashWait(float x, float y)
    {
        // dash to direction
        dashTrail.enabled = true;
        if((x == 1 || x == -1) && y == 0) rb.gravityScale = 0;
        isDashing = true;
        yield return new WaitForSeconds(.3f);

        // can dash
        rb.gravityScale = 1.5f;
        isDashing = false;
        dashTrail.enabled = false;
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

    // get every enemy hitbox collision and damage the colliding enemy
    private void Attack()
    {
        swordTrail.enabled = true;
        swordAnimator.SetTrigger("Attack");
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
        StartCoroutine(AttackWait());
    }

    IEnumerator AttackWait()
    {
        canAttack = false;
        yield return new WaitForSeconds(0.3f);

        swordTrail.enabled = false;
        canAttack = true;
    }

    private void Interact()
    {
        Collider2D[] c = Physics2D.OverlapCircleAll((Vector2)transform.position, interactRange, interactLayer);

        foreach (Collider2D obj in c)
        {
            // if interacting object is ...
            if (obj.tag == "shop")
            {
                // open UI
                SoundManager.PlaySound(audioSource, GameAssets.i.buttonClick);
                obj.GetComponent<Shop>().OpenShop(); inUI ^= true;
                return;
            }
            else if (obj.tag == "dialogue")
            {
                // start dialogue
                SoundManager.PlaySound(audioSource, GameAssets.i.buttonClick);
                obj.GetComponent<Conversation>().nextLine();
                return;
            }
            else if (obj.tag == "puzzle")
            {
                // interact with light block
                SoundManager.PlaySound(audioSource, GameAssets.i.buttonClick);
                obj.GetComponent<Puzzle>().interactSwitch();
                return;
            }
            else if(obj.tag == "chest")
            {
                // open chest
                SoundManager.PlaySound(audioSource, GameAssets.i.buttonClick);
                obj.GetComponent<Chest>().interactChest();
                return;
            }
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

    public bool TrySpend(int itemCost)
    {
        if(money >= itemCost)
        {
            money -= itemCost;
            Debug.Log("current money : " + money.ToString());
            return true;
        }
        else
        {
            Debug.Log("need more money");
            return false;
        }
    }

    public void AddMaxHealth(int amount)
    {
        maxHealth += amount;
    }

    public void AddDamage(int amount)
    {
        attackDamage += amount;
    }

    // add item to quick slot, increase the amount of duplicate item
    public void AddQuickSlotItem(ShopItemList.ItemType i, int effect)
    {
        quickSlotItemEffect = effect;
        if(i != quickSlotItem)
        {
            quickSlotItem = i;
            quickSlotItemAmount = 1;
            
        }
        else
        {
            quickSlotItemAmount++;
        }
    }

    public void TakeDamage(int amount)
    {
        health = (health - amount < 0 ? 0 : health - amount);
        if(health == 0)
        {
            menuManager.playerDied();
            transform.position = spawnPoint;
            health = maxHealth;
        }
    }
}
