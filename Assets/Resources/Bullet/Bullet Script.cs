using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSript : MonoBehaviour
{
    public Health healthScript;
    public int damage = 1;
    //movement speed in units per second
    public float movementSpeed = 200f;
    public float bulletTimer = 6f;
    



    private void Start()
    {

    }


    void Update()
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;

        if (bulletTimer <= 0.0f)
        {
            bulletEnd();
        }
        else
        {
            bulletTimer -= Time.deltaTime;
        }



    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Health enemyHealth = collision.gameObject.GetComponent<Health>();
            if (enemyHealth != null)
            {

                enemyHealth.takeDamage(damage);

            }
            bulletEnd();
        }
    }
        void bulletEnd()
        {
            Destroy(gameObject);
        }


}

    

