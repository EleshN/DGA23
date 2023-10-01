using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    public float health = 100f;
    public Slider healthBar;
    public GameObject enemyPrefab;
    public float spawnHeight = 2f; // Height from which the enemy will be spawned

    void Update()
    {
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

        healthBar.transform.position = transform.position + new Vector3(0, 1.5f, 0);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, spawnHeight, transform.position.z);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
