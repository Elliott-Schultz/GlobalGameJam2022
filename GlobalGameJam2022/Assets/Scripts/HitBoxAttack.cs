using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxAttack : MonoBehaviour
{
    public int damage;
    public bool didDamage = false;
    public float knockBack;
    public float hitLength = 0.1f;

    private void OnEnable()
    {
        didDamage = false;
    }

    private void OnDisable()
    {
        didDamage = false;   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if(!didDamage)
            {
                didDamage = true;
                collision.gameObject.GetComponent<EnemyController>().hit(damage, gameObject.transform.parent.position, knockBack, hitLength);
            }
        }
    }
}
