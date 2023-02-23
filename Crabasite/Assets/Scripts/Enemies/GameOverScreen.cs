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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

}
