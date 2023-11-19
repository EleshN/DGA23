using Unity.VisualScripting;
using UnityEngine;

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



    protected override void Start()
    {
        base.Start();
        stackTimeLeft = stackTime;
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
            print(stackVal);
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





}
