using UnityEngine;
using UnityEngine.UI; 

public class PlayerBase : MonoBehaviour, IDamageable
{
    [Range(0.1f, 10.0f)]
    public float RegenTickSpeed;
    [SerializeField] float health;
    [SerializeField] HealthBar healthBar;
    public GameObject EnemyBaseObject;
    public Image damageDirectionIndicator; 

    private ColorIndicator colorIndicator;
    private Camera mainCamera; 

    public AudioClip notifSound;

    void Start()
    {
        GameManager.Instance.Register(this);
        healthBar.SetHealthBar(health);
        healthBar.gameObject.SetActive(false);
        colorIndicator = GetComponent<ColorIndicator>();
        GameManager.Instance.ValidEnemyTargets.Add(transform);
        mainCamera = Camera.main; // Cache main camera
        damageDirectionIndicator.enabled = false; // Initially disable the indicator

        
    }

    private void Awake()
    {
        //Set rotation to 0
        transform.rotation = Quaternion.identity;
    }

    public void TakeDamage(float amount, Transform damageSource)
    {
        if (health <= 0)
        {
            return;
        }
        health -= amount;
        healthBar.gameObject.SetActive(true);
        healthBar.UpdateHealthBar(health);
        colorIndicator.IndicateDamage();

        UpdateDamageIndicator(damageSource.position);

        if (health <= 0)
        {
            GameManager.Instance.Unregister(this);
            Instantiate(EnemyBaseObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void UpdateDamageIndicator(Vector3 damageSourcePosition)
{
    // Calculate the distance between the player and the damage source
    float distanceToSource = Vector3.Distance(transform.position, damageSourcePosition);

    // Set the minimum distance required to draw the indicator
    float minDrawDistance = 1f; // Adjust this value as needed

    if (distanceToSource >= minDrawDistance)
    {
        Vector3 screenPos = mainCamera.WorldToViewportPoint(damageSourcePosition);
        // Normalize position within the viewport
        screenPos.x = Mathf.Clamp01(screenPos.x);
        screenPos.y = Mathf.Clamp01(screenPos.y);

        // Determine the closest screen edge and apply a consistent margin inside the viewport
        float edgeMargin = 0.08f; // Adjusted to be slightly off the edge
        Vector3 indicatorPos = screenPos;
        if (screenPos.x <= edgeMargin) indicatorPos.x = edgeMargin;
        else if (screenPos.x >= 1 - edgeMargin) indicatorPos.x = 1 - edgeMargin;
        if (screenPos.y <= edgeMargin) indicatorPos.y = edgeMargin;
        else if (screenPos.y >= 1 - edgeMargin) indicatorPos.y = 1 - edgeMargin;

        // Check if the damage source is behind the camera
        Vector3 directionToSource = damageSourcePosition - mainCamera.transform.position;
        if (Vector3.Dot(mainCamera.transform.forward, directionToSource) < 0)
        {
            // Damage source is behind the camera, flip the indicator position
            if (indicatorPos.x < 0.5f) indicatorPos.x = 1 - edgeMargin;
            else indicatorPos.x = edgeMargin;
            if (indicatorPos.y < 0.5f) indicatorPos.y = 1 - edgeMargin;
            else indicatorPos.y = edgeMargin;
        }

        // Convert to screen position and update the indicator's position and visibility
        damageDirectionIndicator.rectTransform.anchorMin = indicatorPos;
        damageDirectionIndicator.rectTransform.anchorMax = indicatorPos;
        damageDirectionIndicator.rectTransform.anchoredPosition = Vector2.zero;
        damageDirectionIndicator.enabled = true;
    }
    else
    {
        // If the player is too close to the damage source, hide the indicator
        damageDirectionIndicator.enabled = false;
    }
}


}
