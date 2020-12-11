using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 500;
    public int currentHealth;
    public Rigidbody rb;
    public HealthBar healthBar;
    public int harm =200;
    void Start ()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update ()
    {
        if (currentHealth <= 0)
            RestartGame();
    }

    void OnCollisionEnter (Collision collision)
    {
        //Reduce health
        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
        }   
        else if (collision.gameObject.tag == "enemytank")
        {
            rb.AddRelativeForce(new Vector3(-900.1f,0.0f , 900.0f));
            TakeDamage(harm);
        }
    }

    void TakeDamage (int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    void RestartGame ()
    {
        SceneManager.LoadScene(3);
    }
}
