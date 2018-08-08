using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    #region Public Variables

    public Transform[] spawns;
    public GameObject[] armor;

    [Header("Health")]
    public float maxHealth = 100;

    #endregion

    #region Private Variables

    GameManager gm;

    Player player;
    EnemyStage stage;
    Animator animator;

    Image healthBarFrame;
    Image healthBarValue;
    float health;

    #endregion

    private void Start()
    {
        gm = GameObject.FindWithTag("Scripts").GetComponent<GameManager>();
        animator = gameObject.GetComponent<Animator>();

        healthBarFrame = GameObject.FindWithTag("Enemy HealthBar Frame").GetComponent<Image>();
        healthBarValue = GameObject.FindWithTag("Boss HealthBar").GetComponent<Image>();
        setHealth(maxHealth);

        setStage(EnemyStage.TWO);
    }

    private void Update()
    {
        if (gameObject.transform.position.y <= -8) Teleport(spawns[new System.Random().Next(0, spawns.Length - 1)]);
    }

    public void Teleport(Transform value)
    {
        animator.Play("EnemyTeleportEnd");
        transform.position = value.position;
        transform.rotation = value.rotation;
    }

    public float getHealth()
    {
        return health;
    }

    public void setHealth(float value)
    {
        healthBarFrame.rectTransform.sizeDelta = new Vector2(maxHealth + 5, 15);

        health = value;
        if (health > maxHealth) health = maxHealth;
        if (health >= 0) healthBarValue.rectTransform.sizeDelta = new Vector2(health, 10);
        if (health < 0)
        {
            health = 0;
            gm.EnemyDeath();
        }
    }

    public EnemyStage getStage()
    {
        return stage;
    }
    
    public void setStage(EnemyStage value)
    {
        stage = value;
    }
}

public enum EnemyStage
{
    ZERO,   // No Armor
    ONE,    // One Piece of Armor
    TWO     // Two Pieces of Armor
}
