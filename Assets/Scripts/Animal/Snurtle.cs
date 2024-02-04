using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snurtle : Animal
{
    [Header("Snurtle Attack Stats")]
    [SerializeField] Hitbox hitbox;

    [Tooltip("the entities that the snurtle can attak")]
    [SerializeField] Tag[] targets;
    [SerializeField] float attackDelay; //TODO

    [Tooltip("The amount of time in seconds that the hitbox is active when attacking")]
    [SerializeField] float hitboxActiveTime; //TODO


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        hitbox.Initialize();
        hitbox?.SetUniformDamage(targets, animalDamage * damageMultiplier);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator SnurtleAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        // code to play sound
        hitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(hitboxActiveTime);
        hitbox.gameObject.SetActive(false);
    }
}
