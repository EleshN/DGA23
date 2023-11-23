using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Parrot : Animal
{
    [Header("Parrot Emo Box")]
    public BirdEmoBox BirdEmoBox;

    [Header("Parrot Flight")]
    [SerializeField] bool follow = false;

    [SerializeField] float flightRadius;
    [SerializeField] float flightHeight;
    [SerializeField] float circleDurration;

    float currFlightTime; // the current time of the parabolic animation
    float timeFirstMotion = 0; // time needed for bird to take off
    [SerializeField] Quaternion landingRotation; // the transform of bird at the end of landing

    [SerializeField] Vector3 targetLocationAir;
    [SerializeField] Vector3 initLocation;
    [SerializeField] Vector3 secondinitLocation;
    //[SerializeField] Vector3 currLocation;
    //[SerializeField] float vSpeed;
    //[Tooltip("The inital vertical speed of parrot flight, used for parabolic motion")]
    //[SerializeField] float hSpeed;

    public bool inMotion = false;
    bool atFirstDestination = false;
    bool doneFlight = false;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        // Movement
        switch (currEmotion)
        {
            case Emotion.ANGER:
                if (agent.speed != angerSpeed)
                {
                    agent.speed = angerSpeed;
                    //print("agent anger speed: " + angerSpeed.ToString() + "|" + agent.speed.ToString());
                }
                AngerTarget();
                break;
            case Emotion.LOVE:
                if (agent.speed != loveSpeed)
                {
                    agent.speed = loveSpeed;
                    //print("agent love speed: " + loveSpeed.ToString() + "|" + agent.speed.ToString());
                }
                LoveTarget();
                break;
            default:
                if (agent.speed != emoSpeed)
                {
                    agent.speed = emoSpeed;
                }
                EmoTarget();
                break;
        }

        agent.enabled = !inMotion;
        if (agent.enabled){
            agent.destination = targetPosition;
        }
        //if (!inMotion) agent.destination = targetPosition;

        //print("transform position - y: " + transform.position.y.ToString());
    }

    //-----------------------------// TARGETING //------------------------------//

    /// <summary>
    /// Overrides parent method, sets bird emo box inactive
    /// </summary>
    protected override void EmoTarget()
    {
        print("ughhhhhh");
        base.EmoTarget();
        BirdEmoBox.gameObject.SetActive(false);
    }

    /// <summary>
    /// Flies in a circle of defind radius, spreads Anger, calls "SpreadEmotion"
    /// Set the bird emo box active
    /// </summary>
    public override void AngerTarget() {
        SpreadEmotion(Emotion.ANGER);
    }

    /// <summary> 
    /// Flies in a circle of defind radius, spreads Love, calls "SpreadEmotion"
    /// </summary>
    public override void LoveTarget() {
        if (!follow)
        {
            SpreadEmotion(Emotion.LOVE);
        }
        else if (Vector3.Magnitude(targetTransform.position - transform.position) > loveDistance)
        {
            targetPosition = targetTransform.position;
        }
        else
        {
            targetPosition = transform.position;
        }
    }

    public override bool ApplyEmotionEffect(Emotion emotion, Transform newTarget = null)
    {
        if (GetEmotion() == Emotion.EMOTIONLESS && emotion == Emotion.LOVE)
        {
            follow = true;
            if (newTarget)
            {
                //print("Following: " + newTarget.ToString());
                this.targetTransform = newTarget;
            }
        }
        else
        {
            //print("Set Follow: false");
            follow = false;
        }

        SetEmotion(emotion);
        return true;
        
    }

    //-------------------------// MOVEMENT (helpers) //-------------------------//

    /// <summary>
    /// Flies in a circle of defind radius, spreads the emotion of the parrot
    /// Sets the bird emobox active
    /// </summary>
    /// <param name="targetEmotion"> The emotion the parrot is spreading</param>
    void SpreadEmotion(Emotion targetEmotion)
    {
        BirdEmoBox.gameObject.SetActive(true);

        // Update some variables
        if (!atFirstDestination && !inMotion) // take off (parabolic)
        {
            initLocation = transform.position;
            currFlightTime = 0;
            targetLocationAir = new Vector3(initLocation.x + flightRadius, initLocation.y + flightHeight, initLocation.z);

            atFirstDestination = Vector3.Distance(targetLocationAir, transform.position) < 0.2f;
        }

        doneFlight = (currFlightTime > circleDurration);

        // Perform the motion
        if (!atFirstDestination) // take off (parabolic)
        {
            (timeFirstMotion, _) = ParabolicMotion(agent.speed, targetLocationAir, initLocation);
            atFirstDestination = Vector3.Distance(targetLocationAir, transform.position) < 0.2f;
        }
        else if (!doneFlight) // circular flight
        {
            //print("landing1");
            CircuilarMotion(agent.speed, timeFirstMotion, targetLocationAir.y);
            secondinitLocation = transform.position;

        }
        else if (inMotion) // landing (parabolic)
        {
            //print("Second init location: " + secondinitLocation.ToString());
            (_, landingRotation) = ParabolicMotion(agent.speed, initLocation, secondinitLocation, circleDurration);
            inMotion = Vector3.Distance(initLocation, transform.position) > 0.2f;
        }
        else
        {
            Reset(landingRotation);
        }

    }

    /// <summary>
    /// Moves the bird in parabolic motion
    /// Returns the amount of time needed to do this motion
    /// </summary>
    /// <param name="hSpeed"> Speed of the animal depending on emotion</param>
    /// <param name="targetLocation"></param>
    /// <param name="initLocation"></param>
    (float, Quaternion) ParabolicMotion(float hSpeed, Vector3 targetLocation, Vector3 initLocation, float timeOffSet = 0f)
    {
        //print("init location: " + initLocation.ToString());
        inMotion = true;
        currFlightTime += Time.deltaTime;

        // gets x,y, z displacement between final and inital location 
        float dx = targetLocation.x - initLocation.x;
        float dz = targetLocation.z - initLocation.z;
        float dy = targetLocation.y - initLocation.y;
        //if (dy < 0 && vSpeed > 0) vSpeed = -(vSpeed);

        // point velocity vector to movement direction
        Vector3 velocity = (targetLocation - initLocation).normalized * hSpeed;

        // gets horizontal displacement between final and inital location 
        //float dh = Mathf.Sqrt(Mathf.Pow(dx, 2f) + Mathf.Pow(dz, 2f));

        // gets the time needed to arrive to final location
        float dt = (targetLocation - initLocation).magnitude / hSpeed;

        // calculate the needed vertical acceleration to fly to a certain location
        //float vAcc = 2 * (dy - vSpeed * dt) / (Mathf.Pow(dt, 2f)); // dy = ut + 1/2at^2
        //float vAcc = - (Mathf.Pow(vSpeed, 2f)/(2 * dy));
        //float vAcc = -(vSpeed / dt);

        // find y by: y = y0+ 1/2 a t^2
        // float changeX = dx / dh * hSpeed * (currFlightTime - timeOffSet);
        // float changeY = vSpeed * (currFlightTime - timeOffSet) + 0.5f * vAcc * Mathf.Pow(currFlightTime - timeOffSet, 2f);
        // float changeZ = dz / dh * hSpeed * (currFlightTime - timeOffSet);

        //print("update: " + changeX.ToString() + "," + changeY.ToString() + "," + changeZ.ToString());

        transform.position = initLocation + velocity * (currFlightTime - timeOffSet);//new Vector3(initLocation.x + changeX,
                                            //initLocation.y + changeY,
                                            //(initLocation.z + changeZ));
        LookAt(dx, dy, dz);
        return (dt, transform.rotation);
    }

    void CircuilarMotion(float hSpeed, float timeOffset, float yPos)
    {
        inMotion = true;

        currFlightTime += Time.deltaTime * (hSpeed/flightRadius);

        float x = flightRadius * Mathf.Cos(currFlightTime - timeOffset);
        float y = 0;
        float z = flightRadius * Mathf.Sin(currFlightTime - timeOffset);

        transform.position = new Vector3(initLocation.x + x,
                                            yPos + y,
                                            initLocation.z + z);
        LookAt(-z, y, x);
    }

    private void Reset(Quaternion resetRotation)
    {
        LookAt(resetRotation.x, 0, resetRotation.z);
        SetEmotion(Emotion.EMOTIONLESS);
        inMotion = false;
        atFirstDestination = false;
        doneFlight = false;
        //vSpeed = Mathf.Abs(vSpeed);
    }

    private void LookAt(float x, float y, float z)
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(x,y,z));
    }

    //------------------------------// EMOTIONS //------------------------------//


    //-------------------------------// COMBAT //-------------------------------//

    public override void Attack() { }

    public override void TakeDamage(float damage, Transform source){}

    //-----------------------------// COLLISION //-----------------------------//

    ///// <summary>
    ///// Applies amotion 
    ///// </summary>
    ///// <param name="collision"></param>
    //private void OnTriggerEnter(Collider collision)
    //{
    //    if (collision.gameObject.CompareTag("Animal")) {
    //        collision.gameObject.GetComponent<Animal>().ApplyEmotionEffect(currEmotion);
    //    }
    //}

}
