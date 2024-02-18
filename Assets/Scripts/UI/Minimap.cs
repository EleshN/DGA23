using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{

    [SerializeField] float MaxScale = 2.0f;

    [SerializeField] float MinScale = 0.5f;

    [SerializeField] float linearInterpFactor = 0.04f;

    bool upScale; 

    private void Start()
    {
        transform.localScale = new Vector3(MinScale, MinScale, MinScale);
        upScale = false;
    }

    private void Update()
    {
        float scale = transform.localScale.x;
        if (upScale){
            scale = Math.Min(scale + linearInterpFactor, MaxScale);
        }
        else {
            scale = Math.Max(scale - linearInterpFactor, MinScale);
        }
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public void OnPress(){
        upScale = !upScale;
    }

}
