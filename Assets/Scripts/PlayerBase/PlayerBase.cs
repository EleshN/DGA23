using UnityEngine;

public class PlayerBase : MonoBehaviour, IDamageable
{
    [Range(0.1f, 10.0f)]
    public float RegenTickSpeed;
    [SerializeField] float health;
    [SerializeField] HealthBar healthBar;
    public GameObject EnemyBaseObject;

    private ColorIndicator colorIndicator;

    public void Start()
    {
        GameManager.Instance.Register(this);
        healthBar.SetHealthBar(health);
        healthBar.gameObject.SetActive(false);
        colorIndicator = GetComponent<ColorIndicator>();
        GameManager.Instance.ValidEnemyTargets.Add(transform);
    }
    public void TakeDamage(float amount, Transform damageSource)
    {
        // this prevents same-frame calls to takeDamage and summons two or more enemy bases
        if (health <= 0){
            return;
        }
        health -= amount;
        healthBar.gameObject.SetActive(true);
        healthBar.UpdateHealthBar(health);
        colorIndicator.IndicateDamage();

        if (health <= 0)
        {
            GameManager.Instance.Unregister(this);
            Instantiate(EnemyBaseObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
