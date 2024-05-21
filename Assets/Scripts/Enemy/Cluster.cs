using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster : MonoBehaviour
{
    [SerializeField]
    List<GameObject> pieces;

    [SerializeField]
    int timeToWait = 3; //The time between when you spawn the pieces and when they disappear
    private float currentTime = 0;

    [Header("Physics Params")]
    [SerializeField]
    int minYVel;
    [SerializeField]
    int maxYVel;
    [SerializeField]
    int minHorizVel;
    [SerializeField]
    int maxHorizVel;
    

    // Start is called before the first frame update
    void Start()
    {
        currentTime = timeToWait;

        //Launch each piece in different directions
        bool everyOther = false;
        foreach (GameObject g in pieces) {
            
            //Add y velocities
            float yVel = Random.Range(minYVel, maxYVel);
            //Send them in different horizontal directions
            int mult = 1;
            if (everyOther) {
                mult = -1;
            }
            // Add horizontal force
            float horizMult = Random.Range(minHorizVel, maxHorizVel);
            // X and Y are opposite because those are the horizontal directions in this game
            float xVel = 1 * horizMult * mult;
            float zVel = -1 * horizMult * mult;
            //Set the velocity. Make sure gravity is on
            //print("Adding x vel " + xVel + " and y vel " + yVel + " and z vel " + zVel);
            g.GetComponent<Rigidbody>().velocity = new Vector3(xVel, yVel, zVel);

            everyOther = !everyOther;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime <= 0)
        {
            foreach (GameObject g in pieces)
            {
                Destroy(g.gameObject);
            }
            Destroy(this.gameObject);
        }
        else {
            currentTime -= Time.deltaTime;
        }
    }
}
