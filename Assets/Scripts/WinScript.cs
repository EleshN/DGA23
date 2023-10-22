using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    public Text resultText;
    public Text restartOrNextText;
    //public Text toDisplay;

    //void Start()
    //{

    //}

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        gameObject.SetActive(true);
    //        Init(true);
    //    } else if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        gameObject.SetActive(true);
    //        Init(false);
    //    }
    //}

    // Param bool result - If player won or lost
    public void Init(bool result)
    {
        gameObject.SetActive(true);
        if (result)
        {
            resultText.text = "You Win!";
            restartOrNextText.text = "Next Level";
        }
        else
        {
            resultText.text = "You Lose!";
            restartOrNextText.text = "Restart";
        }
    }

    public void ToNextLevel()
    {
        if (restartOrNextText.text.Equals("Next Level"))
        {
            SceneManager.LoadScene("GamePlayLevel2");
        }
        else
        {
            SceneManager.LoadScene("GamePlayLevel1");
        }
    }

    public void ToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }


    //public void GoToScene(bool r)
    //{
    //    if (Input.GetKeyDown("return") && r)
    //    {
    //        //nextScene = "LevelSelect";
    //        //OpenScene();
    //        SceneManager.LoadScene("GamePlayLevel1");
    //    } else if (Input.GetKeyDown("space"))
    //    {
    //        //nextScene = "GamePlayLevel1";
    //        //OpenScene();
    //        SceneManager.LoadScene("LevelSelect");
    //    }
    //}

}
