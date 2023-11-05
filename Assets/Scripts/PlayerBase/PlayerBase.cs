using UnityEngine;

public class PlayerBase : MonoBehaviour, IDamageable
{
    [Range(0.1f, 10.0f)]
    public float RegenTickSpeed;
    public float HP;
    public GameObject EnemyBaseObject;

    // Reference to the DamageIndicator component
    private DamageIndicator damageIndicator;

    void Start()
    {
        GameManager.Instance.Register(this);

        // Get the DamageIndicator component attached to the same GameObject
        damageIndicator = GetComponent<DamageIndicator>();

        // You could also choose to add DamageIndicator component if it doesn't exist already
        if (damageIndicator == null)
        {
            damageIndicator = gameObject.AddComponent<DamageIndicator>();
        }
    }

    public void TakeDamage(float amount)
    {
        if (!isDamageable()) return;

        HP -= amount;

        // Call IndicateDamage on the DamageIndicator component
        if (damageIndicator != null)
        {
            damageIndicator.IndicateDamage();
        }

        if (HP <= 0)
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
