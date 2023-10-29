using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cat : Animal
{

    [SerializeField] float debuffRadius;
    public override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(Isolation), 0, 1);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void LoveTarget()
    {
        if (targetTransform != GameManager.Instance.PlayerTransform)
        {
            targetTransform = GameManager.Instance.PlayerTransform;
        }
        if (Vector3.Magnitude(targetTransform.position - transform.position) > loveDistance)
        {
            targetPosition = targetTransform.position;
        }
        else
        {
            targetPosition = transform.position;
        }
    }

    public override void AngerTarget()
    {
        if (targetTransform == null)
            GameManager.Instance.FindClosest(transform.position, GameManager.Instance.TeamEnemy);
        else
        {
            targetPosition = targetTransform.position;
        }
    }

    private void Isolation()
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, debuffRadius);
        bool isolated = false;

        // Check if there's any GameObject with the "Animal" tag nearby other than 'this'
        foreach (Collider col in nearbyColliders)
        {
            if (col.gameObject.CompareTag("Animal") && col.gameObject != gameObject)
            {
                damageMultiplier = 0.5f;
                healthMultiplier = 0.5f;
                isolated = true;
                break;
            }
        }

        if (!isolated)
        {
            damageMultiplier = 1f;
            healthMultiplier = 1f;
        }
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

}