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
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
        Physics2D.IgnoreLayerCollision(6, 7, true);
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(x, y);

        if(!inUI && !isDashing) {
            Walk(dir);
        }

        if (Input.GetKeyDown("c"))
        {
            if(!inUI && coll.onGround && !isDashing) Jump();
        }

        if(Input.GetKeyDown("left shift"))
        {
            if(quickSlotItemAmount != 0)
            {
                if(health != maxHealth)
                {
                    health = (health + quickSlotItemEffect > maxHealth ? maxHealth : health + quickSlotItemEffect);
                    quickSlotItemAmount--;
                }
            }
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

        if (!inUI && Input.GetKeyDown("z") && canAttack)
        {
            Attack();
        }

    
        if(Input.GetKeyDown("x")) {
            if(xRaw == 0 && yRaw == 0)
            {
                Interact();
            }
            else if(!inUI && (xRaw != 0 || yRaw != 0) && !hasDashed)
            {
                Dash(xRaw, yRaw);
            }
        }

        if(coll.onGround)
        {
            hasDashed = false;
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
        if((x == 1 || x == -1) && y == 0) rb.gravityScale = 0;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        rb.gravityScale = 1.5f;
        isDashing = false;
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
        canAttack = true;
    }

    private void Interact()
    {
        Collider2D[] c = Physics2D.OverlapCircleAll((Vector2)transform.position, interactRange, interactLayer);

        foreach (Collider2D interact in c)
        {
            if (interact.tag == "shop")
            {
                interact.GetComponent<Shop>().OpenShop(); inUI ^= true;
                return;
            }
            else if (interact.tag == "dialogue")
            {
                interact.GetComponent<Conversation>().nextLine();
                return;
            }
            else if (interact.tag == "puzzle")
            {
                interact.GetComponent<Puzzle>().interactSwitch();
                return;
            }
            else if(interact.tag == "chest")
            {
                interact.GetComponent<Chest>().interactChest();
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
            transform.position = new Vector3(0, 0, 0);
            health = maxHealth;
        }
    }
}
