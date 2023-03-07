using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPause : MonoBehaviour
{
    public GameObject PausePanel;

    void Start(){
        GameObject[] obj = GameObject.FindObjectsOfType<GameObject>(true);
        foreach(GameObject go in obj){
            if(go.tag == "PausePanel"){
                PausePanel = go;
                break;
            }
        }
    }
    
    // Update is called once per framepublic GameObject pausePanel;
    // Checks if pause Menu has been opened, if so freeze time => Maybe later not freezing?
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                PausePanel.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                PausePanel.SetActive(false);
            }
        }
    }

    public void UnPauseMenu()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitPauseMenu()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

}
