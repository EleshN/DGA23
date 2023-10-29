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

    public void Init(bool result)
    {
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

}
