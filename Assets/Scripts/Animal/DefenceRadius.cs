using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceRadius : MonoBehaviour
{
    [SerializeField] float defenceRadius;
    private Enemy enemy;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Enemy")
    //    {
    //        print("STAY");
    //        Enemy enemy = other.gameObject.GetComponent<Enemy>();

    //        if (enemy.targetTransform.gameObject.GetComponent<PlayerBase>() != null ||
    //        enemy.targetTransform.gameObject.GetComponent<Animal>().GetEmotion() != Emotion.DEFENCE)
    //        {
    //            enemy.targetTransform = GetComponentInParent<Animal>().transform;
    //        }
    //    }

    //}

    private void Start()
    {
        
    }

    void Update()
    {
        //RaycastHit hit;

        Collider[] cls = Physics.OverlapSphere(transform.position, defenceRadius);

        foreach(Collider cl in cls)
        {
            Enemy enemy = cl.GetComponent<Enemy>();

            if (enemy != null)
            {
                makeTarget(enemy);
            }
        }
        //print("UPDATE: " + Physics.SphereCast(center, defenceRadius, transform.forward, out hit, 10, layerMask).ToString());

        // Cast a sphere wrapping character controller 10 meters forward
        // to see if it is about to hit anything.
        //if (Physics.SphereCast(center, defenceRadius, transform.forward, out hit, 0, layerMask))
        //{
        //    enemy = hit.transform.GetComponent<Enemy>();
        //    currhitobject = hit.transform.gameObject;
        //    print("Defence hit enemy: " + enemy.ToString());
        //}

        //if (enemy != null)
        //{
        //    makeTarget(enemy);
        //}
    }

    void makeTarget(Enemy enemy)
    {
        //print(enemy.targetTransform.gameObject != Emotion.DEFENCE);
        if (enemy.targetTransform.gameObject.GetComponent<Animal>() == null ||
            enemy.targetTransform.gameObject.GetComponent<Animal>().GetEmotion() != Emotion.DEFENCE)
        {
            enemy.targetTransform = GetComponentInParent<Animal>().transform;
        }
    }

}
