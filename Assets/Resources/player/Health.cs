using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 10;
    public float CHealth;
   
    // Start is called before the first frame update

    void Start()
    {
        CHealth = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log(CHealth);
        }
     
    }


    public void takeDamage(int amount)
    {
        CHealth -= amount;
        if (CHealth <= 0)
        {
            Destroy(gameObject);
        }


    }


}
