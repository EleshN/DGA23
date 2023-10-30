using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultSceneOpener : MonoBehaviour
{
    public TMP_Text resultText;
    public TMP_Text restartOrNextText;
    public TMP_Text ammoTypeText;
    public TMP_Text ammoCountText;
    public GameObject gameCanvas;
    public GameManager gameManager;

    public void Init(bool result)
    {
        gameCanvas.SetActive(false);
        gameObject.SetActive(true);
        ammoTypeText.enabled = false;
        ammoCountText.enabled = false;
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
            int level = gameManager.LevelNumber + 1;
            SceneManager.LoadScene("GamePlayLevel"+level.ToString());
        }
        else
        {
            //int level = gameManager.LevelNumber;
            //SceneManager.LoadScene("GamePlayLevel"+level.ToString());
            // Temporary change for prototype demo
            SceneManager.LoadScene("GameTestScene");
        }
    }

    public void ToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

}
