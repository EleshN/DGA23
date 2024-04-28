using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Level : MonoBehaviour
{

    // TODO: use saved data instead of hardcoded number
    /// <summary>
    /// the highest level number finished (game starts at 0)
    /// </summary>
    public static int maxLevelDefeated = 3;

    public int levelNumber = 1;

    public float hoverScale = 1.2f;

    private Vector3[] defaultScales;

    bool locked = false;

    bool defeated = false;

    [SerializeField] GameObject lockIcon;

    [SerializeField] GameObject playerBase;

    [SerializeField] GameObject enemyBase;

    void Start()
    {
        defeated = levelNumber <= maxLevelDefeated;
        locked = levelNumber > maxLevelDefeated + 1;
        if (defeated){
            enemyBase.SetActive(false);
            playerBase.SetActive(true);
        }
        else {
            playerBase.SetActive(false);
            enemyBase.SetActive(true);
        }
        lockIcon.SetActive(!defeated && locked);
        defaultScales = new Vector3[3];
        defaultScales[0] = lockIcon.transform.localScale;
        defaultScales[1] = playerBase.transform.localScale;
        defaultScales[2] = enemyBase.transform.localScale;
    }

    private void Awake()
    {

    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        Vector3 scale = Vector3.one;
        if (hit.collider != null) {
            if (hit.collider.gameObject.tag == Tag.Level.ToString()){
                if (hit.collider.transform.parent.gameObject == gameObject){
                    // enlarge the sprite if hovered by increasing scalar factor
                    scale = new Vector3(hoverScale, hoverScale, 1);
                    // load this level if this level is clicked and not locked
                    if (Input.GetMouseButtonDown(0) && !locked) {
                        SceneManager.LoadScene("level" + levelNumber);
                    }
                }
                
            }
        }
        lockIcon.transform.localScale = Vector3.Scale(scale, defaultScales[0]);
        playerBase.transform.localScale = Vector3.Scale(scale, defaultScales[1]);
        enemyBase.transform.localScale = Vector3.Scale(scale, defaultScales[2]);
    }
    
}

