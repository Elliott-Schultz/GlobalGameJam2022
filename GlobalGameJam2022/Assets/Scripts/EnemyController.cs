using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health;
    public int damage;
    public float knockBack;
    public float hitLength;

    public float walkingRange;
    public float agroRadius;
    private bool left;
    private Vector2 originalPosition;

    public bool hitToRight;
    public bool hitSomething;
    public bool facingRight;

    public float gotHit;
    private bool canDoDamage = true;
    public bool canMove = true;
    private Rigidbody2D rb;
    private bool hitFromRight;
    private float gotHitKnockBack;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        int rand = Random.Range(0, 2);
        left = rand == 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gotHit <= 0 && canMove)
        {
            canDoDamage = true;
            if (!hitSomething)
            {
                if (transform.position.x >= originalPosition.x + walkingRange)
                {
                    left = true;
                }
                if (left)
                {
                    if (transform.localScale.x < 0)
                    {
                        facingRight = false;
                        FlipSprite();
                    }
                    transform.Translate(-1f * Time.deltaTime, 0f, 0f);
                }

                if (transform.position.x <= originalPosition.x - walkingRange)
                {
                    left = false;
                }
                if (!left)
                {
                    if (transform.localScale.x > 0)
                    {
                        facingRight = true;
                        FlipSprite();
                    }
                    transform.Translate(1f * Time.deltaTime, 0f, 0f);
                }
            }
            else
            {

                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (Vector2.Distance(transform.position, player.transform.position) < agroRadius)
                {
                    if (transform.position.x >= player.gameObject.transform.position.x)
                    {
                        left = true;
                    }
                    if (left)
                    {
                        if (transform.localScale.x < 0)
                        {
                            facingRight = false;
                            FlipSprite();
                        }
                        transform.Translate(-1f * Time.deltaTime, 0f, 0f);
                    }

                    if (transform.position.x <= player.gameObject.transform.position.x)
                    {
                        left = false;
                    }
                    if (!left)
                    {
                        if (transform.localScale.x > 0)
                        {
                            facingRight = true;
                            FlipSprite();
                        }
                        transform.Translate(1f * Time.deltaTime, 0f, 0f);
                    }
                }
                else
                {
                    hitSomething = false;
                }
            }
        }
        else if (gotHit > 0)
        {
            rb.velocity = Vector2.zero;
            if (hitFromRight)
            {
                if (!facingRight)
                {
                    FlipSprite();
                }
                rb.AddForce(new Vector2(-gotHitKnockBack, gotHitKnockBack), ForceMode2D.Impulse);
                //rb.velocity = new Vector2(-knockBack, knockBack);
            }
            else
            {
                if (facingRight)
                {
                    FlipSprite();
                }
                rb.AddForce(new Vector2(gotHitKnockBack, gotHitKnockBack), ForceMode2D.Impulse);
                //rb.velocity = new Vector2(knockBack, knockBack);
            }
            gotHit -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && canDoDamage)
        {
            hitSomething = true;
            hitToRight = collision.gameObject.transform.position.x > transform.position.x;

            if(hitToRight && !facingRight)
            {
                FlipSprite();
                facingRight = true;
            }
            else if(!hitToRight && facingRight)
            {
                FlipSprite();
                facingRight = false;
            }

            collision.gameObject.GetComponent<PlayerController>().isHit(hitLength);
            collision.gameObject.GetComponent<PlayerController>().changeHealth(damage, true, transform.position, knockBack);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (!hitSomething)
            {
                left = !left;
            }
        }
    }

    private void FlipSprite()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void hit(int damage, Vector2 positionOfDamageSource, float knockBack, float hitLength)
    {
        canDoDamage = false;
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            gotHit = hitLength;
            this.gotHitKnockBack = knockBack;
            if (positionOfDamageSource.x < transform.position.x)
            {
                hitFromRight = false;
            }
            else
            {
                hitFromRight = true;
            }
            hitSomething = true;
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
