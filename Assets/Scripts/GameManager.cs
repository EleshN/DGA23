using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _Instance;
    public int LevelNumber = 1;
    public List<Animal> Animals { get; private set; }
    public List<Animal> Enemies { get; private set; }
    public List<PlayerBase> PlayerBases { get; private set; }
    public List<EnemyBase> EnemyBases { get; private set; }

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
        Animals = new();
        Enemies = new();
        PlayerBases = new();
        EnemyBases = new();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (len(PlayerBases) == 0) Debug.Log("Game Over");
        if (len(EnemyBases) == 0) Debug.Log("You Win");
    }
}
