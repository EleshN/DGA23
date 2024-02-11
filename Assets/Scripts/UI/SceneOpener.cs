using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneOpener : MonoBehaviour
{
    // Loading scene name
    [SerializeField] protected string nextScene = "";

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OpenScene()
    {
        SceneManager.LoadScene(nextScene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
