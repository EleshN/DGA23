using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{

    [SerializeField] float MaxScale = 2.0f;

    [SerializeField] float MinScale = 0.5f;

    [SerializeField] float linearInterpFactor = 0.04f;

    [SerializeField] RectTransform rectTransform;

    bool upScale; 

    private void Start()
    {
        rectTransform.localScale = new Vector3(MinScale, MinScale, MinScale);
        upScale = false;
    }

    private void Update()
    {
        float scale = rectTransform.localScale.x;
        if (upScale){
            scale = Math.Min(scale + linearInterpFactor, MaxScale);
        }
        else {
            scale = Math.Max(scale - linearInterpFactor, MinScale);
        }
        rectTransform.localScale = new Vector3(scale, scale, scale);

        // put the minimap's bottom right corner at the bottom right corner of the screen (anchor)
        rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x, scale * 200, rectTransform.anchoredPosition3D.z);
    }

    public void OnPress(){
        upScale = !upScale;
    }

}
