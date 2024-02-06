using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject endTestPanel;

    public static GameController instance;
    void Start()
    {
        instance = this;
        Time.timeScale = 1;
    }

    public void ShowEndTest()
    {
        endTestPanel.SetActive(true);
        Time.timeScale = 0;
    
    }

    public void RestartTest()
    {
        SceneManager.LoadScene(1);
       
    }
    
}
