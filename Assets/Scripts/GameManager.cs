using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _Instance;
    public int LevelNumber = 1;

    /// <summary>
    /// All player bases currently on the map
    /// </summary>
    private HashSet<PlayerBase> PlayerBases;

    /// <summary>
    /// All enemy bases currently on the map
    /// </summary>
    private HashSet<EnemyBase> EnemyBases;

    /// <summary>
    /// All animal gameobjects
    /// </summary>
    private HashSet<Animal> Animals;

    /// <summary>
    /// Set of animals currently in loved with the player
    /// </summary>
    public HashSet<Animal> followers;

    /// <summary>
    /// All enemies currently on the map (robots, enemy bases)
    /// </summary>
    public HashSet<Transform> TeamEnemy;

    /// <summary>
    /// All player-side entities currently on the map
    /// </summary>
    public HashSet<Transform> TeamPlayer;
    

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
        TeamPlayer = new();
        Animals = new();
        followers = new();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        // PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerBases.Count == 0)
        {
            // you lose
        }
        if (EnemyBases.Count == 0)
        {
           // you win
        }
    }

    public Transform FindClosest(Vector3 source, HashSet<Transform> transforms)
    {
        // find closest using Euclidean distance
        Transform closest = null;
        float minDist = float.PositiveInfinity;
        foreach (Transform target in transforms)
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

    public GameObject FindClosestTargetForEnemmy(Enemy e)
    {
        GameObject closest = null;
        Vector3 source = e.gameObject.transform.position;
        float minDist = float.PositiveInfinity;
        // check each animal, whether they can be attacked: if yes, consider the animal as a candidate.
        foreach (Animal animal in Animals)
        {
            if(animal.currEmotion == Emotion.ANGER)
            {
                Vector3 targetPosition = animal.transform.position;
                float distance = (targetPosition - source).magnitude;
                if (distance < minDist)
                {
                    minDist = distance;
                    closest = animal.gameObject;
                }
            }
            
        }
        foreach (PlayerBase playerBase in PlayerBases)
        {
            Vector3 targetPosition = playerBase.transform.position;
            float distance = (targetPosition - source).magnitude;
            if (distance < minDist)
            {
                minDist = distance;
                closest = playerBase.gameObject;
            }
        }
        return closest;
    }


    /// <summary>
    /// adds player base to the internal collection of player bases
    /// </summary>
    public void Register(PlayerBase pbase)
    {
        PlayerBases.Add(pbase);
        TeamPlayer.Add(pbase.transform);
    }

    /// <summary>
    /// removes player base from the internal collection of player bases
    /// </summary>
    public void Unregister(PlayerBase pbase)
    {
        PlayerBases.Remove(pbase);
        TeamPlayer.Remove(pbase.transform);
    }

    /// <summary>
    /// adds enemy base to the collection of enemies and internal collection of enemy bases
    /// </summary>
    public void Register(EnemyBase ebase)
    {
        EnemyBases.Add(ebase);
        TeamEnemy.Add(ebase.transform);
    }

    /// <summary>
    /// removes enemy base from the collection of enemies and internal collection of enemy bases
    /// </summary>
    public void Unregister(EnemyBase ebase)
    {
        EnemyBases.Remove(ebase);
        TeamEnemy.Remove(ebase.transform);
    }

    /// <summary>
    /// adds enemy to the collection of enemies
    /// </summary>
    public void Register(Enemy e)
    {
        TeamEnemy.Add(e.transform);
    }

    /// <summary>
    /// removes enemy from the collection of enemies
    /// </summary>
    public void Unregister(Enemy e)
    {
        TeamEnemy.Remove(e.transform);
    }

    /// <summary>
    /// adds animal to internal collection of Animals
    /// </summary>
    public void Register(Animal a)
    {
        TeamPlayer.Add(a.transform);
        Animals.Add(a); // useful to maintain all animals, not all animals qualify as a target for enemies
    }

    /// <summary>
    /// removes animal to internal collection of Animals
    /// </summary>
    public void Unregister(Animal a)
    {
        // animals never die so this method is probably unnecessary
        TeamPlayer.Remove(a.transform);
        Animals.Remove(a);
    }
}
