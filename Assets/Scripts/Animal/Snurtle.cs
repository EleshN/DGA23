using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snurtle : Animal
{
    [Header("Snurtle Stats")]
    [SerializeField] float emoSpeed = 0.5f;
    [SerializeField] float loveSpeed = 1f;
    [SerializeField] float angerSpeed = 1f;
    [SerializeField] Hitbox hitbox;
    [SerializeField] float attackDelay;
    [Tooltip("The amount of time in seconds that the hitbox is active when attacking")]
    [SerializeField] float hitboxActiveTime;

    [SerializeField] float healthBuffConst = 2f;

    [SerializeField] ParticleSystem ps;

    public override void Start()
    {
        base.Start();
        ps.Pause();
        ps.Clear();
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
            targetTransform = GameManager.Instance.FindClosest(transform.position, GameManager.Instance.TeamEnemy);
        else
        {
            targetPosition = targetTransform.position;
        }
    }

    public override void Attack()
    {
        hitbox.SetDamage(animalDamage);
    }

    /// <summary>
    /// Activates Snurtle's defensive form. Disables movement, multiplies health by healthMultiplier
    /// </summary>
    public void Defend()
    {
        // Disable movement and attack
        damageMultiplier = 0f;

        // Buff Health
        healthMultiplier = healthBuffConst;
    }



}
