using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPause : MonoBehaviour
{
    public GameObject pausePanel;
    // Update is called once per framepublic GameObject pausePanel;
    // Checks if pause Menu has been opened, if so freeze time => Maybe later not freezing?
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                pausePanel.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                pausePanel.SetActive(false);
            }
        }
    }

    public void UnPauseMenu()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitPauseMenu()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

}
