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
    public AudioSource soundEffectPlayer;
    public Vector3 newPlayerSpawn;
    public GameObject removingWall;

    void Start()
    {
        self = GetComponent<Rigidbody2D>();
        health = maxHealth;
        EnemyHealthBarobj.SetActive(false);
        Physics2D.IgnoreLayerCollision(7, 7, true);
    }

    public void TakeDamage(int damage)
    {
        // if damaged by player, show health bar
        if(health != maxHealth) EnemyHealthBarobj.SetActive(true);

        health -= damage;
        if(health <= 0) Die();

        // update health bar
        EnemyHealthBar.GetComponent<Image>().fillAmount = (float)health / (float)maxHealth;
    }

    void Die()
    {
        // remove wall, set new spawn point if exist.
        if (newPlayerSpawn != new Vector3(0, 0, 0)) player.GetComponent<CharacterController2D>().spawnPoint = newPlayerSpawn;
        if (removingWall != null) removingWall.SetActive(false);

        // add money to player
        player.GetComponent<CharacterController2D>().money += reward;

        SoundManager.PlaySound(soundEffectPlayer, GameAssets.i.die);
        Destroy(gameObject);
    }
}
