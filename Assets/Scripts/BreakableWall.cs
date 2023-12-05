using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BreakableWall : MonoBehaviour, DamageableWall
{
    [SerializeField] HealthBar healthBar;
    ColorIndicator colorIndicator;
    NavMeshObstacle navMeshObstacle;

    [Header("Stats")]
    [SerializeField] float maxHealth;
    [SerializeField] protected float health;

    //// Start is called before the first frame update
    //void Awake()
    //{
        
    //}
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.SetHealthBar(maxHealth);
        colorIndicator = GetComponent<ColorIndicator>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    public void TakeDamage(float damage)
    {
        if (healthBar == null)
        {
            Debug.LogError("HealthBar not assigned in BreakableWall");
            return;
        }
        if (colorIndicator == null)
        {
            Debug.LogError("ColorIndicator not assigned in BreakableWall");
            return;
        }
        health -= damage;
        healthBar.UpdateHealthBar(health);
        colorIndicator.IndicateDamage();

        if (health <= 0)
        {
            DestroyWall();
        }
    }
    void DestroyWall()
    {
        if (navMeshObstacle != null)
        {
            navMeshObstacle.enabled = false;
        }

        Destroy(gameObject);
    }
}
