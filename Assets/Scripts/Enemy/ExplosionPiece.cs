using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPiece : MonoBehaviour
{
    [SerializeField]
    float fadeRate = 1/255;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color current = GetComponent<SpriteRenderer>().color;
        print("Alpha is " + current.a);
        if (current.a == 0) {
            print("Destroying " + gameObject.name);
            Destroy(gameObject);
        }
        Color newc = new Color(current.r, current.g, current.b, current.a - fadeRate);
        GetComponent<SpriteRenderer>().color = newc;
    }
}
