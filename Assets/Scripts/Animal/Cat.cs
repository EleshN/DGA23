using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cat : Animal
{

    [SerializeField] float debuffRadius;
    [SerializeField] float healthDebuffConst = 0.5f;
    [SerializeField] float damageDebuffConst = 0.5f;

    [SerializeField] ParticleSystem ps;

    public override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(Debuff), 0, 1);
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
        if (targetTransform == null) {
            targetPosition = GameManager.Instance.FindClosest(transform.position, GameManager.Instance.TeamEnemy).position;
        }
        else
        {
            targetPosition = targetTransform.position;
        }
    }

    private void Debuff()
    {
        if (currEmotion != Emotion.EMOTIONLESS)
        {
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, debuffRadius);
            bool isolated = false;

            // Check if there's any GameObject with the "Animal" tag nearby other than 'this'
            foreach (Collider col in nearbyColliders)
            {
                if (col.gameObject.CompareTag("Animal") && col.gameObject != gameObject)
                {
                    if (!ps.isPlaying)
                    {
                        ps.Play();
                    }
                    damageMultiplier = damageDebuffConst;
                    healthMultiplier = healthDebuffConst;
                    isolated = true;
                    break;
                }
            }

            if (!isolated)
            {
                if (ps.isPlaying)
                {
                    ps.Pause();
                    ps.Clear();
                }
                damageMultiplier = 1f;
                healthMultiplier = 1f;
            }
        }
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

}