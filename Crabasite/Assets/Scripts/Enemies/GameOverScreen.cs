using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    //Setup to activate the GameOverScreen

    public void Setup() {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartButton() {
        DataPersistenceManager dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        dpm.LoadGame(true);
        SceneManager.LoadScene("Level 0 - spaceship");
        Time.timeScale = 1;
    }

    public void QuitButton() {
        DataPersistenceManager.instance.SaveGame(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
