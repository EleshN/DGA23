using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour, IDamageable
{
    [SerializeField] float health = 100f;
    [SerializeField] HealthBar healthBar;
    // public Slider healthBar;
    public GameObject[] enemyPrefabs;
    public GameObject PlayerBaseObject;
    float spawnHeight = 0f; // Height from which the enemy will be spawned
    // public Vector2 newSize = new Vector2(1,1);
    float nextSpawnTime = 0f;

    private ColorIndicator colorIndicator;


    [Header("Enemy Spawning")]
    [Tooltip("Delay between each time an enemy spawns")]
    [SerializeField] float minSpawnDelay = 1f;
    [SerializeField] float maxSpawnDelay = 5f;

    void Start()
    {
        GameManager.Instance.Register(this);

        // RectTransform rectTransform = healthBar.GetComponent<RectTransform>();
        // rectTransform.sizeDelta = newSize;
        // healthBar.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        healthBar.SetHealthBar(health);
        colorIndicator = GetComponent<ColorIndicator>();
        resetNextSpawnTime();
    }

    void Update()
    {
        nextSpawnTime -= Time.deltaTime;
        if (nextSpawnTime <= 0)
        {
            if (GameManager.Instance.WithinEnemySpawnCap())
            {
                //just spwans the first one for now
                Vector3 spawnPosition = new Vector3(transform.position.x, spawnHeight, transform.position.z);
                Instantiate(enemyPrefabs[0], spawnPosition, Quaternion.identity);
            }

            resetNextSpawnTime();
        }
    }

    private void resetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    public void TakeDamage(float damageAmount, Transform damageSource)
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health);
        colorIndicator.IndicateDamage();
        if (health <= 0)
        {
            GameManager.Instance.Unregister(this);
            Instantiate(PlayerBaseObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
