using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAlpha : MonoBehaviour
{
    [SerializeField]
    public GameObject Player;

    // Use this for initialization
    [SerializeField]
    SpriteRenderer oldHit;
    List<string> xrayTargets;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        // Add all enemies to list
        xrayTargets = new List<string>
        {
            Tag.EnemyBase.ToString(),
            Tag.PlayerBase.ToString(),
            Tag.Scenery.ToString()
        };
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        XRay();
        //XRayEnemies();
    }

    // Make objects that interfere with the camera transparent
    private void XRay()
    {
        float characterDistance = Vector3.Distance(transform.position, Player.transform.position);
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, out RaycastHit hit, characterDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (xrayTargets.Contains(hit.transform.gameObject.tag))
            {
                SpriteRenderer spriteTrans;
                if (hit.transform.gameObject.GetComponent<SpriteRenderer>())
                {
                    spriteTrans = hit.transform.gameObject.GetComponent<SpriteRenderer>();
                } 
                else
                {
                    spriteTrans = hit.transform.gameObject.GetComponentInChildren<SpriteRenderer>();
                }

                Color colorB = spriteTrans.color;
                colorB.a = 0.5f;
                spriteTrans.color = colorB;

                if (oldHit && oldHit != spriteTrans)
                {
                    Color colorA = oldHit.color;
                    colorA.a = 1f;
                    oldHit.color = colorA;
                }
                oldHit = spriteTrans;
            }
            else {
                if (oldHit)
                {
                    Color colorA = oldHit.color;
                    colorA.a = 1f;
                    oldHit.color = colorA;
                    oldHit = null;
                }
            }
        }
    }

    private void XRayEnemies() {
        foreach (Enemy enemy in GameManager.Instance.GetEnemies())
        {
            float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);
            Vector3 enemyDirection = (enemy.transform.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, enemyDirection, out RaycastHit enemyHit, enemyDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                if (xrayTargets.Contains(enemyHit.transform.gameObject.tag))
                {
                    SpriteRenderer spriteTrans;
                    if (enemyHit.transform.gameObject.GetComponent<SpriteRenderer>())
                    {
                        spriteTrans = enemyHit.transform.gameObject.GetComponent<SpriteRenderer>();
                    } 
                    else
                    {
                        spriteTrans = enemyHit.transform.gameObject.GetComponentInChildren<SpriteRenderer>();
                    }

                    Color colorB = spriteTrans.color;
                    colorB.a = 0.5f;
                    spriteTrans.color = colorB;
                }
            }
        }
    }
}  

