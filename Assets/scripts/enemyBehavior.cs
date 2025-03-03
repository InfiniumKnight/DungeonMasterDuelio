using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int damage = 1;
    public float maxHealth = 2;
    public float currentHealth;
    Animator animator;
    private float attackTimeCounter;
    public float attackSpeed = 1.0f;

    public float speed = 3.0f;
    Rigidbody2D rb;
    public Transform target;
    Vector2 moveDirection;

    // Use this for initialization
    void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        currentHealth = maxHealth;
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        moveDirection = direction;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<goboController>().gameOver == false)
        {
            if (other.gameObject.tag == "Player")
            {
                if (attackSpeed <= attackTimeCounter)
                {
                    other.gameObject.GetComponent<goboController>().UpdateHealth(-damage);
                    attackTimeCounter = 0;
                }
                else
                {
                    attackTimeCounter += Time.deltaTime;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        moveEnemy(moveDirection);
    }
    void moveEnemy(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }

    public void UpdateHealth(float mod)
    {
        
        currentHealth += mod;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0)
        {
            goboController gobo = GameObject.FindGameObjectWithTag("Player").GetComponent<goboController>();
            gobo.currentXp += 5;
            gobo.xp.text = gobo.currentXp.ToString() + "/" + gobo.xpToNextLevel.ToString();
            Destroy(gameObject);
        }
    }

}