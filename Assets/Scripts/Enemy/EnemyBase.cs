using System;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class WeightedPair 
{
    public GameObject prefab;
    public int weight;
}

public class EnemyBase : MonoBehaviour, IDamageable
{
    [SerializeField] float health = 100f;
    [SerializeField] HealthBar healthBar;
    private ColorIndicator colorIndicator;

    [Header("Enemy Spawn System")]
    [Tooltip("Delay between each time an enemy spawns")]
    [SerializeField] float minSpawnDelay = 1f;
    [SerializeField] float maxSpawnDelay = 5f;
    [SerializeField] WeightedPair[] enemyWeights;
    [SerializeField] GameObject PlayerBaseObject;

    private int weightSum;
    private int[] weightRange;
    private float nextSpawnTime = 0f;

    void Start()
    {
        GameManager.Instance.Register(this);
        healthBar.SetHealthBar(health);
        healthBar.gameObject.SetActive(false); // hide hp bar when at maximum
        colorIndicator = GetComponent<ColorIndicator>();
        weightRange = new int[enemyWeights.Length];
        weightSum = 0;
        for (int i = 0; i<enemyWeights.Length; i++){
            int weight = enemyWeights[i].weight;
            Assert.IsTrue(weight >= 0, "Spawning system does not support negative weights");
            weightSum += weight;
            weightRange[i] = weightSum;
        }
        resetNextSpawnTime();
    }

    void Update()
    {
        nextSpawnTime -= Time.deltaTime;
        if (nextSpawnTime <= 0)
        {
            resetNextSpawnTime();
            if (GameManager.Instance.WithinEnemySpawnCap())
            {
                int spawnType = UnityEngine.Random.Range(1, weightSum+1);
                for (int i=0; i<enemyWeights.Length; i++){
                    if(spawnType <= weightRange[i]){
                        Vector3 spawnPosition = transform.position;
                        Instantiate(enemyWeights[i].prefab, spawnPosition, Quaternion.identity);
                        return;
                    }
                }
            }
            
        }
    }

    private void resetNextSpawnTime()
    {
        nextSpawnTime = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    public void TakeDamage(float damageAmount, Transform damageSource)
    {
        // this prevents same-frame calls to takeDamage and summons two or more player bases
        if (health <= 0){
            return;
        }
        health -= damageAmount;
        healthBar.gameObject.SetActive(true);
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
