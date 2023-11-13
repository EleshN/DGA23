using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : Enemy
{
    [Tooltip("Number of ants stacked on this ant")]
    [SerializeField] int stackVal = 1;

    protected override void Attack()
    {
        //the man behind the slaughter
    }

}
