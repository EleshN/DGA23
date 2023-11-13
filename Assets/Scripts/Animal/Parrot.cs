using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    float timeFirstMotion = 0; // Time needed for bird to take off
    [SerializeField] Vector3 targetLocationAir;
    [SerializeField] Vector3 initLocation;
    [SerializeField] Vector3 secondinitLocation;
    //[SerializeField] Vector3 currLocation;
    [Tooltip("The inital vertical speed of parrot flight, used for parabolic motion")]
    [SerializeField] float vSpeed;
    [SerializeField] float hSpeed;

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
                if (agent.speed != angerSpeed) agent.speed = angerSpeed;
                AngerTarget();
                BirdEmoBox.gameObject.SetActive(true);
                break;
            case Emotion.LOVE:
                if (agent.speed != loveSpeed) agent.speed = loveSpeed;
                LoveTarget();
                BirdEmoBox.gameObject.SetActive(true);
                break;
            default:
                if (agent.speed != emoSpeed) agent.speed = emoSpeed;
                EmoTarget();
                BirdEmoBox.gameObject.SetActive(false);
                break;
        }

        if (follow) agent.destination = targetPosition;
        //print("transform position - y: " + transform.position.y.ToString());
    }

    //-----------------------------// TARGETING //------------------------------//

    /// <summary>
    /// Flies in a circle of defind radius, spreads Anger, calls "SpreadEmotion"
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
    /// </summary>
    /// <param name="targetEmotion"> The emotion the parrot is spreading</param>
    void SpreadEmotion(Emotion targetEmotion)
    {

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
            timeFirstMotion = ParabolicMotion(agent.speed, targetLocationAir, initLocation);
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
            ParabolicMotion(agent.speed, initLocation, secondinitLocation, circleDurration);
            inMotion = Vector3.Distance(initLocation, transform.position) > 0.2f;
        }
        else
        {
            Reset();
        }

    }

    /// <summary>
    /// Moves the bird in parabolic motion
    /// Returns the amount of time needed to do this motion
    /// </summary>
    /// <param name="hSpeed"> Speed of the animal depending on emotion</param>
    /// <param name="targetLocation"></param>
    /// <param name="initLocation"></param>
    float ParabolicMotion(float hSpeed, Vector3 targetLocation, Vector3 initLocation, float timeOffSet = 0f)
    {
        //print("init location: " + initLocation.ToString());
        inMotion = true;
        currFlightTime += Time.deltaTime;

        // gets x,y, z displacement between final and inital location 
        float dx = targetLocation.x - initLocation.x;
        float dz = targetLocation.z - initLocation.z;
        float dy = targetLocation.y - initLocation.y;
        if (dy < 0 && vSpeed > 0) vSpeed = -(vSpeed);

        // gets horizontal displacement between final and inital location 
        float dh = Mathf.Sqrt(Mathf.Pow(dx, 2f) + Mathf.Pow(dz, 2f));

        // gets the time needed to arrive to final location
        float dt = dh / hSpeed;

        // calculate the needed vertical acceleration to fly to a certain location
        float vAcc = 2 * (dy - vSpeed * dt) / (Mathf.Pow(dt, 2f)); // dy = ut + 1/2at^2
        //print("vAcc: " + vAcc.ToString());

        // find y by: y = y0+ 1/2 a t^2
        float changeX = dx / dh * hSpeed * (currFlightTime - timeOffSet);
        float changeY = vSpeed * (currFlightTime - timeOffSet) + 0.5f * vAcc * Mathf.Pow(currFlightTime - timeOffSet, 2f);
        float changeZ = dz / dh * hSpeed * (currFlightTime - timeOffSet);

        //print("update: " + changeX.ToString() + "," + changeY.ToString() + "," + changeZ.ToString());

        transform.position = new Vector3(initLocation.x + changeX,
                                            initLocation.y + changeY,
                                            (initLocation.z + changeZ));
        LookAt(changeX, changeY, changeZ);
        return dt;
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

    private void Reset()
    {
        LookAt(transform.rotation.x, 0, transform.rotation.z);
        SetEmotion(Emotion.EMOTIONLESS);
        inMotion = false;
        atFirstDestination = false;
        doneFlight = false;
        vSpeed = Mathf.Abs(vSpeed);
    }

    private void LookAt(float x, float y, float z)
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(x,y,z));
    }

    //------------------------------// EMOTIONS //------------------------------//


    //-------------------------------// ATTACK //-------------------------------//

    /// <summary>
    /// Does nothing
    /// </summary>
    public override void Attack() { }

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
