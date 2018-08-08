using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {

    public float projectileDamage = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            Destroy(gameObject);

            Enemy enemy = collision.collider.gameObject.GetComponent<Enemy>();
            enemy.setHealth(enemy.getHealth() - projectileDamage);
        }
    }
}
