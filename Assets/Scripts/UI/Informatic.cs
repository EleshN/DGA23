using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Informatic : MonoBehaviour
{
    //A message that shows at the beginning of a level.
    //This is used to introduce an enemy or animal type, and in
    // the tutorial levels at the beginning of the game.

    //The text that describes how to interact with the popup
    [SerializeField]
    GameObject text;

    //The main image containing information
    [SerializeField]
    Image info;

    //A translucent layer that appears behind the main image
    [SerializeField]
    Image screen;

    //Whether the popup is visible
    bool active = false;

    //The input to toggle on / off. Currently return
    public KeyCode toggle = KeyCode.Return;

    // Start is called before the first frame update
    void Start()
    {
        activate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(toggle)) {
            if (active)
            {
                deactivate();
            }
            else {
                activate();
            }
        }
    }

    void activate() {
        //Pause game
        Time.timeScale = 0;
        //Make everything visible
        text.SetActive(true);
        info.enabled = true;
        screen.enabled = true;
        //Track status
        active = true;
    }

    void deactivate() {
        //Unpause game
        Time.timeScale = 1;
        //Make everything visible
        text.SetActive(false);
        info.enabled = false;
        screen.enabled = false;
        //Track status
        active = false;
    }
}
