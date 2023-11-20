using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    [SerializeField]
    public GameObject Player;

    [SerializeField]
    SpriteRenderer oldHit;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        XRay();
    }

    // Hacer a los objetos que interfieran con la vision transparentes
    private void XRay()
    {
        float characterDistance = Vector3.Distance(transform.position, Player.transform.position);
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, fwd, out hit, characterDistance))
        {
            //print("Raycasthit: " + hit.transform.gameObject.name);
            //Currently, only sprites that turn transparent are bases and scenery
            if (hit.transform.gameObject.name.ToLower().Contains("base") ||
                hit.transform.gameObject.CompareTag("Scenery") ||
                (hit.transform.parent && hit.transform.parent.gameObject.name.ToLower().Contains("base"))) //For bases, collider is often on child
            {

                SpriteRenderer spriteTrans;
                if (hit.transform.gameObject.GetComponent<SpriteRenderer>())
                {
                    spriteTrans = hit.transform.gameObject.GetComponent<SpriteRenderer>();
                } //Account for hitting a child
                else
                {
                    //If we hit a child, find spriterenderer in parent's children
                    spriteTrans = hit.transform.parent.gameObject.GetComponentInChildren<SpriteRenderer>();
                }

                //print("Raycast hit an alpha-able object");
                // Add transparence if newly seeing object
                Color colorB = spriteTrans.color;
                colorB.a = 0.5f;
                spriteTrans.color = colorB;

                // Reduce transparence if we no longer see an object
                if (oldHit && oldHit != spriteTrans)
                {
                    //print("Resetting oldhit");
                    Color colorA = oldHit.color;
                    colorA.a = 1f;
                    oldHit.color = colorA;
                }
                else
                {
                    //print("oldhit is " + oldHit);
                }

                // Save hit
                oldHit = spriteTrans;
            }
            else {
                //print("Hit a non-valid object");
                // Reduce transparence if we no longer see a alpha-able object
                if (oldHit)
                {
                    //print("Seeing oldhit,it is " + oldHit.gameObject.name + ", turning it back to normal");
                    Color colorA = oldHit.color;
                    colorA.a = 1f;
                    oldHit.color = colorA;
                    oldHit = null;
                }
            }
        }
        else {
            print("Raycast missed! Ohhhhh!");
            //We will never be here, raycast will reasonably never miss completely
        }
    }
}  

