using System;
using UnityEngine;
using UnityEngine.AI;


public class NavMeshObstacleAgent : MonoBehaviour
{
    NavMeshAgent agent;
    NavMeshObstacle obstacle;

    [Tooltip("the duration to stop and become obstacle until transitioning to agent mode")]
    public float stopTime = 2;

    float timer;

    float speed;

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

    public void SetSpeed(float speed)
    {
        if (agent.enabled) 
        {
            agent.speed = speed;
            this.speed = speed;
        }
    }

    public float GetSpeed()
    {
        if (agent.enabled) 
        {
            return agent.speed;
        }
        return speed;
    }

    public void SetStoppingDistance(float dist)
    {
        if (agent.enabled) 
        {
            agent.stoppingDistance = dist;
        }
    }


    public void SetDestination(Vector3 destination)
    { 
        if (agent.enabled)
        {
            agent.SetDestination(destination);
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
            agent.enabled = true;
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
    
}