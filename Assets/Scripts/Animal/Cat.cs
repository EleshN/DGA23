using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cat : Animal
{
    public override void Update()
    {
        base.Update();

        Isolation();
    }

    public override void LoveTarget()
    {
        if (targetTransform != GameManager.Instance.PlayerTransform)
        {
            targetTransform = GameManager.Instance.PlayerTransform;
            print("nope");
        }
        if (Vector3.Magnitude(targetTransform.position - transform.position) > loveDistance)
        {
            targetPosition = targetTransform.position;
        }
        else
        {
            print("close");
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
     //currently no way to test this
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, 5f);

        // Check if there's any GameObject with the "Animal" tag nearby other than 'this'
        Collider foundAnimal = System.Array.Find(nearbyColliders, collider => collider.CompareTag("Animal") && collider.gameObject != this.gameObject);

        if (foundAnimal != null)
        {
            this.damageMultiplier = 0.5f;
            this.healthMultiplier = 0.5f;
        }
    }

}
