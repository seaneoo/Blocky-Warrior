using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    public float projectileDamage = 4;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            Destroy(gameObject);

            Player player = collision.collider.gameObject.GetComponent<Player>();
            player.setHealth(player.getHealth() - projectileDamage);
        }

        if (collision.collider.tag == "Enemy")
        {
            Destroy(gameObject);

            Enemy enemy = collision.collider.gameObject.GetComponent<Enemy>();
            enemy.setHealth(enemy.getHealth() - projectileDamage);
        }
    }
}
