using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

    #region Public Variables

    public Transform[] spawns;
    public TextMeshProUGUI meleeText, jumpText;

    [Header("Health")]
    public float maxHealth = 100;
    public float healthRemoved = 8;
    public float healthGained = 5;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 4f;

    [Header("Attacking")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float pushForce;

    [Header("Objects")]
    public GameObject gun;
    public GameObject knife;

    [Header("Sound")]
    public new AudioClip[] audio;

    #endregion

    #region Private Variables

    GameManager gm;

    Rigidbody rb;
    GameObject enemy;

    Vector3 lForward, lRight;
    Vector3 jumpMovement;

    Image healthBarFrame;
    Image healthBarValue;

    bool canJump = true;
    bool melee = false;
    bool grounded = true;
    bool canMelee;
    bool canRotate;

    float health;
    float meleeCooldown;
    float rotateCooldown;

    #endregion

    private void Start()
    {
        gm = GameObject.FindWithTag("Scripts").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        enemy = GameObject.FindWithTag("Enemy");

        healthBarFrame = GameObject.FindWithTag("Player HealthBar Frame").GetComponent<Image>();
        healthBarValue = GameObject.FindWithTag("Player HealthBar").GetComponent<Image>();
        setHealth(120);

        lForward = Camera.main.transform.forward;
        lForward.y = 0;
        lForward = Vector3.Normalize(lForward);
        lRight = Quaternion.Euler(new Vector3(0, 90, 0)) * lForward;

        InvokeRepeating("Countdown", 0.0f, 1.0f);
    }

    private void Update()
    {
        Transform point = spawns[new System.Random().Next(0, spawns.Length - 1)];
        if (gameObject.transform.position.y <= -8)
        {
            Teleport(point);
            setHealth(getHealth() - 2);
        }

        float distance = Vector3.Distance(enemy.transform.position, transform.position);
        if (distance <= 2.5) melee = true;
        else melee = false;

        if(gm.started)
        {
            Move();

            if (Input.GetButtonDown("Jump") && canJump)
                Jump();

            if (Input.GetButtonDown("Jump") && !grounded && canRotate)
            {
                GameObject go = GameObject.FindWithTag("MainPlatform");
                go.transform.Rotate(0, go.transform.position.y + 90, 0);

                rotateCooldown = Time.time + 5.0f;
                canRotate = false;
            }

            if (Input.GetButtonDown("Fire1"))
                Attack();
        }

        if (!canMelee && Time.time > meleeCooldown) canMelee = true;
        if (!canRotate && Time.time > rotateCooldown) canRotate = true;
    }

    private void Countdown()
    {
        if (!canMelee) meleeText.SetText("Melee\n" + (meleeCooldown - Time.time).ToString("0") + " secs");
        if (!canRotate) jumpText.SetText("Rotate\n" + (rotateCooldown - Time.time).ToString("0") + " secs");
    }

    private void PlaySound(AudioClip audio)
    {
        AudioSource source = Camera.main.gameObject.GetComponent<AudioSource>();
        source.clip = audio;
        source.Play();
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
            gm.PlayerDeath();
        }
    }

    public void Teleport(Transform value)
    {
        transform.position = value.position;
    }

    private void Move()
    {
        Vector3 movementRight = lRight * moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 movementUp = lForward * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");

        Vector3 normals = Vector3.Normalize(movementRight + movementUp);

        transform.forward = normals;
        transform.position += movementRight;
        transform.position += movementUp;
    }

    private void Attack()
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();

        if(melee && canMelee)
        {
            Animator knifeAnim = knife.GetComponent<Animator>();
            knifeAnim.Play("KnifeDraw");
            PlaySound(audio[2]);

            enemyScript.setHealth(enemyScript.getHealth() - 5);
            setHealth(getHealth() + healthGained);

            enemy.GetComponent<Rigidbody>().AddForce(-enemy.transform.forward * 5, ForceMode.VelocityChange);
            StartCoroutine(enemy.GetComponent<EnemyAI>().Freeze(0.5f));

            meleeCooldown = Time.time + 5.0f;
            canMelee = false;

        }
        else
        {
            setHealth(getHealth() - healthRemoved);

            GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;
            Destroy(bullet, 2.0f);

            Animator gunAnim = gun.GetComponent<Animator>();
            gunAnim.Play("GunRecoil");
            PlaySound(audio[0]);
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
        canJump = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor")
        {
            canJump = true;
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Floor")
        {
            grounded = false;
        }
    }
}