using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallWin : MonoBehaviour
{
    public WinScript WinScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            
            WinScript.Init(true);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            WinScript.Init(false);
        }
    }
}
