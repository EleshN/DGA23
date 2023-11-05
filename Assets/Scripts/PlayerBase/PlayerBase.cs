using UnityEngine;

public class PlayerBase : MonoBehaviour, IDamageable
{
    [Range(0.1f, 10.0f)]
    public float RegenTickSpeed;
    [SerializeField] float health;
    public GameObject EnemyBaseObject;
    private ColorIndicator colorIndicator;

    void Start()
    {
        GameManager.Instance.Register(this);

        // Get the DamageIndicator component attached to the same GameObject
        colorIndicator = GetComponent<ColorIndicator>();
    }

    public void TakeDamage(float amount)
    {
        if (!isDamageable()) return;

        health -= amount;
        // healthBar.UpdateHealthBar(health);
        if (colorIndicator != null)
        {
            colorIndicator.IndicateDamage();
        }

        if (health <= 0)
        {
            GameManager.Instance.Unregister(this);
            Instantiate(EnemyBaseObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public bool isDamageable()
    {
        // Implementation for isDamageable
        // Return true or false based on the PlayerBase's current state
        return true;
    }
}
