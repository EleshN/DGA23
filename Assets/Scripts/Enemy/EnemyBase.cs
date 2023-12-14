using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

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
    
    public WeightedPair[] enemyWeights;
    public GameObject[] enemyPrefabs;
    public GameObject PlayerBaseObject;
    float spawnHeight = 0f; // Height from which the enemy will be spawned
    // public Vector2 newSize = new Vector2(1,1);
    float nextSpawnTime = 0f;

    private ColorIndicator colorIndicator;
    private int weightSum;
    private int[] weightRange;
    private int spwanType;


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
                spwanType = UnityEngine.Random.Range(1, weightSum+1);
                for (int i=0; i<enemyWeights.Length; i++){
                    if(spwanType <= weightRange[i]){
                        Vector3 spawnPosition = new Vector3(transform.position.x-0.5f, spawnHeight, transform.position.z-1);
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
