using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D self;
    public int maxHealth = 150;
    public int health;
    public int reward = 50;
    public Transform EnemyHealthBar;
    public GameObject EnemyHealthBarobj;

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Rigidbody2D>();
        health = maxHealth;
        EnemyHealthBarobj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if(health != maxHealth) EnemyHealthBarobj.SetActive(true);
        health -= damage;
        if(health <= 0) Die();
        EnemyHealthBar.GetComponent<Image>().fillAmount = (float)health / (float)maxHealth;
    }

    void Die()
    {
        player.GetComponent<CharacterController2D>().money += reward;
        Destroy(gameObject);
    }
}
