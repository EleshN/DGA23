using UnityEngine;
using System.Collections;

public class Dog : Animal
{
    public override void Update()
    {
        base.Update();

        //check radius from other animals.  If animal close, reduce damage and max health
    }

    public override void LoveTarget()
    {
        if (targetTransform != GameManager.Instance.PlayerTransform)
        {
            targetTransform = GameManager.Instance.PlayerTransform;
        }
        if(Vector3.Magnitude(targetTransform.position - transform.position) > loveDistance)
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
}
