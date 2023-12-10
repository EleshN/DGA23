using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class NavMeshObstacleAgent : MonoBehaviour
{
    NavMeshAgent agent;
    NavMeshObstacle obstacle;

    [Tooltip("the duration to stop and become obstacle until transitioning to agent mode")]
    public float stopTime = 2;

    float timer;

    /// <summary>
    /// NavMeshAgent component velocity
    /// </summary>
    public Vector3 Velocity { get { return agent.velocity; } }

    /// <summary>
    /// NavMeshAgent component speed
    /// </summary>
    public float Speed { get { return agent.speed;} set { agent.speed = value;}}

    /// <summary>
    /// NavMeshAgent component destination. Attempts to set destination when agent is inactive will be ignored.
    /// </summary>
    public Vector3 Destination {get {return agent.destination;} set {if (agent.enabled){ agent.SetDestination(value);} }}

    /// <summary>
    /// NavMeshAgent component stopping distance
    /// </summary>
    public float StoppingDistance { get { return agent.stoppingDistance;} set { agent.stoppingDistance = value;}}

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
        obstacle.carving = true;
        agent.enabled = true;
        timer = 0;
    }

    public void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            timer = Math.Max(0, timer);
        }
    }


    // public void SetSpeed(float speed)
    // {
    //     if (agent.enabled) 
    //     {
    //         agent.speed = speed;
    //         this.speed = speed;
    //     }
    // }

    // public float GetSpeed()
    // {
    //     if (agent.enabled) 
    //     {
    //         return agent.speed;
    //     }
    //     return speed;
    // }

    // public void SetStoppingDistance(float dist)
    // {
    //     if (agent.enabled) 
    //     {
    //         agent.stoppingDistance = dist;
    //     }
    // }


    // public void SetDestination(Vector3 destination)
    // { 
    //     if (agent.enabled)
    //     {
    //         agent.SetDestination(destination);
    //     }
    // }

    public void SetAgentMode(){
        if (agent.enabled){
            return;
        }
        if (timer <= 0)
        {
            timer = stopTime;
            obstacle.enabled = false;
            StartCoroutine(EnableNavMeshAgent());
        }
    }

    public void SetObstacleMode(){
        if (obstacle.enabled){
            return;
        }
        if (timer <= 0)
        {
            timer = stopTime;
            agent.enabled = false;
            obstacle.enabled = true;
        }
    }

    /// <summary>
    /// turns on the navmesh agent, after making sure the navmesh obstacle is turned off for at least 1 frame.
    /// </summary>
    /// <returns></returns>
    IEnumerator EnableNavMeshAgent()
    {
        //delay for one frame
        yield return 0;
        agent.enabled = true;
    }
    
}