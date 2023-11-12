using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : Sprite
{
    //The animator controller attached to the player
    [SerializeField]
    Animator anim;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //print("h" + horizontal);
        //print("v " + vertical);
        

        if (vertical > 0)
        {
            anim.SetBool("Upwards", true);
            anim.SetBool("Sideways", false);
        }
        else {
            anim.SetBool("Upwards", false);
            //Sideways motion only enabled for facing forward
            if (horizontal > 0)
            {
                anim.SetBool("Sideways", true);
                anim.SetBool("Left", false);
            }
            else if (horizontal < 0)
            {
                anim.SetBool("Sideways", true);
                anim.SetBool("Left", true);
            }
            else
            {
                anim.SetBool("Sideways", false);
            }
        }
        transform.rotation = Quaternion.Euler(transform.parent.rotation.x * -1.0f, transform.parent.rotation.y * -1.0f, transform.parent.rotation.z * -1.0f);

    }
}
