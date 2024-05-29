using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour, IDamageable
{

    [SerializeField]
    GameObject clusterPrefab;

    public void TakeDamage(float damageAmount, Transform damageSource)
    {
        if (GetComponentInChildren<SpriteRenderer>().color == Color.white) {
            GetComponentInChildren<SpriteRenderer>().color = Color.grey;
        }
        else
        {
            Instantiate(clusterPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
