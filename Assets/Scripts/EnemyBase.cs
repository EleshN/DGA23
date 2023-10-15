using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    public float health = 100f;
    public Slider healthBar;
    public GameObject enemyPrefab;
    public GameObject PlayerBaseObject;
    public float spawnHeight = 2f; // Height from which the enemy will be spawned
    public Vector2 newSize = new Vector2(1,1);
    public float nextSpwanTime = 0f;

    void Start()
    {
        GameManager.Instance.EnemyBases.Add(this);
        GameManager.Instance.TeamEnemy.Add(gameObject.transform);
        
        RectTransform rectTransform = healthBar.GetComponent<RectTransform>();
        rectTransform.sizeDelta = newSize;
        healthBar.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        UpdateHealthBar();
        
        resetNextSpawnTime();
    }
    
    void Update()
    {
        nextSpwanTime -= Time.deltaTime;
        if (nextSpwanTime <= 0)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, spawnHeight, transform.position.z);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            resetNextSpawnTime();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            health += 10;
            UpdateHealthBar();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            health -= 10;
            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        health = Mathf.Clamp(health, 0, 100);
        healthBar.value = health / 100f;

        if (health <= 0)
        {
            GameManager.Instance.EnemyBases.Remove(this);
            GameManager.Instance.TeamEnemy.Remove(gameObject.transform);
            Instantiate(PlayerBaseObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void resetNextSpawnTime()
    {
        nextSpwanTime = Random.Range(1f, 5f);
    }
}
