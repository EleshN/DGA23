using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultSceneOpener : MonoBehaviour
{
    [Tooltip("the textbox for storing the result as either win/lose")]
    public TMP_Text resultText;

    [Tooltip("the textbox indicating the action corresponding to restarting or proceeding to next level")]
    public TMP_Text restartOrNextText;

    int currentLevel;

    bool levelResult;

    /// <summary>
    /// constructor to initialize the results menu for the given level and game result.
    /// </summary>
    /// <param name="currentLevel">current level (the level that the player just finished)</param>
    /// <param name="result">true only if player successfully finishes the level</param>
    public void Init(int currentLevel, bool result)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        PauseGame.isPaused = true;
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
        this.currentLevel = currentLevel;
        this.levelResult = result;
    }

    /// <summary>
    /// transition function to load the scene corresponding to the next level.
    /// In the event that the level ends in player's defeat, the current level is treated as the "next".
    /// </summary>
    public void ToNextLevel()
    {
        Time.timeScale = 1f;
        PauseGame.isPaused = false;
        //TODO: what if there is no next level? (result menu needs an edge case adjustment, keep this in mind for end product)
        if (levelResult)
        {
            int level = currentLevel + 1;
            SceneManager.LoadScene("Level"+level.ToString());
        }
        else
        {
            // replay current level
            SceneManager.LoadScene("Level"+currentLevel.ToString());
        }
    }

    public void ToLevelSelect()
    {
        PauseGame.isPaused = false;
        SceneManager.LoadScene("LevelSelect");
    }

}
