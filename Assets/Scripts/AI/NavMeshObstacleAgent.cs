using System;
using System.Collections;
using System.ComponentModel.Design;
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
        // make sure timer only ticks when one of agent/obstacle is on. It is possible for both to be off during transition from obstacle to agent
        bool obstacleAgentActive = agent.enabled || obstacle.enabled;
        if (timer > 0 && obstacleAgentActive )
        {
            timer -= Time.deltaTime;
            timer = Math.Max(0, timer);
        }
    }

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
        print("why is obstacle enabled for " + gameObject.name + " " + obstacle.enabled);
        agent.enabled = true;
    }
    
}