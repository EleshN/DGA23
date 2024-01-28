using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{

    public static int MaxLevel = 10;

    private static GameManager _Instance;
    public int LevelNumber = -1;

    /// <summary>
    /// Are we in debug? If not, can't see meshes. Perhaps more functionality to come
    /// </summary>
    [SerializeField]
    public bool isDebug;

    /// <summary>
    /// Limit for the number of enemies
    /// </summary>
    public int EnemySpawnCap;

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
    /// All enemy gameobjects
    /// </summary>
    private HashSet<Enemy> Enemies;

    /// <summary>
    /// Set of animals currently in loved with the player
    /// </summary>
    public HashSet<Animal> followers;

    /// <summary>
    /// All enemies currently on the map (robots, enemy bases)
    /// </summary>
    public HashSet<Transform> TeamEnemy;

    /// <summary>
    /// All player-side entities currently on the map that can be targeted by enemies
    /// </summary>
    public HashSet<Transform> ValidEnemyTargets;

    [HideInInspector] public Player PlayerObject;

    private Collider PlayerCollider;

    // Reference to Player Transform for player target tracking
    [HideInInspector] public Transform PlayerTransform;

    [Header("Game UI")]

    [SerializeField] GameObject GameCanvas;

    [SerializeField] ResultSceneOpener ResultSceneOpener;
    [SerializeField] TMP_Text enemyBaseCount;
    [SerializeField] TMP_Text playerBaseCount;

    [SerializeField] TMP_Text loveCount;
    [SerializeField] TMP_Text angerCount;
    [SerializeField] GameObject loveUI;
    [SerializeField] GameObject angerUI;

    [SerializeField] GameObject levelTextBkg;
    [SerializeField] TMP_Text levelText;

    /// <summary>
    /// whether the current running level is completed (ongoing vs won/lost)
    /// </summary>
    private bool isLevelComplete;

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
        ValidEnemyTargets = new();
        Animals = new();
        Enemies = new();
        followers = new();
        isLevelComplete = false;

        // TODO: usage of "Player" tag is not unique. Transform is fine but Player component not necessarily accessible.
        // PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject[] candidates = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in candidates)
        {
            Player playerComponent = obj.GetComponent<Player>();
            if (playerComponent != null)
            {
                PlayerObject = playerComponent;
                PlayerTransform = PlayerObject.transform;
                PlayerCollider = PlayerTransform.GetComponent<Collider>();
            }
        }
        Assert.IsTrue(PlayerObject != null, "Unable to find player script");
        Assert.IsTrue(PlayerTransform != null, "Unable to find player");
        Assert.IsTrue(PlayerCollider != null, "Unable to find player's collider");
        Assert.IsTrue(LevelNumber >= 0, "Level Number in GameManager must be set");

        levelText.enabled = LevelNumber >= 1;
        levelTextBkg.SetActive(LevelNumber >= 1);
        if (LevelNumber >= 1){
            levelText.text = "Level: " + LevelNumber;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerBases.Count == 0 && !isLevelComplete)
        {
            // you lose
            GameCanvas.SetActive(false);
            ResultSceneOpener.Init(LevelNumber,false);
            isLevelComplete = true;
        }
        if (EnemyBases.Count == 0 && !isLevelComplete)
        {
            // you win
            GameCanvas.SetActive(false);
            ResultSceneOpener.Init(LevelNumber, true);
            isLevelComplete = true;
        }

        // update GameCanvas text elements
        UpdateIconCounts(PlayerObject.GetCurrentAmmoType());
    }

    private void UpdateIconCounts(string currentSelected = "")
    {
        // update all counts
        for (int i = 0; i < PlayerObject.ammoNames.Length; i++)
        {
            int count = PlayerObject.ammo[i];
            switch (PlayerObject.ammoNames[i]) {
                case "anger":
                    angerCount.text = count.ToString();
                    break;
                case "love":
                    loveCount.text = count.ToString();
                    break;
                default:
                    Debug.Log("Invalid emotion");
                    break;
            }
        }

        // switch between icon focus
        switch (currentSelected)
        {
            case "anger":
                angerUI.transform.localScale = new Vector3(1, 1, 1);
                loveUI.transform.localScale = new Vector3(.5f, .5f, .5f);
                break;
            case "love":
                loveUI.transform.localScale = new Vector3(1, 1, 1);
                angerUI.transform.localScale = new Vector3(.5f, .5f, .5f);
                break;
            default:
                break;
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

    public GameObject FindClosestTargetForEnemy(Enemy e)
    {
        GameObject closest = null;
        Vector3 source = e.gameObject.transform.position;
        float minDist = float.PositiveInfinity;
        // check each animal, whether they can be attacked: if yes, consider the animal as a candidate.
        foreach (Animal animal in Animals)
        {
            if(animal.GetEmotion() == Emotion.ANGER)
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
        ValidEnemyTargets.Add(pbase.transform);
        playerBaseCount.text = "Total Player Bases: " + PlayerBases.Count.ToString();
    }

    /// <summary>
    /// removes player base from the internal collection of player bases
    /// </summary>
    public void Unregister(PlayerBase pbase)
    {
        PlayerBases.Remove(pbase);
        ValidEnemyTargets.Remove(pbase.transform);
        playerBaseCount.text = "Total Player Bases: " + PlayerBases.Count.ToString();
    }

    /// <summary>
    /// adds enemy base to the collection of enemies and internal collection of enemy bases
    /// </summary>
    public void Register(EnemyBase ebase)
    {
        EnemyBases.Add(ebase);
        TeamEnemy.Add(ebase.transform);
        enemyBaseCount.text = "Total Enemy Bases: " + EnemyBases.Count.ToString();
    }

    /// <summary>
    /// removes enemy base from the collection of enemies and internal collection of enemy bases
    /// </summary>
    public void Unregister(EnemyBase ebase)
    {
        EnemyBases.Remove(ebase);
        TeamEnemy.Remove(ebase.transform);
        enemyBaseCount.text = "Total Enemy Bases: " + EnemyBases.Count.ToString();
    }

    /// <summary>
    /// adds enemy to the collection of enemies
    /// </summary>
    public void Register(Enemy e)
    {
        TeamEnemy.Add(e.transform);
        Enemies.Add(e);
    }

    /// <summary>
    /// removes enemy from the collection of enemies
    /// </summary>
    public void Unregister(Enemy e)
    {
        TeamEnemy.Remove(e.transform);
        Enemies.Remove(e);
    }

    /// <summary>
    /// adds animal to internal collection of Animals
    /// 
    /// <para>A registered animal does not interfere with player's movement</para>
    /// </summary>
    public void Register(Animal a)
    {
        Animals.Add(a); // useful to maintain all animals, not all animals qualify as a target for enemies
        if (a.TryGetComponent<Collider>(out Collider animalCollider))
        {
            Physics.IgnoreCollision(PlayerCollider, animalCollider);
        }
    }

    /// <summary>
    /// removes animal to internal collection of Animals
    /// </summary>
    public void Unregister(Animal a)
    {
        Animals.Remove(a);
    }

    /// <summary>
    /// adds animal to internal collection of Animals
    /// 
    /// <para>A registered animal does not interfere with player's movement</para>
    /// </summary>
    public void Register(Tree t)
    {
        //Trees.Add(t); // useful to maintain all animals, not all animals qualify as a target for enemies
        print("hello");
        if (t.TryGetComponent<Collider>(out Collider treeCollider))
        {
            Physics.IgnoreCollision(PlayerCollider, treeCollider);
        }
    }

    /// <summary>
    /// removes animal to internal collection of Animals
    /// </summary>
    public void Unregister(Tree t)
    {
        //Trees.Remove(t);
    }

    /// <summary>
    /// returns whether more enemies can be spawned without hitting the enemy spawn cap
    /// </summary>
    public bool WithinEnemySpawnCap()
    {
        return (TeamEnemy.Count - EnemyBases.Count) < EnemySpawnCap;
    }

    // /// <summary>
    // /// Pause all animals, robots, bases on screen.
    // /// </summary>
    // private void PauseObjects()
    // {
    //     // turn off all animal, enemy, player scripts
    //     foreach (Enemy e in Enemies)
    //     {
    //         e.gameObject.GetComponent<Enemy>().enabled = false;
    //     }
    //     foreach (Animal a in Animals)
    //     {
    //         a.gameObject.GetComponent<Animal>().enabled = false;
    //     }
    //     PlayerTransform.gameObject.GetComponentInParent<Player>().enabled = false;
    // }
}
