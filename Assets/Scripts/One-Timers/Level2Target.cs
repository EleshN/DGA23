using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Target : MonoBehaviour
{
    [SerializeField]
    Animal dog;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
         
    }

    // Update is called once per frame
    void Update()
    {
        if (dog.GetEmotion() == Emotion.LOVE)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
