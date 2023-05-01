using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class DamageableTile : MonoBehaviour
{

    public int damage = 10;
    public float tick = 10f;
    private bool canDamagePlayer = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(canDamagePlayer && collision.tag == "Player")
        {
            StartCoroutine(damageWait());
            collision.GetComponent<CharacterController2D>().TakeDamage(damage);
        }
    }

    IEnumerator damageWait()
    {
        canDamagePlayer = false;
        yield return new WaitForSeconds(tick);
        canDamagePlayer = true;
    }
}
