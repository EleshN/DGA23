using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Rendering;

public class Level : MonoBehaviour
{

    // TODO: use saved data instead of hardcoded number
    /// <summary>
    /// the highest level number finished (game starts at 0)
    /// </summary>
    public static int maxLevelDefeated = 3;

    /// <summary>
    /// the level this represents
    /// </summary>
    public int levelNumber = 1;

    public float hoverScale = 1.2f;

    /// <summary>
    /// whether this level is locked (inaccessible to the player )
    /// </summary>
    bool locked = false;

    /// <summary>
    /// whether this level has been defeated by the player
    /// </summary>
    bool defeated = false;

    [SerializeField] GameObject lockIcon;

    /// <summary>
    /// the player base icon
    /// </summary>
    [SerializeField] GameObject playerBase;

    /// <summary>
    /// the enemy base icon
    /// </summary>
    [SerializeField] GameObject enemyBase;

    /// <summary>
    /// the folder object containing the three icons (for uniform scaling purposes)
    /// </summary>
    [SerializeField] GameObject selectorContents;

    void Start()
    {
        // initialize properties
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
    }

    private void Awake()
    {
        SortingGroup sortingGroup = GetComponent<SortingGroup>();
        sortingGroup.sortingOrder = -(int) transform.position.y;
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        Vector3 scale = Vector3.one;
        if (hit.collider != null) {
            if (hit.collider.gameObject.tag == Tag.Level.ToString()){
                if (hit.collider.transform.parent.gameObject == selectorContents){
                    // enlarge the sprite if mouse is hovering over
                    scale = new Vector3(hoverScale, hoverScale, 1);
                    // load this level if this level is clicked and not locked
                    if (Input.GetMouseButtonDown(0) && !locked) {
                        SceneManager.LoadScene("level" + levelNumber);
                    }
                }
                
            }
        }
        selectorContents.transform.localScale = scale;
    }
    
}

