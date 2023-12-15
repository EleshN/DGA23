using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : Sprite
{
    // archived

    
    // private float lastPos;

    // protected override void Update()
    // {
    //     //Enemies don't have rigidbody, so velocity / direction must be calculated manually
    //     float deltaX = transform.position.x - lastPos;
    //     lastPos = transform.position.x;

    //     Vector3 newScale = transform.localScale;
    //     if (deltaX > 0)
    //     {
    //         //Make sprite face right if moving right
    //         newScale.x = Mathf.Abs(newScale.x);
    //     }
    //     else if (deltaX < 0) {
    //         //Make sprite face left if moving left
    //         newScale.x = -1 * Mathf.Abs(newScale.x);
    //     }
    //     //If x velocity is zero, leave it how it was
    //     transform.localScale = newScale;
    //     base.Update();
    // }
}
