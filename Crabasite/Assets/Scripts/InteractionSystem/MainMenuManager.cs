using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour, IDataPersistence
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button optionsGameButton;
    [SerializeField] private Button quitGameButton;

    private string level = "Level 1 - space";

    // New Game starts with fresh GameData
    private void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            loadGameButton.interactable = false;
        }
    }
    public void NewGameMenu()
    {
        DisableMenuButtons();
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("Level 1 - space");
    }

    // Game will be loaded from saved GameData
    public void LoadGameMenu()
    {
        DisableMenuButtons();
        DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadSceneAsync(this.level);
    }

    public void OptionsMenu()
    {
        DisableMenuButtons();
        // Add Options Screen here with settings.
    }
    public void QuitGameMenu()
    {
        DisableMenuButtons();
        Application.Quit();

        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }
    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        loadGameButton.interactable = false;
        optionsGameButton.interactable = false;
        quitGameButton.interactable = false;
    }

    public void LoadData(GameData data)
    {
        this.level = data.level;
    }
    public void SaveData(ref GameData data)
    {
        data.level = this.level;
    }

}
