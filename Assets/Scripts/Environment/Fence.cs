using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour, IDamageable
{
    public void TakeDamage(float damageAmount, Transform damageSource)
    {
        if (GetComponent<SpriteRenderer>().color == Color.white) {
            GetComponent<SpriteRenderer>().color = Color.grey;
        }
        else
        {
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
