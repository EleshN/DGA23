using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Ant : Enemy
{
    [Tooltip("Number of ants stacked on this ant")]
    public int stackVal = 1;
    [SerializeField] int maxStack;
    [Tooltip("The check radius around ant to start stacking with another ant")]
    [SerializeField] float stackSearchRadius;
    [Tooltip("Time it takes for ants to stack")]
    [SerializeField] float stackTime;
    float stackTimeLeft;
    [Tooltip("How often an ant checks if it can stack")]
    [SerializeField] float searchRepeatTime;
    float searchTime;
    bool stacking;

    [SerializeField] GameObject stackPrefab;
    [SerializeField] Transform antStack;
    [SerializeField] float stackSpacing;


    [Header("Combat")]
    float baseDamage;
    float baseMaxHealth;

    [SerializeField] Hitbox hitbox;
    [SerializeField] float attackDelay;
    [SerializeField] float hitboxActiveTime;
    [Tooltip("The bonus health and damage applied to Ant stack")]
    [SerializeField] float[] stackMultiplier;



    protected override void Start()
    {
        base.Start();
        baseDamage = robotDamage;
        baseMaxHealth = maxHealth;
        stackTimeLeft = stackTime;
        hitbox.Initialize();
    }

    protected override void Update()
    {
        searchTime -= Time.deltaTime;
        if(searchTime <= 0)
        {
            CheckStack();
            searchTime = searchRepeatTime;
        }
        if (!stacking)
        {
            base.Update();
        }
        else if (targetTransform != null)
        {
            Move(targetTransform.position);
        }
    }

    protected override void Attack()
    {
        // perhaps for melee enemies, this is where we animate the attack motion.
        hitbox.SetUniformDamage(base.targets, robotDamage);
        StartCoroutine(ToggleHitbox());
    }

    /// <summary>
    /// turns on the hitbox to deal damage to opponents and then turns off the hitbox once damage time is over (as indicated by hitboxActiveTime)
    /// </summary>
    IEnumerator ToggleHitbox()
    {
        yield return new WaitForSeconds(attackDelay);
        hitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(hitboxActiveTime);
        hitbox.gameObject.SetActive(false);

    }

    public void CheckStack()
    {
        stacking = false;
        Collider[] cls = Physics.OverlapSphere(transform.position, stackSearchRadius);

        foreach(Collider cl in cls)
        {
            Ant ant = cl.GetComponent<Ant>();
            if (ant != null && ant != this && ant.stackVal + stackVal < maxStack)
            {
                targetTransform = ant.transform;
                stacking = true;
                break;
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        Ant ant = other.GetComponent<Ant>();
        if(ant != null)
        {
            print("stacking");
            stackTimeLeft -= Time.deltaTime;
            if(stackTimeLeft <= 0 && ant.stackVal + stackVal <= maxStack)
            {
                Stack(ant);
            }
        }
    }

    void Stack(Ant ant)
    {
        if(ant.gameObject != null)
        {
            stackVal += ant.stackVal;
            UpdateStats(ant.stackVal);
            print("Stack Value: " + stackVal);
            Destroy(ant.gameObject);

        }
        foreach (Transform child in antStack)
        {
            Destroy(child.gameObject);
        }
        for(int i = 1; i < stackVal; i++)
        {
            Transform stackAnt = Instantiate(stackPrefab, antStack).transform;
            stackAnt.position += new Vector3(0,i * stackSpacing,0);
        }


        stacking = false;
        stackTimeLeft = stackTime;
    }
    /// <summary>
    /// Called whenever an Ant stacks to update the health, max health, and damage
    /// to increase based on ant stack size
    /// </summary>
    void UpdateStats(int stackAdded)
    {
        robotDamage = baseDamage * stackMultiplier[stackVal-1];
        maxHealth = baseMaxHealth * stackMultiplier[stackVal -1];
        health = health + stackVal * baseMaxHealth;
        print(robotDamage + ", " + maxHealth);
    }





}
