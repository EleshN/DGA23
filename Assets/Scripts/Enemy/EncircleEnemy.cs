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
  [SerializeField] private float attackDelay = 1f;
  [SerializeField] private float hitboxActiveTime = 2f;

  private float angle;
  private bool firstCircleCompleted = false;
  private bool isDrawingCircle = false;

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

    if (state == EnemyState.CHASE || state == EnemyState.ATTACK)
    {
      EncircleTarget();
    }
  }

  private void EncircleTarget()
  {
    if (targetTransform != null)
    {
      angle += encircleSpeed * Time.deltaTime;

      Vector3 direction = (transform.position - targetTransform.position).normalized;
      direction = Quaternion.Euler(0, angle, 0) * direction;
      Vector3 newPosition = targetTransform.position + direction * encircleRadius;

      agent.SetDestination(newPosition);

      if (!isDrawingCircle)
      {
        DrawCirclePath();
      }

      if (angle >= 360f)
      {
        if (firstCircleCompleted)
        {
          StartCoroutine(PerformAttack());
        }
        else
        {
          firstCircleCompleted = true;
        }
        angle -= 360f; // Reset the angle for the next encircling
      }
    }
  }

  private void DrawCirclePath()
  {
    for (int i = 0; i <= segments; i++)
    {
      float radian = 2 * Mathf.PI * i / segments;
      lineRenderer.SetPosition(i, new Vector3(Mathf.Sin(radian) * encircleRadius, 0, Mathf.Cos(radian) * encircleRadius));
    }
  }

  IEnumerator PerformAttack()
  {
    isDrawingCircle = true;
    Material originalMaterial = lineRenderer.material;
    lineRenderer.material = attackMaterial;
    lineRenderer.loop = true;

    Attack();

    yield return new WaitForSeconds(0.5f); // Duration for the red color

    lineRenderer.material = originalMaterial;
    isDrawingCircle = false;
  }

  protected override void Attack()
  {
    if (currentAtackTime <= 0)
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

      currentAtackTime = attackCountDown; // Resetting attack cooldown
    }
  }
}
