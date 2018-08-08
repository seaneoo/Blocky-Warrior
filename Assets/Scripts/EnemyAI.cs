using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    #region Public variables

    [System.NonSerialized]
    public AttackStage attackStage;

    public Animator animator;
    public Transform backUpTarget;

    [Header("Projectiles")]
    public GameObject projectile;
    public Transform projectileSpawn;
    public float projectileSpeed;

    [Header("Movement")]
    public float moveSpeed;

    #endregion

    #region Private variables

    GameObject player;

    bool frozen;
    int seconds = 0;

    #endregion

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        frozen = false;

        attackStage = AttackStage.PRE;

        InvokeRepeating("Count", 2.0f, 1.0f);
        InvokeRepeating("PickStage", 20.0f, 20.0f);
        InvokeRepeating("StandardAttack", 1.0f, 0.5f);
        InvokeRepeating("BurstAttack", 0.0f, 1.25f);
    }

    private void Update()
    {
        if(!frozen) Move();
    }

    public IEnumerator Freeze(float seconds)
    {
        frozen = true;
        yield return new WaitForSeconds(seconds);
        frozen = false;
    }

    private void Count()
    {
        seconds++;
        if(seconds == 2)
        {
            Debug.Log("Standard (initial)");
            attackStage = AttackStage.STAGE_1;
        }
    }

    private void PickStage()
    {
        AttackStage currentStage = attackStage;
        if(currentStage == AttackStage.PRE)
        {
            attackStage = AttackStage.STAGE_1;
        }else {
            if(currentStage == AttackStage.STAGE_1)
            {
                attackStage = AttackStage.STAGE_2;
            }else
            {
                attackStage = AttackStage.STAGE_1;
            }
        }
    }

    private void Move()
    {
        transform.LookAt(player.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (attackStage == AttackStage.STAGE_1)
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceFromPlayer > 10.0f)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 1.5f * Time.deltaTime);
            }

            if (distanceFromPlayer < 9.9f)
            {
                transform.position = Vector3.MoveTowards(transform.position, backUpTarget.position, 2.0f * Time.deltaTime);
            }
        }
    }

    private void StandardAttack()
    {
        if(!frozen)
        {
            if (attackStage == AttackStage.STAGE_1)
            {
                animator.Play("BossGunRecoil");

                GameObject go = (GameObject)Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
                go.GetComponent<Rigidbody>().velocity = go.transform.forward * projectileSpeed;
                Destroy(go, 5.0f);
            }
        }
    }

    private void BurstAttack()
    {
        if(!frozen)
        {
            if (attackStage == AttackStage.STAGE_2)
            {
                animator.Play("BossGunBurstRecoil");

                int numOfShots = 5;
                float spreadAngle = 20.0f;

                var qAngle = Quaternion.AngleAxis(-numOfShots / 2.0f * spreadAngle, transform.up) * transform.rotation;
                var qDelta = Quaternion.AngleAxis(spreadAngle, transform.up);

                for (var i = 0; i < numOfShots; i++)
                {
                    GameObject go = (GameObject)Instantiate(projectile, projectileSpawn.position, qAngle);
                    go.GetComponent<Rigidbody>().velocity = go.transform.forward * (projectileSpeed / 1.5f);
                    qAngle = qDelta * qAngle;

                    Destroy(go, 5.0f);
                }
            }
        }
    }
}

public enum AttackStage
{
    PRE,     // Before game starts (set-up)
    STAGE_1, // Standard gun attack
    STAGE_2  // Special attack
}
