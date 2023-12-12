using UnityEngine;
using System.Collections;

public class EncircleEnemy : Enemy
{
  [Header("Encircle Enemy Specifics")]
  [SerializeField] private float encircleRadius = 5f;
  [SerializeField] private float encircleSpeed = 2f; // Speed at which the enemy moves around the circle
  [SerializeField] private LineRenderer lineRenderer;
  [SerializeField] private int segments = 50; // Number of segments to create a circle
  [SerializeField] private Material attackMaterial; // Material used when attacking

  private float angle;
  // private bool firstCircleCompleted = false;
  // private bool isDrawingCircle = false;

  protected override void Start()
  {
    base.Start();
    InitializeLineRenderer();
  }

  private void InitializeLineRenderer()
  {
    lineRenderer.positionCount = segments + 1;
    lineRenderer.useWorldSpace = false;
  }

  protected override void Update()
  {
    base.Update();
    // if whithin radius, attack:
    if (Vector3.Distance(transform.position, targetTransform.position) <= encircleRadius)
    {
      state = EnemyState.ATTACK;
      EncircleTarget();
    }
  }

  private Vector3 startPosition;
  [SerializeField] private float returnErrorMargin = 1.0f; // Error margin for returning to the start position

  private void EncircleTarget()
  {
    if (targetTransform != null)
    {
      // Calculate the angle increment based on time and encircle speed
      angle += encircleSpeed * Time.deltaTime;

      // Calculate the direction vector from the target to the enemy and rotate it
      Vector3 direction = (transform.position - targetTransform.position).normalized;
      direction = Quaternion.Euler(0, angle, 0) * direction;
      Vector3 newPosition = targetTransform.position + direction * encircleRadius;

      // Set the NavMeshAgent's destination to the new position
      agent.Destination = newPosition;

      // Check if the enemy has returned to the near vicinity of the start position
      if (Vector3.Distance(startPosition, transform.position) <= returnErrorMargin && angle >= 360f)
      {
        Attack();
        angle = 0; // Reset the angle for the next encircling
      }
    }
  }




  private void DrawCirclePath()
  {
    lineRenderer.useWorldSpace = true; // Use world space for correct positioning
    Vector3 enemyPosition = transform.position;
    float yOffset = -0.5f; // Adjust this value to set how far below the enemy the line should be drawn

    for (int i = 0; i <= segments; i++)
    {
      float radian = 2 * Mathf.PI * i / segments;
      lineRenderer.SetPosition(i, new Vector3(enemyPosition.x + Mathf.Sin(radian) * encircleRadius, enemyPosition.y - yOffset, enemyPosition.z + Mathf.Cos(radian) * encircleRadius));
    }
  }


  IEnumerator PerformAttack()
  {
    // isDrawingCircle = true;
    Material originalMaterial = lineRenderer.material;
    lineRenderer.material = attackMaterial;
    lineRenderer.loop = true;

    Attack();

    yield return new WaitForSeconds(0.5f); // Duration for the red color

    lineRenderer.material = originalMaterial;
    // isDrawingCircle = false;
  }

  protected override void Attack()
  {
    Collider[] hitColliders = Physics.OverlapSphere(transform.position, encircleRadius);
    foreach (var hitCollider in hitColliders)
    {
      if (hitCollider.gameObject == this.gameObject) continue; // Skip self
      IDamageable damageable = hitCollider.GetComponent<IDamageable>();
      if (damageable != null)
      {
        damageable.TakeDamage(robotDamage, transform);
      }
    }
  
  }
}
