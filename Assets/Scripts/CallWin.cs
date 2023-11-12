using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CallWin : MonoBehaviour
{
    public ResultSceneOpener ResultSceneOpener;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ResultSceneOpener.Init(true);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            ResultSceneOpener.Init(false);
        }
    }
}
