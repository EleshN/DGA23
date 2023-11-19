using UnityEngine;

public class Ant : Enemy
{
    [Tooltip("Number of ants stacked on this ant")]
    public int stackVal = 1;
    [Tooltip("The check radius around ant to start stacking with another ant")]
    [SerializeField] float stackSearchRadius;
    [Tooltip("Time it takes for ants to stack")]
    [SerializeField] float stackTime;
    float stackTimeLeft;
    [Tooltip("How often an ant checks if it can stack")]
    [SerializeField] float searchRepeatTime;
    float searchTime;

    bool stacking;

    protected override void Start()
    {
        base.Start();
        stackTimeLeft = stackTime;
    }

    protected override void Update()
    {
        if (!stacking)
        {
            base.Update();
        }
    }


    protected override void Attack()
    {

    }

    public void CheckStack()
    {
        stacking = false;
        Collider[] cls = Physics.OverlapSphere(transform.position, stackSearchRadius);

        foreach(Collider cl in cls)
        {
            Ant ant = cl.GetComponent<Ant>();
            if (ant != null)
            {
                targetTransform = ant.transform;
                stacking = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        stackTimeLeft -= Time.deltaTime;
        if(stackTimeLeft <= 0)
        {
            Ant ant = other.GetComponent<Ant>();
            if(ant != null)
                Stack(ant);
        }
    }

    void Stack(Ant ant)
    {
        stackVal += ant.stackVal;
        print(stackVal);
        Destroy(ant);
        stacking = false;
        state = EnemyState.WANDER;
    }





}
