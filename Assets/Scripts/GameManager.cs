using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{

    public static int MaxLevel = 20;

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

    /// <summary>
    /// Required amounts to earn stars
    /// </summary>
    [SerializeField] public int reqShotsFired;
    [SerializeField] public int reqTime;

    //Stars are calculated with the following formula
    // shots% =  1 - (shots - reqshots / reqshots)
    // time% = 1 - (time - reqtime / reqtime)
    // star% = .5 * time% + .5 * shots%

    [HideInInspector] public Player PlayerObject;

    private Collider PlayerCollider;

    // Reference to Player Transform for player target tracking
    [HideInInspector] public Transform PlayerTransform;

    [Header("Game UI")]

    [SerializeField] GameObject GameCanvas;

    [SerializeField] ResultSceneOpener ResultSceneOpener;

    [SerializeField] GameObject levelTextBkg;
    [SerializeField] TMP_Text levelText;

    [SerializeField] public Rotation gunUI;

    /// <summary>
    /// whether the current running level is completed (ongoing vs won/lost)
    /// </summary>
    private bool isLevelComplete;
    
    /// <summary>
    /// the number of enemies killed during this level
    /// </summary>
    private int enemiesKilled;

    /// <summary>
    /// the time spent during this level
    /// </summary>
    private float timeElapsed;

    /// <summary>
    /// the number of bullets used during this level
    /// </summary>
    private int bulletsFired;

    ///
    public AudioSource audioSource;
    public AudioClip captureEnemyBase;
    public AudioClip capturePlayerBase;

    //Level end animation
    public float levelEndCamSpeed;
    public Vector3 lastBasePos; //So we know where to send the camera

    public static GameManager Instance
    {
        get
        {
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

    void OnDestroy(){
        _Instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemiesKilled = 0;
        timeElapsed = 0;
        bulletsFired = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerBases.Count == 0 && !isLevelComplete)
        {
            // you lose
            isLevelComplete = true;
            GameCanvas.SetActive(false);
            ResultSceneOpener.Init(LevelNumber, false);
        }
        if (EnemyBases.Count == 0 && !isLevelComplete)
        {
            isLevelComplete = true;
            // you win
            winLevel();
        }

        if (!isLevelComplete) {
            timeElapsed += Time.deltaTime;
        }
    }

    private void winLevel() {
        StartCoroutine(LevelWinCoro());
    }

    private IEnumerator LevelWinCoro() {
        //Set the camera to go to the base that ended the level
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        cam.GetComponent<EndLevelCamera>().translatingTowards = lastBasePos;
        cam.GetComponent<CameraFollow>().enabled = false;
        cam.GetComponent<EndLevelCamera>().enabled = true;

        //Wait for the camera to get there
        while (cam.GetComponent<EndLevelCamera>().stillGoing) {
            yield return null;
        }

        //Slow down for cinematic purposes
        Time.timeScale = 0.075f;
        yield return new WaitForSeconds(2f / 20); // divided by 20 because of timestep
        //Actually end the level
        GameCanvas.SetActive(false);
        ResultSceneOpener.Init(LevelNumber, true);
        Time.timeScale = 1f;
        //Stop screen shaking
        cam.GetComponent<EndLevelCamera>().enabled = false;
    }

    public Transform FindClosest(Vector3 source, HashSet<Transform> transforms)
    {
        // find closest using Euclidean distance
        Transform closest = null;
        float minDist = float.PositiveInfinity;
        foreach (Transform target in transforms)
        {
            if (target != null)
            {
                Vector3 targetPosition = target.position;
                float distance = (targetPosition - source).magnitude;
                if (distance < minDist)
                {
                    minDist = distance;
                    closest = target;
                }
            }
            else {
                transforms.Remove(target);
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
        Physics.IgnoreCollision(pbase.GetComponent<Collider>(), PlayerCollider);
    }

    /// <summary>
    /// removes player base from the internal collection of player bases
    /// </summary>
    public void Unregister(PlayerBase pbase)
    {
        PlayerBases.Remove(pbase);
        ValidEnemyTargets.Remove(pbase.transform);
        audioSource.PlayOneShot(capturePlayerBase);
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
        audioSource.PlayOneShot(captureEnemyBase);
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
        enemiesKilled += 1;
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
        // print("hello");
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

    /// <summary>
    /// the current set of all enemies in the level
    /// </summary>
    public HashSet<Enemy> GetEnemies(){
        return Enemies;
    }

    #region Player Level Stats

    /// <summary>
    /// the time taken for this level
    /// </summary>
    public float getTimeElapsed(){
        return timeElapsed;
    }

    /// <summary>
    /// number of enemies killed during the level
    /// </summary>
    public int getEnemiesKilled() {
        return enemiesKilled;
    }

    /// <summary>
    /// number of bullets used during the level
    /// </summary>
    public int getBulletsFired() {
        return bulletsFired;
    }

    /// <summary>
    /// increase the number of bullets fired by 1
    /// </summary>
    public void incrementBulletsFired() {
        bulletsFired += 1;
    }

    public double getNumStars() {
        print("Timeelapsed is " + timeElapsed);
        float timePercent = 1 - Mathf.Clamp((timeElapsed - reqTime) / reqTime, 0, 1);
        float shotsPercent =
            1 - Mathf.Clamp((bulletsFired - reqShotsFired) / reqShotsFired, 0, 1);
        double totalStarPercent = .5 * timePercent + .5 * shotsPercent;
        return totalStarPercent;
    }

    #endregion
}
