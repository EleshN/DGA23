using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _Instance;
    public int LevelNumber = 1;
    public List<PlayerBase> PlayerBases { get; private set; }
    public List<EnemyBase> EnemyBases { get; private set; }

    // All enemies on the map (robots, enemy bases)
    public List<Transform> TeamEnemy;

    // Reference to Player Transform for player target tracking
    public Transform PlayerTransform;

    public static GameManager Instance
    {
        get
        {
            if (_Instance == null)
                Debug.LogError("GameManager Non Existent");
            return _Instance;
        }
    }

    private void Awake()
    {
        _Instance = this;
        PlayerBases = new();
        EnemyBases = new();
        TeamEnemy = new();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerBases.Count == 0) Debug.Log("Game Over");
        if (EnemyBases.Count == 0) Debug.Log("You Win");
    }

    public Transform FindClosest(Vector3 source, List<Transform> listTransforms)
    {
        // find closest using Euclidean distance
        Transform closest = null;
        float minDist = float.PositiveInfinity;
        foreach (Transform target in listTransforms)
        {
            Vector3 targetPosition = target.position;
            float distance = (targetPosition - source).magnitude;
            if ( distance < minDist)
            {
                minDist = distance;
                closest = target;
            }
        }
        return closest;
    }
}
