using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSprite : Sprite
{
    // Start is called before the first frame update
    private float spinfactor = .25f;
    private float risefactor = .25f;

    //Rotation benchmarks.
    //Player is below the sprite
    float bottomdist = -24;
    float bottomangle = 45;
    float bottomheight = 3.87f - .28f;
    //Player is even with the sprite
    float evendist = -10;
    float evenangle = 30;
    float evenheight = 3.87f;
    //Player is above the sprite
    float abovedist = -5;
    float aboveangle = 20;
    float aboveheight = 3.87f;

    [SerializeField]
    bool debug = false;

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        Vector3 camPos = base.mainCam.transform.position;
        float xdiff = camPos.x - transform.position.x;
        float zdiff = camPos.z - transform.position.z;
        double newx = xdiff * Math.Cos(.785) - zdiff * Math.Sin(.785); //.785 is 45 in rads
        double newy = zdiff * Math.Cos(.785) + zdiff * Math.Sin(.785);
        //Goes off screen at bottom at about z=-26
        //Even at about z=-10
        //Goes off screen at top at about z=1.5
        //print("In new coords, now at " + newx + "," + newy);
        //newx is like y, newz is like x

        //Rotate
        if (newy < bottomdist)
        {
            if (debug) {
                print("below bottom");
            }
            transform.rotation = Quaternion.Euler(bottomangle, 45, transform.rotation.z);
        }
        else if (newy > bottomdist && newy <= evendist)
        //Player is between the bottom and even benchmarks
        {
            //Compute how far we are
            float numerator = (float)newy - bottomdist;
            float denominator = evendist - bottomdist;
            float proportion = numerator / denominator;

            //Adjust angle
            float baseangle = bottomangle;
            float angledifference = evenangle - bottomangle;

            //Adjust height
            float baseheight = bottomheight;
            float heightdifference = evenheight - bottomheight;

            if (debug)
            {
                print("top statement");
                print("transform position is " + transform.position);
                print("moving to " + new Vector3(transform.position.x, baseheight + heightdifference * proportion));
            }
            transform.rotation = Quaternion.Euler(baseangle + angledifference * proportion, 45, transform.rotation.z);
            transform.position = new Vector3(transform.position.x, baseheight + heightdifference * proportion, transform.position.z);
        }
        else if (newy > evendist && newy <= abovedist)
        //Player is between the even and above benchmarks
        {
            //Compute how far we are
            float numerator = (float)newy - evendist;
            float denominator = abovedist - evendist;
            float proportion = numerator / denominator;

            //Adjust angle
            float baseangle = evenangle;
            float angledifference = aboveangle - evenangle;

            //Adjust height
            float baseheight = evenheight;
            float heightdifference = aboveheight - evenheight;

            if (debug)
            {
                print("bottom statement");
                print("transform position is " + transform.position);
                print("moving to " + new Vector3(transform.position.x, baseheight + heightdifference * proportion));
            }
            transform.rotation = Quaternion.Euler(baseangle + angledifference * proportion, 45, transform.rotation.z);
            transform.position = new Vector3(transform.position.x, baseheight + heightdifference * proportion, transform.position.z);
        }
        else
        //Should be unreachable
        {
            transform.rotation = Quaternion.Euler(aboveangle, 45, transform.rotation.z);
            if (debug) {
                print("above top");
            }
        }
    }
}
