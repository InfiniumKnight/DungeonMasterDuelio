using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class goboController : MonoBehaviour
{
    public bool canattack = true;
    public float speed = 5f;
    public Canvas levelUpScreen;

    public float attackSpeed = 3.0f;
    public float attackL = 1f;

    public float maxHealth = 5;
    public float currentHealth;

    public float attackRange = 3.0f;

    private Transform attackTransform;

    private float attackTimeCounter;

    public TextMeshProUGUI health;

    public TextMeshProUGUI currentLevel;

    public TextMeshProUGUI xp;

    public TextMeshProUGUI attackLevel;

    public float level;
    public float claims;
    public float xpToNextLevel = 10f;
    public float currentXp;

    public bool gameOver = false;

    private float damage = 1.0f;

    public Canvas gameOverScreen;
    public Canvas hud;

    public ParticleSystem GoboHealParticle;

    public ParticleSystem GoboDamageParticle;

    public AudioClip Healsound;
    public AudioClip swingSound;
    public AudioClip hitSound;
    public AudioClip gameOverS;
    public AudioClip LevelUp;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;

    public AudioSource backgroundMusic;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        attackTimeCounter += Time.deltaTime;

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        if (currentXp >= xpToNextLevel)
        {
            currentXp = 0;
            xpToNextLevel += 10;
            level += 1;
            claims += 1;
            PlaySound(LevelUp);
            xp.text = "0/" + xpToNextLevel.ToString();
        }

        if (claims >= 1)
        {
            levelUpScreen.gameObject.SetActive(true);
            if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1))
            {
                attackUp();
                claims -= 1;
                if (claims >= 1)
                {
                    levelUpScreen.gameObject.SetActive(false);
                }
            }
            if (Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2))
            {
                maxHealthUp();
                claims -= 1;
                if (claims >= 1)
                {
                    levelUpScreen.gameObject.SetActive(false);
                }
            }
            if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3))
            {
                heal();
                claims -= 1;
                if (claims >= 1)
                {
                    levelUpScreen.gameObject.SetActive(false);
                }
            }
            
        }

        if (currentHealth <= 0)
        {
            speed = 0.0f;
            gameOverScreen.gameObject.SetActive(true);
            gameOver = true;
            audioSource.volume = 0.1f;
            PlaySound(gameOverS);
            canattack = false;
            backgroundMusic.Stop();
        }

        //animator.SetFloat("Look X", lookDirection.x);
        //animator.SetFloat("Look Y", lookDirection.y);
        //animator.SetFloat("Speed", move.magnitude);

        if (Input.GetKey(KeyCode.R))

        {

            if (gameOver == true)

            {

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene

            }

        }
    }

    public void UpdateHealth(float mod){
        currentHealth += mod;
        if (mod < 0)
        {
            audioSource.volume = 1.0f;
            ParticleSystem GoboDamage = Instantiate(GoboDamageParticle, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            PlaySound(hitSound);
        }

        if (mod > 0)
        {
            ParticleSystem GoboHeal = Instantiate(GoboHealParticle, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            PlaySound(Healsound);
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        health.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (canattack == true)
        {
            EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
            if (attackTimeCounter >= attackSpeed)
            {
                if (enemyController != null)
                {
                    if (attackSpeed <= attackTimeCounter)
                    {
                        //animator.SetTrigger("Attack");
                        audioSource.volume = 1.0f;
                        PlaySound(swingSound);

                        other.gameObject.GetComponent<EnemyController>().UpdateHealth(-damage);
                        attackTimeCounter = 0;
                    }
                    else
                    {
                        attackTimeCounter += Time.deltaTime;
                    }
                }
            }
        }
    }

    public void maxHealthUp()
    {
        maxHealth += 2;
        currentHealth += 2;
        health.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    public void heal()
    {
        UpdateHealth(4);
        health.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    public void attackUp()
    {
        attackSpeed -= .25f;
        attackL += 1;
        attackLevel.text = "lvl" + attackL.ToString();
    }
   
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);

    }
}
