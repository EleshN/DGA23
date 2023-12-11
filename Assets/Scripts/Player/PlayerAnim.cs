using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : Sprite
{
    //The animator controller attached to the player
    [SerializeField]
    Animator anim;
    private float horizontal;
    private float vertical;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!PauseGame.isPaused)
        {
            base.Update();
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            //print("h" + horizontal);
            //print("v " + vertical);
        }

        //See if player is even moving
        if (horizontal == 0 && vertical == 0)
        {
            anim.SetBool("Moving", false);
            anim.SetBool("Sideways", false);
        }
        else
        {
            anim.SetBool("Moving", true);
            //See if they are moving to the side
            if (horizontal == 0)
            {
                anim.SetBool("Sideways", false);
            }
            else {
                //See which direction they are moving
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
            }
            //See if they are moving up or down
            if (vertical > 0)
            {
                anim.SetBool("Upwards", true);
            }
            else
            {
                anim.SetBool("Upwards", false);
            } 
        }
        transform.rotation = Quaternion.Euler(transform.parent.rotation.x * -1.0f, transform.parent.rotation.y * -1.0f, transform.parent.rotation.z * -1.0f);

    }
}
