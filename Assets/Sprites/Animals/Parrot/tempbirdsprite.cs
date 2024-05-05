using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class tempbirdsprite : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator anim;
    [SerializeField] NavMeshAgent nm;

    [SerializeField] int RiseSpeed;

    [SerializeField] Vector3 flySpeed;

    [SerializeField] int FallSpeed;

    [SerializeField] int flycycles;
    private int completedFlyCycles = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb.useGravity = false;
        nm.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void rise() {
        print("Rising");
        nm.enabled = false;
        rb.velocity = new Vector3(0, RiseSpeed, 0);
    }

    public void finishRising() {
        print("Done rising");
        rb.velocity = flySpeed;
    }

    public void fall() {
        print("Falling");
        rb.velocity = new Vector3(0, -1 * FallSpeed, 0);
    }

    public void finishFlyCycle() {
        print("Finished fly cycle");
        completedFlyCycles++;
        if (completedFlyCycles > flycycles) {
            anim.SetTrigger("Fall");
            completedFlyCycles = 0;
        }
    }
}
