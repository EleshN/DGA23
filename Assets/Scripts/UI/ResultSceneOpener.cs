using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultSceneOpener : MonoBehaviour
{
    [SerializeField]
    GameObject WinScreen;
    [SerializeField]
    GameObject LoseScreen;

    int currentLevel;

    bool levelResult;

    [Header("Level Win + Stars")]
    [SerializeField]
    TextMeshProUGUI timeElapsed;
    [SerializeField]
    TextMeshProUGUI shotsFired;
    [SerializeField]
    TextMeshProUGUI enemiesKilled;

    [SerializeField]
    List<GameObject> stars;

    private void Start()
    {
        GetComponent<Canvas>().worldCamera =
            GameObject.FindGameObjectWithTag("MainCamera")
            .GetComponent<CameraFollow>().otherCam;
    }

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
            WinScreen.SetActive(true);
            StartCoroutine(WinLevelCoro());
            //if (currentLevel < GameManager.MaxLevel)
            //{
            //    restartOrNextText.text = "Next Level";
            //} TODO: Last level stuff
        }
        else
        {
            LoseScreen.SetActive(true);
        }
        this.currentLevel = currentLevel;
        this.levelResult = result;
    }

    IEnumerator WinLevelCoro() {
        timeElapsed.text = "";
        shotsFired.text = "";
        enemiesKilled.text = "";

        float waittime = 1f;
        
        //Time
        yield return new WaitForSeconds(waittime);
        int seconds = (int)GameManager.Instance.getTimeElapsed();
        int minutes = seconds / 60;
        seconds = seconds % 60;
        timeElapsed.text = Mathf.Clamp(minutes, 0, 99).ToString() + ":" + seconds.ToString();
        Debug.Log("time done");

        //Shots
        yield return new WaitForSeconds(waittime);
        shotsFired.text = Mathf.Clamp(GameManager.Instance.getBulletsFired(), 0, 999).ToString();
        Debug.Log("shots done");

        //Enemies
        yield return new WaitForSeconds(waittime);
        enemiesKilled.text = Mathf.Clamp(GameManager.Instance.getEnemiesKilled(), 0, 9999).ToString();
        Debug.Log("enemies done");

        //Stars
        double numstars = GameManager.Instance.getNumStars();
        Debug.Log("getnumstars gave " + numstars);
        if (numstars > .25)
        {
            yield return new WaitForSeconds(waittime);
            stars[0].SetActive(true);
            Debug.Log("Earned 1 star");
        }
        if (numstars > .5) {
            yield return new WaitForSeconds(waittime);
            stars[1].SetActive(true);
            Debug.Log("Earned 2 stars");
        }
        if (numstars > .75) {
            yield return new WaitForSeconds(waittime);
            stars[2].SetActive(true);
            Debug.Log("Earned 3 stars");
        }

        //Done!
    }

    /// <summary>
    /// transition function to load the scene corresponding to the next level.
    /// In the event that the level ends in player's defeat, the current level is treated as the "next".
    /// </summary>
    public void ToNextLevel()
    {
        Time.timeScale = 1f;
        PauseGame.isPaused = false;
        if (levelResult)
        {
            int level = currentLevel + 1;
            //TODO: what if there is no next level? (result menu needs an edge case adjustment, keep this in mind for end product)
            if (level > GameManager.MaxLevel){
                // return to main menu etc
                SceneManager.LoadScene("LevelSelect");
            }
            else
            {
                SceneManager.LoadScene("Level"+level.ToString());
            }
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
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelSelect");
    }

    public void GoRestart()
    {
        //Time.timeScale = 1f;
        GameObject.FindAnyObjectByType<PauseGame>().GetComponent<PauseGame>().GoRestart();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
